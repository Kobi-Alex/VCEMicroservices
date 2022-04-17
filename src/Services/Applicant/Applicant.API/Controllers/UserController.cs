using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Applicant.API.Application.Pagginations;
using Applicant.API.Application.Services.Interfaces;
using Applicant.API.Application.Contracts.Dtos.UserDtos;


namespace Applicant.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {

        private readonly IServiceManager _serviceManager;
        public UsersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<IActionResult> GetUsers(int page, string filter, string role, int middleVal = 10, 
            int cntBetween = 5, int limit = 15, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("\n---> Getting All Users...");
            var users = await _serviceManager.UserService.GetAllAsync(cancellationToken);

            if (!String.IsNullOrEmpty(filter))
            {
                users = users.Where(x => (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(filter.ToLower())
                || x.Email.ToLower().Contains(filter.ToLower()));
            }

            if (!String.IsNullOrEmpty(role))
            {
                users = users.Where(x => x.Roles.ToLower()
                .Contains(role.ToLower()));
            }

            if (middleVal <= cntBetween)
            {
                return BadRequest(new { Error = "MiddleVal must be more than cntBetween" });
            }


            return Ok(Paggination<UserReadDto>.GetData(currentPage: page, limit: limit, itemsData: users, 
                middleVal: middleVal, cntBetween: cntBetween));
        }


        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserById(string id, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"\n---> Getting User by Id: {id}");
            var userDto = await _serviceManager.UserService.GetByIdAsync(id, cancellationToken);

            return Ok(userDto);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"\n---> Creating new User: {userCreateDto.Email}");
                var userDto = await _serviceManager.UserService.CreateAsync(userCreateDto);

                return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
            }

            Console.WriteLine($"\n---> Invalid payload");
            return BadRequest(GetModelStateErrors(ModelState.Values));
        }


        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUser(string id, UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"\n---> Updating user {id} ...");
                await _serviceManager.UserService.UpdateAsync(id, userUpdateDto);

                return NoContent();
            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }


        [HttpPost]
        [Route("UpdateEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateEmail(UserChangeEmailDto userChangeEmailDto)
        {
            Console.WriteLine("\n---> Update Email");
            await _serviceManager.UserService.UpdateEmailAsync(userChangeEmailDto);
          
            return Ok();
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceManager.UserService.ChangePassword(userChangePasswordDto);
                return NoContent();
            }

            Console.WriteLine($"\n---> Invalid data");
            return BadRequest(GetModelStateErrors(ModelState.Values));
        }


        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            Console.WriteLine($"\n---> Delete User: {id} ....");
            await _serviceManager.UserService.DeleteAsync(id);

            return NoContent();
        }


        [HttpPost]
        [Route("AddRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AddRole(UserRoleDto userRoleDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceManager.UserService.AddRoleAsync(userRoleDto);
                return NoContent();
            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }


        [HttpPost]
        [Route("RemoveRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RemoveRole(UserRoleDto userRoleDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceManager.UserService.RemoveRoleAsync(userRoleDto);
                return NoContent();
            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }


        [HttpGet]
        [Route("{id}/Exams")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager, Student")]
        public async Task<IActionResult> GetUserExams(string id, int page, int limit)
        {
            var userExams = await _serviceManager.UserService.GetUserExamsAsync(id);
            return Ok(Paggination<UserExamDto>.GetData(currentPage: page, limit: limit, itemsData: userExams));
        }


        [HttpPost]
        [Route("AddExam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public async Task<IActionResult> AddExamToUser(UserExamDto userExamDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceManager.UserService.AddExamToUserAsync(userExamDto);
                return Ok();
            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }


        [HttpPost]
        [Route("RemoveExam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager,Student")]
        public async Task<IActionResult> RemoveExamFromUser(UserExamDto userExamDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceManager.UserService.RemoveExamFromUser(userExamDto);
                return Ok();
            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }

        [HttpGet]
        [Route("e/{idExam}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]

        public async Task<IActionResult> GetExamUsers(int idExam, int page,  int middleVal = 10,
            int cntBetween = 5, int limit = 15, CancellationToken cancellationToken = default)
        {
            var users = await _serviceManager.UserService.GetExamUsersAsync(idExam, cancellationToken);


            return Ok(Paggination<UserExamReadDto>.GetData(currentPage: page, limit: limit, itemsData: users,
                middleVal: middleVal, cntBetween: cntBetween));
            //return Ok(users);
        }

        [HttpPost]
        [Route("SendMessage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SendMessage([FromBody] UserEmailDto userChangeEmail)
        {
            //Send message on the email

            if (ModelState.IsValid)
            {
                Console.WriteLine($"\n---> Send Access code ...");
                await _serviceManager.UserService.AccessCodeAsync(userChangeEmail);

                return Ok();
            }

            return BadRequest(new
            {
                Error = "Invalid data"
            });

        }
        /// <summary>
        /// Gets all modelstate errors
        /// </summary>
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