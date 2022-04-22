using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using Applicant.Domain.Entities;
using Applicant.Domain.Repositories;
using Applicant.API.Application.Exceptions;
using Applicant.API.Application.Configurations;
using Applicant.API.Application.Services.Interfaces;
using Applicant.API.Application.Contracts.Dtos.UserDtos;
using Applicant.API.Application.Contracts.Dtos.AuthDtos;

using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Applicant.API.Application.Contracts.Infrastructure;
using Applicant.API.Application.Models;

namespace Applicant.API.Application.Services
{
    internal sealed class AccessCodeService : IAccessCodeService
    {
        private readonly IMapper _mapper;
        private readonly JwtConfig _jwtConfig;
        private readonly IEmailService _emailService;
        private readonly IRepositoryManager _repositoryManager;

        private PasswordHasher<User> _hasher;
        private Random _randomNumbers = new Random();

        public AccessCodeService(IRepositoryManager repositoryManager, IMapper mapper, IOptionsMonitor<JwtConfig> optionsMonitor, IEmailService emailService)
        {
            _mapper = mapper;
            _emailService = emailService;
            _hasher = new PasswordHasher<User>();
            _repositoryManager = repositoryManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }


        public async Task<AuthResult> RegisterUserAsync(AuthRegisterDto authRegisterDto, CancellationToken cancellationToken = default)
        {
            var user = await _repositoryManager.UserRepository.GetByEmailAsync(authRegisterDto.Email);

            if (user != null)
            {
                throw new EmailAlreadyInUseException();
            }

            //var userCodes = _context.AccessCodes.Where(x => x.Email == user.Email).OrderByDescending(x => x.ExpiryDate).ToList();
            var userCodes = await _repositoryManager.AccessCodeRepository.GetAllByEmail(authRegisterDto.Email);

            if (userCodes.ToList().Count == 0)
            {
                await AccessCodeAsync(authRegisterDto);
                return null;
                //throw new AccessCodeNotFoundException();
            }

            if (userCodes.ToList()[0].Code != authRegisterDto.Code)
            {
                throw new AccessCodeIncorrectException();
            }

            var newUser = new User()
            {
                FirstName = authRegisterDto.FirstName,
                LastName = authRegisterDto.LastName,
                Email = authRegisterDto.Email,
                Password = _hasher.HashPassword(null, authRegisterDto.Password),
                AdditionalInfo = authRegisterDto.AdditionalInfo,
                CreatedAt = new DateTimeOffset(DateTime.Now),
                UpdatedAt = new DateTimeOffset(DateTime.Now)
            };

            var role = await _repositoryManager.RoleRepository.GetByName("Student");

            if (role != null)
            {
                newUser.Roles.Add(role);
            }

            _repositoryManager.UserRepository.Insert(newUser);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            var auth = await GenerateJwtToken(newUser);

            Console.WriteLine($"\n---> New user: {auth.User.Email}");

            _repositoryManager.AccessCodeRepository.RemoveRange(userCodes);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return auth;
        }


        public async Task<AuthResult> LoginUserAsync(AuthLoginDto authLoginDto, CancellationToken cancellationToken = default)
        {

            var user = await _repositoryManager.UserRepository.GetByEmailAsync(authLoginDto.Email);

            if (user is null)
            {
                throw new InvalidLoginException();
            }

            var isCorrect = CheckPassword(user, authLoginDto.Password);

            if (!isCorrect)
            {
                throw new InvalidLoginException();
            }

            var jwtToken = await GenerateJwtToken(user);
            Console.WriteLine($"\n---> Login: {user.Id} | Date: {DateTime.UtcNow}");

            return jwtToken;
        }


        public async Task<AuthResult> RefreshTokenAsync(AuthTokenRequestDto tokenRequest, CancellationToken cancellationToken = default)
        {
            var result = await VerifyAndGenerateToken(tokenRequest);

            if (result is null)
            {
                throw new InvalidTokenException();
            }

            return result;

        }


        public async Task AccessCodeAsync(AuthRegisterDto authRegisterDto, CancellationToken cancellationToken = default)
        {

            var user = await _repositoryManager.UserRepository.GetByEmailAsync(authRegisterDto.Email);

            if (user != null)
            {
                throw new EmailAlreadyInUseException();
            }

            Console.WriteLine("\n---> Send Access code"); ;

            var accessCode = _randomNumbers.Next(100000, 999999);

            Email email = new Email();
            email.To= authRegisterDto.Email;
            email.Subject = "Access code";
            email.Body = $"<h1>Your access code: {accessCode}</h1>";

            await _emailService.SendEmail(email);

            //using (MailMessage mail = new MailMessage())
            //{
            //    mail.From = new MailAddress(_emailConfig.From, "It step Administration"); ;
            //    mail.To.Add(authRegisterDto.Email);
            //    mail.Subject = "Access code";
            //    mail.Body = $"<h1>Your access code: {accessCode}</h1>";
            //    mail.IsBodyHtml = true;

            //    using (SmtpClient smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port))
            //    {
            //        smtp.Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password);//Real email and password

            //        smtp.EnableSsl = true;
            //        smtp.Send(mail);
            //    }
            //}

            var newAC = new AccessCode()
            {
                Code = accessCode,
                Email = authRegisterDto.Email,
                ExpiryDate = new DateTimeOffset(DateTime.Now).AddMinutes(30)
            };

            _repositoryManager.AccessCodeRepository.Create(newAC);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
            //_context.AccessCodes.Add(new AccessCode() { Code = accessCode, Email = emailRequest.Email, ExpiryDate = new DateTimeOffset(DateTime.Now).AddMinutes(30) });
            //_context.SaveChanges();
        }

        private bool CheckPassword(User user, string password)
        {
            var res = _hasher.VerifyHashedPassword(null, user.Password, password);

            switch (res)
            {
                case PasswordVerificationResult.Failed:
                    return false;
                case PasswordVerificationResult.Success:
                    return true;
                case PasswordVerificationResult.SuccessRehashNeeded:
                    return true;
                default:
                    return false;
            }
        }

        private async Task<AuthResult> VerifyAndGenerateToken(AuthTokenRequestDto tokenRequest)
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
                        Error = "Token has not yet expired"

                    };
                }

                var storedToken = await _repositoryManager.RefreshTokenRepository.Get(tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = "Token does not exist"

                    };
                }

                // Validation 5 - validate if used
                if (storedToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = "Token has been used"
                    };
                }

                // Validation 6 - validate if revoked
                if (storedToken.IsRevorked)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = "Token has been revoked"
                    };
                }

                // Validation 7 - validate the id
                var jti = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = "Token doesn't match"
                    };
                }

                // update current token 
                storedToken.IsUsed = true;

                _repositoryManager.RefreshTokenRepository.Delete(storedToken);
                await _repositoryManager.UnitOfWork.SaveChangesAsync();

                // Generate a new token
                var dbUser = await _repositoryManager.UserRepository.GetByIdAsync(storedToken.UserId);

                return await GenerateJwtToken(dbUser);

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = "Token has expired please re-login"
                    };

                }
                else
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = $"Something went wrong: {ex.Message}"
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

        private async Task<AuthResult> GenerateJwtToken(User user, CancellationToken cancellationToken = default)
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


            _repositoryManager.RefreshTokenRepository.Create(refreshToken);

            var userReadDto = _mapper.Map<UserReadDto>(user);

            if (user.Roles != null)
            {
                userReadDto.Roles = String.Join(",", user.Roles.ToArray().Select(x => x.Name));
            }

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

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