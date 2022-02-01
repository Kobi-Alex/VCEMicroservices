using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(IMapper mapper, IUserRepo repository, RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _mapper = mapper;
            _roleManager = roleManager;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers()
        {
            Console.WriteLine("\n---> Getting All Users...");

            var users = await _repository.GetUsersAsync();

            var mapper = _mapper.Map<IEnumerable<UserReadDto>>(users);

            foreach (var item in users)
            {
                var user = mapper.FirstOrDefault(x => x.Id == item.Id);

                if (user != null)
                {
                    user.Roles = await _repository.GetRolesUserAsync(item);
                }
            }

            return Ok(mapper);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserReadDto>> GetUserById(string id)
        {
            Console.WriteLine($"\n---> Getting User by Id: {id}");


            var user = await _repository.GetUserByIdAsync(id);

            if (user == null) return NotFound();

            var mapper = _mapper.Map<UserReadDto>(user);
            mapper.Roles = await _repository.GetRolesUserAsync(user);
            
            return Ok(mapper);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto user)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"\n---> Creating new User: {user.Email}");
                var existingUser = await _repository.GetUserByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    Console.WriteLine($"\n---> Could not create user for {user.Email} | Email already in use");

                    return BadRequest(new
                    {
                        Errors = new List<string>()
                        {
                            "Email already in use"
                        },
                        Success = false
                    });
                }


                var newUser = new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    AdditionalInfo = user.AdditionaInfo != null ? user.AdditionaInfo : "",
                    Email = user.Email,
                    UserName = user.UserName,
                    NormalizedEmail = user.Email.ToUpper(),
                    NormalizedUserName = user.UserName.ToUpper(),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now)
                };

                var result = await _repository.CreateUserAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    Console.WriteLine($"\n---> New user: {newUser.Id}");

                    return Ok(_mapper.Map<UserReadDto>(newUser));
                }
                else
                {
                    Console.WriteLine($"\n---> Could not create user: {user.Email}");

                    return BadRequest(new
                    {
                        Success = false,
                        Errors = result.Errors.Select(x => x.Description).ToList(),
                    });
                }
            }

            Console.WriteLine($"\n---> Invalid payload");

            var modelErrors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }

            return BadRequest(new
            {
                Errors = modelErrors,
                Success = false
            });
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUser(string id, UserUpdateDto user)
        {
            if (ModelState.IsValid)
            {
                if (id != user.Id)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"User Id mismatch" }
                    });
                }

                var userExists = await _repository.GetUserByIdAsync(id);

                if (userExists == null) return NotFound();

                var userEmailExitss = await _repository.GetUserByEmailAsync(user.Email);

                if (userEmailExitss != null && userEmailExitss.Id != id)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"Email: {user.Email} is already used" }
                    });
                }

                var result = await _repository.UpdateUserAsync(id, user);

                if (result.Succeeded)
                {
                    Console.WriteLine($"\n---> Update User: {id}");

                    _repository.SaveChanges();
                    return NoContent();
                }
                else
                {
                    Console.WriteLine($"\n---> Could not update user: {id}");


                    return BadRequest(new
                    {
                        Success = false,
                        Errors = result.Errors.Select(x => x.Description).ToList()
                    });
                }
            }

            var modelErrors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }

            return BadRequest(new
            {
                Errors = modelErrors,
                Success = false
            });
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(UserChangePassword userChangePassword)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.GetUserByIdAsync(userChangePassword.IdUser) == null)
                {
                    return NotFound();
                }

                var result = await _repository.ChangePasswordAsync(userChangePassword);

                if (result.Succeeded)
                {
                    Console.WriteLine($"\n---> Change password for user: {userChangePassword.IdUser}");

                    return NoContent();
                }
                else
                {
                    Console.WriteLine($"\n---> Could not change password for user: {userChangePassword.IdUser}");

                    return BadRequest(new
                    {
                        Success = false,
                        Errors = result.Errors.Select(x => x.Description).ToList()
                    });
                }
            }

            Console.WriteLine($"\n---> Invalid data");

            return BadRequest(new
            {
                Errors = new List<string>()
                {
                    "Invalid data"
                },
                Success = false
            });
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            Console.WriteLine($"\n---> Delete User: {id} ....");
            var user = await _repository.GetUserByIdAsync(id);

            if (user == null) return NotFound();

            var result = await _repository.DeleteUserAsync(id);

            if (result)
            {
                Console.WriteLine($"\n---> Deleted User: {id} successful");

                return Ok();
            }

            Console.WriteLine($"\n---> Could not delete user: {id}");

            return BadRequest(new
            {
                Success = false,
                Errors = new List<string>() { "Could not delete user!" }
            });
        }

        [HttpPost]
        [Route("AddRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]

        public async Task<IActionResult> AddRole(UserRoleDto userRoleDto)
        {
            if (ModelState.IsValid)
            {
                var existsUser = await _repository.GetUserByIdAsync(userRoleDto.UserId);

                if (existsUser == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"User: <{userRoleDto.UserId}> doesn't exists" }
                    });
                }

                var roleExists = await _roleManager.FindByNameAsync(userRoleDto.Role);

                if (roleExists == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"Role: <{userRoleDto.Role}> doesn't exists" }
                    });
                }

                var result = await _repository.AddRoleToUserAsync(userRoleDto);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = result.Errors.Select(x => x.Description).ToList(),
                    });
                }
            }

            var modelErrors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }

            return BadRequest(new
            {
                Errors = modelErrors,
                Success = false
            });
        }
        [HttpPost]
        [Route("RemoveRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]

        public async Task<IActionResult> RemoveRole(UserRoleDto userRoleDto)
        {
            if (ModelState.IsValid)
            {
                var existsUser = await _repository.GetUserByIdAsync(userRoleDto.UserId);

                if (existsUser == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"User: <{userRoleDto.UserId}> doesn't exists" }
                    });
                }

                var roleExists = await _roleManager.FindByNameAsync(userRoleDto.Role);

                if (roleExists == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"Role: <{userRoleDto.Role}> doesn't exists" }
                    });
                }

                var result = await _repository.DeleteRoleFromUserAsync(userRoleDto);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = result.Errors.Select(x => x.Description).ToList(),
                    });
                }
            }

            var modelErrors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }

            return BadRequest(new
            {
                Errors = modelErrors,
                Success = false
            });
        }
    }
}
