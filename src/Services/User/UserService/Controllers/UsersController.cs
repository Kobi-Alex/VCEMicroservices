using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UserService.Data.Interfaces;
using UserService.Dto.User;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IRoleRepository roleRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("\n---> Getting All Users...");

            var users = await _userRepository.GetAllAsync(cancellationToken);

            var userReadDto = _mapper.Map<IEnumerable<UserReadDto>>(users);

            foreach (var item in users)
            {
                if(item.Roles!=null)
                {
                    userReadDto.FirstOrDefault(x=>x.Id == item.Id).Roles = String.Join(", ", item.Roles.ToArray().Select(x => x.Name).ToArray());
                }
            }


            return Ok(userReadDto);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserReadDto>> GetUserById(int id, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"\n---> Getting User by Id: {id}");


            var user = await _userRepository.GetByIdAsync(id, cancellationToken);

            if (user == null) return NotFound();

            var mapper = _mapper.Map<UserReadDto>(user);
            //mapper.Roles = await _repository.GetRolesUserAsync(user);

            return Ok(mapper);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto user)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"\n---> Creating new User: {user.Email}");
                var existingUser = await _userRepository.GetByEmailAsync(user.Email);

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

                var newUser = new User();

                _mapper.Map(user,newUser);

                _userRepository.Create(newUser, user.Password);
               await _userRepository.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, _mapper.Map<UserReadDto>(newUser));
            }

            Console.WriteLine($"\n---> Invalid payload");

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUser(int id, UserUpdateDto user)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"\n---> Updating user {id} ...");

                var userExists = await _userRepository.GetByIdAsync(id);

                if (userExists == null) return NotFound();

                var userEmailExitss = await _userRepository.GetByEmailAsync(user.Email);

                if (userEmailExitss != null && userEmailExitss.Id != id)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"Email: {user.Email} is already used" }
                    });
                }

                _mapper.Map(user, userExists);
                userExists.UpdatedAt = new DateTimeOffset(DateTime.Now);

                _userRepository.Update(userExists);
                await _userRepository.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(UserChangePasswordDto userChangePassword)
        {
            if (ModelState.IsValid)
            {
                var existsUser = await _userRepository.GetByIdAsync(userChangePassword.IdUser);
                if (existsUser == null)
                {
                    return NotFound();
                }

                if(!_userRepository.CheckPassword(existsUser, userChangePassword.CurrentPassword))
                {
                    return BadRequest(new
                    {
                        Error = new List<string> { "Incorect password" }
                    });
                }


                Console.WriteLine($"\n---> Changing password user: {userChangePassword.IdUser}");

                _userRepository.ChangePassword(existsUser, userChangePassword.NewPassword);
                await _userRepository.SaveChangesAsync();

                return NoContent();
            }

            Console.WriteLine($"\n---> Invalid data");

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            Console.WriteLine($"\n---> Delete User: {id} ....");
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null) return NotFound();

            var admins = await _userRepository.GetAdmins();

            if (admins.Count() <= 1)
            {
                return BadRequest(new
                {
                    Error = new List<string> { "Could not delete user." }
                });
            }
       
            _userRepository.Delete(user);

            await _userRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("AddRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]

        public async Task<IActionResult> AddRole(UserRoleDto userRoleDto)
        {
            if (ModelState.IsValid)
            {
                var existsUser = await _userRepository.GetByIdAsync(userRoleDto.UserId);

                if (existsUser == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"User: <{userRoleDto.UserId}> doesn't exists" }
                    });
                }

                var roleExists = await _roleRepository.GetByName(userRoleDto.Role);

                if (roleExists == null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string> { $"Role: <{userRoleDto.Role}> doesn't exists" }
                    });
                }


                if(existsUser.Roles.FirstOrDefault(x=>x.Id == roleExists.Id)!=null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string> { $"User has role: {roleExists.Name}" }
                    });
                }

                Console.WriteLine($"\n---> Add role: {roleExists.Name} to: {existsUser.Id}");

                existsUser.Roles.Add(roleExists);
                _userRepository.Update(existsUser);
                await _userRepository.SaveChangesAsync();

                return NoContent();

            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }
        [HttpPost]
        [Route("RemoveRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]

        public async Task<IActionResult> RemoveRole(UserRoleDto userRoleDto)
        {
            if (ModelState.IsValid)
            {
                var existsUser = await _userRepository.GetByIdAsync(userRoleDto.UserId);

                if (existsUser == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"User: <{userRoleDto.UserId}> doesn't exists" }
                    });
                }

                var roleExists = await _roleRepository.GetByName(userRoleDto.Role);

                if (roleExists == null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string> { $"Role: <{userRoleDto.Role}> doesn't exists" }
                    });
                }


                if (existsUser.Roles.FirstOrDefault(x => x.Id == roleExists.Id) == null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string> { $"User doesn't have role: {roleExists.Name}" }
                    });
                }
                Console.WriteLine($"\n---> Delete role: {roleExists.Name} from: {existsUser.Id}");
                existsUser.Roles.Remove(roleExists);
                _userRepository.Update(existsUser);
                await _userRepository.SaveChangesAsync();

                return NoContent();

            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }
        
        private List<string> GetModelStateErrors(IEnumerable<ModelStateEntry> modelState)
        {
            var modelErrors = new List<string>();
            foreach (var ms in modelState)
            {
                foreach (var modelError in ms.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }

            return modelErrors;
        }
    }
}
