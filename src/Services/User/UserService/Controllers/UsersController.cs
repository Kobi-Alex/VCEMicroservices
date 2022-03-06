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
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UserService.Configuration;
using UserService.Data;
using UserService.Data.Interfaces;
using UserService.Data.Paggination;
using UserService.Dto.User;
using UserService.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAccessCodeRepository _accessCodeRepository;
        private readonly IMapper _mapper;
        private readonly EmailConfiguration _emailConfig;
        private Random randomNumbers = new Random();
        private Random randomPassword = new Random();

        public UsersController(IUserRepository userRepository, IRoleRepository roleRepository, IAccessCodeRepository accessCodeRepository, IMapper mapper, EmailConfiguration emailConfig)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _accessCodeRepository = accessCodeRepository;

            _emailConfig = emailConfig;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers(int page, string filter, string role, int limit = 15, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("\n---> Getting All Users...");

            var users = await _userRepository.GetAllAsync(cancellationToken);

            var userReadDto = _mapper.Map<IEnumerable<UserReadDto>>(users);

            foreach (var item in users)
            {
                if (item.Roles != null)
                {
                    userReadDto.FirstOrDefault(x => x.Id == item.Id).Roles = String.Join(",", item.Roles.ToArray().Select(x => x.Name).ToArray());
                }
            }

            if (!String.IsNullOrEmpty(filter))
            {
                userReadDto = userReadDto.Where(x => x.FirstName.ToLower().Contains(filter.ToLower())
                || x.LastName.ToLower().Contains(filter.ToLower())
                || x.Email.ToLower().Contains(filter.ToLower()));
            }

            if (!String.IsNullOrEmpty(role))
            {
                userReadDto = userReadDto.Where(x => x.Roles.ToLower().Contains(role.ToLower()));
            }

            //var list = new List<UserReadDto>();

            //string[] roles = new string[4] { "Student", "Admin", "Teacher", "Manager" };

            //int k = 0;

            //for (int i = 0; i < 100; i++)
            //{
            //    list.Add(new UserReadDto { Id = i.ToString(), Roles = roles[k], Email = $"user{i}@google.com", FirstName = $"User{i}", LastName = $"L{i}", CreatedAt = DateTime.Now });
            //    k++;
            //    if (k >= roles.Length) k = 0;
            //}



            //return Ok(userReadDto);

            return Ok(Pagination<UserReadDto>.GetData(page, limit, userReadDto));


            //var list = new List<UserReadDto>();

            //for (int i = 0; i < 500; i++)
            //{
            //    list.Add(new UserReadDto() { FirstName = $"User: {i}", AdditionalInfo = $"Additional Info user: {i}", Email = $"csuser{i}@google.com", LastName = $"Smith: {i}", Id = Guid.NewGuid().ToString(), Roles = "Student" });
            //}
            //return Ok(list);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserReadDto>> GetUserById(string id, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"\n---> Getting User by Id: {id}");

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);

            if (user == null) return NotFound();

            var mapper = _mapper.Map<UserReadDto>(user);
            mapper.Roles = String.Join(",", user.Roles.ToArray().Select(x => x.Name).ToArray());

            return Ok(mapper);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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

                Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$"); // for password
                bool isCorrectPassword = false;
                var newUser = new User();
                string password = RandomString(10);

                while (!isCorrectPassword)
                {
                    if (regex.IsMatch(password))
                    {
                        isCorrectPassword = true;
                    }
                    else
                    {
                        password = RandomString(10);
                    }
                }

                _mapper.Map(user, newUser);

                _userRepository.Create(newUser, password);
                await _userRepository.SaveChangesAsync();

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_emailConfig.From, "It step Administration"); ;
                    mail.To.Add(user.Email);
                    mail.Subject = "Data";
                    mail.IsBodyHtml = true;
                    mail.Body = $"</h1>Your email: {user.Email} | Password: {password}</h1>";
                    //mail.Attachments.Add(new Attachment("D:\\Aloha.7z"));//--Uncomment this to send any attachment  

                    // SmtpClient клас з за до якого можна відправити лист

                    using (SmtpClient smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port))
                    {
                        smtp.Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password);//Real email and password

                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, _mapper.Map<UserReadDto>(newUser));
            }

            Console.WriteLine($"\n---> Invalid payload");

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutUser(string id, UserUpdateDto user)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"\n---> Updating user {id} ...");

                var userExists = await _userRepository.GetByIdAsync(id);

                if (userExists == null) return NotFound();

                //var userEmailExitss = await _userRepository.GetByEmailAsync(user.Email);

                //if (userEmailExitss != null && userEmailExitss.Id != id)
                //{
                //    return BadRequest(new
                //    {
                //        Success = false,
                //        Errors = new List<string> { $"Email: {user.Email} is already used" }
                //    });
                //}

                _mapper.Map(user, userExists);
                userExists.UpdatedAt = new DateTimeOffset(DateTime.Now);

                _userRepository.Update(userExists);
                await _userRepository.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }

        [HttpPost]
        [Route("UpdateEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> UpdateEmail(UserChangeEmailDto userChangeEmailDto)
        {
            var user = await _userRepository.GetByIdAsync(userChangeEmailDto.Id);

            if (user == null) return NotFound();

            var userCodes = await _accessCodeRepository.GetByEmail(userChangeEmailDto.Email);

            if (userCodes.Count() == 0)
            {
                return NotFound();
            }

            if (userCodes.ToList()[0].Code != userChangeEmailDto.AccessCode)
            {
                return BadRequest(new { Errors = new List<string>() { "Incorect access code" } });
            }


            _userRepository.ChangeEmail(user, userChangeEmailDto.Email);
            await _userRepository.SaveChangesAsync();

            _accessCodeRepository.RemoveByEmail(userChangeEmailDto.Email);
            await _accessCodeRepository.SaveChangesAsync();


            Console.WriteLine("\n---> Update Email");

            return Ok();
        }

        [HttpPost]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] UserEmailDto userChangeEmail)
        {

            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetByIdAsync(userChangeEmail.Id);

                if (existingUser == null) return NotFound();


                var userEmailExitss = await _userRepository.GetByEmailAsync(userChangeEmail.Email);

                if (userEmailExitss != null && userEmailExitss.Id != userChangeEmail.Id)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Errors = new List<string> { $"Email: {userChangeEmail.Email} is already used" }
                    });
                }

                if (userEmailExitss != null && userEmailExitss.Id == userChangeEmail.Id)
                {
                    return NoContent();

                }


                Console.WriteLine("\n---> Send Access code"); ;

                var accessCode = randomNumbers.Next(100000, 999999);

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_emailConfig.From, "It step Administration"); ;
                    mail.To.Add(userChangeEmail.Email);
                    mail.Subject = "Access code";
                    mail.IsBodyHtml = true;
                    mail.Body = $"</h1>Your access code: {accessCode}</h1>";
                    //mail.Attachments.Add(new Attachment("D:\\Aloha.7z"));//--Uncomment this to send any attachment  

                    // SmtpClient клас з за до якого можна відправити лист

                    using (SmtpClient smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port))
                    {
                        smtp.Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password);//Real email and password

                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                _accessCodeRepository.Create(new AccessCode() { Code = accessCode, Email = userChangeEmail.Email, ExpiryDate = new DateTimeOffset(DateTime.Now).AddMinutes(30) });
                await _accessCodeRepository.SaveChangesAsync();
                //_context.AccessCodes.Add(new AccessCode() { Code = accessCode, Email = emailRequest.Email, ExpiryDate = new DateTimeOffset(DateTime.Now).AddMinutes(30) });
                //_context.SaveChanges();

                return Ok();
            }

            return BadRequest(new
            {
                Error = "Invalid data"
            });

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

                if (!_userRepository.CheckPassword(existsUser, userChangePassword.CurrentPassword))
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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


                if (existsUser.Roles.FirstOrDefault(x => x.Id == roleExists.Id) != null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string> { $"User has role: {roleExists.Name}" }
                    });
                }

                if (roleExists.Name == "Student")
                {
                    if (existsUser.Roles.Any(x => x.Name == "Admin") || existsUser.Roles.Any(x => x.Name == "Manager") || existsUser.Roles.Any(x => x.Name == "Teacher"))
                    {

                        return BadRequest(new { Error = new List<string>() { $"Cannot add role: < {roleExists.Name} > to user with roles < Admin, Manager or Teacher >" } });
                    }
                }
                else
                {
                    if (existsUser.Roles.Any(x => x.Name == "Student"))
                    {
                        return BadRequest(new { Error = new List<string>() { $"Cannot add role: < {roleExists.Name} > to user with role < Student >" } });
                    }
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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

        [HttpGet]
        [Route("Exams/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]

        public async Task<IActionResult> GetUserExams(string id, int page, int limit)
        {
            //_mapper.Map<IEnumerable<UserReadDto>>(users);
            //var userExams = _mapper. await _userRepository.GetUserExams(id);
            var userExams = _mapper.Map<IEnumerable<UserExamDto>>(await _userRepository.GetUserExams(id));
            return Ok(Pagination<UserExamDto>.GetData(page,limit,userExams));
        }


        [HttpPost]
        [Route("AddExam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public async Task<IActionResult> AddExamToUser(UserExamDto userExamDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetByIdAsync(userExamDto.UserId);

                if (user == null)
                {
                    return NotFound(new
                    {
                        Errors = new List<string> { $"User < {userExamDto.UserId} > doesn't exists" }
                    });
                }

                if (user.Roles.FirstOrDefault(x => x.Name == "Student") == null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string> { $"Could not add exam to user" }
                    });
                }

                var userExam = await _userRepository.GetUserExamAsync(userExamDto);

                if (userExam != null)
                {
                    return BadRequest(new
                    {
                        Errors = new List<string> { $"Exam < {userExamDto.ExamId} > is already exists" }
                    });
                }

                _userRepository.AddExamToUser(new UserExams() { ExamId = userExamDto.ExamId, UserId = userExamDto.UserId });
                await _userRepository.SaveChangesAsync();

                return Ok();
            }
            
            return BadRequest(GetModelStateErrors(ModelState.Values));
        }

        [HttpPost]
        [Route("RemoveExam")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
        public async Task<IActionResult> RemoveExamFromUser(UserExamDto userExamDto)
        {
            if(ModelState.IsValid)
            {
                var userExam = await _userRepository.GetUserExamAsync(userExamDto);

                if (userExam == null) return NotFound();

                _userRepository.RemoveExamFromUser(userExam);
                await _userRepository.SaveChangesAsync();

                return Ok();
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

        private string RandomString(int length)
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYabcdefghijklmnpqrstuvwxy#$^+=!*()@%&0123456789";

            return new string(Enumerable.Repeat(chars, length).Select(x => x[randomPassword.Next(x.Length)]).ToArray());
        }
    }
}
