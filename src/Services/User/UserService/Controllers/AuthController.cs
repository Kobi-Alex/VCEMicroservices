﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Configuration;
using UserService.Data;
using UserService.Data.Interfaces;
using UserService.Dto.Auth;
using UserService.Dto.User;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtConfig _jwtConfig;
        private readonly IMapper _mapper;

        public AuthController(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IRoleRepository roleRepository,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _roleRepository = roleRepository;
            _jwtConfig = optionsMonitor.CurrentValue;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto user)
        {
            if (ModelState.IsValid)
            {
                //We can utilise the model

                var existingUser = await _userRepository.GetByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new
                    {
                        Error = new List<string>()
                        {
                            "Email already in use"
                        },
                    });
                }


                var newUser = new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    AdditionalInfo = user.AdditionaInfo,
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now)
                };

                var userRole = await _roleRepository.GetByName("Student");

                if (userRole != null)
                {
                    newUser.Roles.Add(userRole);
                }

                _userRepository.Create(newUser, user.Password);

                await _userRepository.SaveChangesAsync();

                var auth = await GenerateJwtToken(newUser);
               
                return Ok(auth);
               
            }

            return BadRequest(new
            {
                Error = new List<string>()
                {
                    "Invalid payload"
                },
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new 
                    {
                        Error = new List<string>()
                        {
                            "Invalid login request"
                        },
                    });
                }

                var isCorrect =  _userRepository.CheckPassword(existingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new
                    {
                        Error = new List<string>()
                        {
                            "Invalid login request",
                        },
                    });
                }

                var jwtToken = await GenerateJwtToken(existingUser);


                Console.WriteLine($"\n---> Login: {existingUser.Id} | Date: {DateTime.UtcNow}");

                return Ok(jwtToken);
            }

            return BadRequest(new 
            {
                Error = new List<string>()
                {
                    "Invalid payload"
                },
            });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await VerifyAndGenerateToken(tokenRequest);

                if (result == null)
                {
                    return BadRequest(new 
                    {
                        Error = new List<string>()
                        {
                            "Invalid tokens"
                        },
                    });
                }

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return BadRequest(new 
            {
                Error = new List<string>()
                {
                    "Invalid payload"
                },
            });
        }

        private async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(tokenRequest.Token);
                var token = jsonToken as JwtSecurityToken;

                var result = token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                // Validation 2 - Validate encryption alg
                if (result == false)
                {
                    return null;
                }

                var utcExpiryDate = long.Parse(token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                var utcNow = ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds);

                var s = UnixTimeStampToDateTime(utcNow);

                if (UnixTimeStampToDateTime(utcExpiryDate) > UnixTimeStampToDateTime(utcNow))
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>() {
                            "Token has not yet expired"
                        }
                    };
                }

                var storedToken = await _refreshTokenRepository.Get(tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>() {
                            "Token does not exist"
                        }
                    };
                }

                // Validation 5 - validate if used
                if (storedToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>() {
                            "Token has been used"
                        }
                    };
                }

                // Validation 6 - validate if revoked
                if (storedToken.IsRevorked)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>() {
                            "Token has been revoked"
                        }
                    };
                }

                // Validation 7 - validate the id
                var jti = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>() {
                            "Token doesn't match"
                        }
                    };
                }

                // update current token 

                storedToken.IsUsed = true;

                _refreshTokenRepository.Delete(storedToken);
                await _refreshTokenRepository.SaveChangesAsync();


                // Generate a new token
                User dbUser = null;
                return await GenerateJwtToken(dbUser);

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {

                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>() {
                            "Token has expired please re-login"
                        }
                    };

                }
                else
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>() {
                            "Something went wrong."
                        }
                    };
                }
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToLocalTime();

            return dateTimeVal;
        }

        private async Task<AuthResult> GenerateJwtToken(User user)
        {
            //Now its ime to define the jwt token which will be responsible of creating our tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            //We get our secret from the appsettings
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            //var userRoles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("IdUser", user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, ((int)(DateTime.UtcNow.AddMinutes(1).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds).ToString()));

            //userRoles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
            user.Roles.ToList().ForEach(r => claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, r.Name)));

            // we define our token descriptor
            // We need to utilise claims which are properties in our token which gives information about the token
            // which belong to the specific user who it belongs to
            // so it could contain their id, name, email the good part is that these information
            // are generated by our server and identity framework which is valid and trusted

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                //Expires = DateTime.UtcNow.AddSeconds(10),

                // here we are adding the encryption alogorithim information which will be used to decrypt our token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(35) + Guid.NewGuid().ToString()
            };


            _refreshTokenRepository.Create(refreshToken);

            var userReadDto = _mapper.Map<UserReadDto>(user);

            if(user.Roles!=null)
            {
                userReadDto.Roles = String.Join(", ", user.Roles.ToArray().Select(x=>x.Name));
            }

            await _refreshTokenRepository.SaveChangesAsync();

            return new AuthResult()
            {
                User = userReadDto,
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length).Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
