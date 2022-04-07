using System;
using System.Net;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Applicant.Domain.Entities;
using Applicant.Domain.Repositories;
using Applicant.API.Application.Exceptions;
using Applicant.API.Application.Configurations;
using Applicant.API.Application.Services.Interfaces;
using Applicant.API.Application.Contracts.Dtos.UserDtos;

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Applicant.API.Grpc;
using System.Linq.Expressions;

namespace Applicant.API.Application.Services
{
    internal sealed class UserService : IUserService
    {
        private Random randomPassword = new Random();
        private Random randomNumbers = new Random(); // for secret code 

        private readonly IMapper _mapper;
        private PasswordHasher<User> _hasher;
        private readonly EmailConfiguration _emailConfig;
        private readonly IRepositoryManager _repositoryManager;
        private readonly ReportGrpcService _reportGrpcService;

        public UserService(IRepositoryManager repositoryManager, IMapper mapper, EmailConfiguration emailConfig, 
            ReportGrpcService reportGrpcService)
        {
            _mapper = mapper;
            _emailConfig = emailConfig;
            _hasher = new PasswordHasher<User>();
            _repositoryManager = repositoryManager;
            _reportGrpcService = reportGrpcService;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = await _repositoryManager.UserRepository.GetAllAsync(cancellationToken);
            var usersDto = _mapper.Map<IEnumerable<UserReadDto>>(users);

            foreach (var item in users)
            {
                if (item.Roles != null)
                {
                    usersDto.FirstOrDefault(x => x.Id == item.Id).Roles = String
                        .Join(",", item.Roles.ToArray().Select(x => x.Name).ToArray());
                }
            }


            return usersDto;
        }


        public async Task<UserReadDto> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {

            var user = await _repositoryManager.UserRepository.GetByIdAsync(id, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(id);
            }

            var userDto = _mapper.Map<UserReadDto>(user);
            userDto.Roles = String.Join(",", user.Roles.ToArray().Select(x => x.Name).ToArray());

            return userDto;
        }


        public async Task<UserReadDto> CreateAsync(UserCreateDto userCreateDto, CancellationToken cancellationToken = default)
        {

            var existingUser = await _repositoryManager.UserRepository.GetByEmailAsync(userCreateDto.Email);

            if (existingUser != null)
            {
                throw new EmailAlreadyInUseException(userCreateDto.Email);
            }

            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$"); // for password
            bool isCorrectPassword = false;
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

            var user = _mapper.Map<User>(userCreateDto);
            user.Password = _hasher.HashPassword(null, password);

            _repositoryManager.UserRepository.Insert(user);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);


            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailConfig.From, "It step Administration"); ;
                mail.To.Add(userCreateDto.Email);
                mail.Subject = "Data";
                mail.IsBodyHtml = true;
                mail.Body = $"</h1>Your email: {userCreateDto.Email} | Password: {password}</h1>";
                //mail.Attachments.Add(new Attachment("D:\\Aloha.7z"));//--Uncomment this to send any attachment  

                // SmtpClient ���� � �� �� ����� ����� ��������� ����

                using (SmtpClient smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password); //Real email and password

                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            return _mapper.Map<UserReadDto>(user);
        }


        public async Task UpdateAsync(string id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
        {
            var user = await _repositoryManager.UserRepository.GetByIdAsync(id, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(id);
            }

            _mapper.Map(userUpdateDto, user);
            user.CreatedAt = new DateTimeOffset(DateTime.Now);
            
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        public async Task UpdateEmailAsync(UserChangeEmailDto userChangeEmailDto, CancellationToken cancellationToken = default)
        {

            var user = await _repositoryManager.UserRepository.GetByIdAsync(userChangeEmailDto.Id, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(userChangeEmailDto.Id);
            }

            var userCodes = await _repositoryManager.AccessCodeRepository.GetAllByEmail(userChangeEmailDto.Email);

            if (userCodes.Count() == 0)
            {
                throw new AccessCodeNotFoundException();
            }

            if (userCodes.ToList()[0].Code != userChangeEmailDto.AccessCode)
            {
                throw new IncorrectAccessCodeException();
            }

            user.Email = userChangeEmailDto.Email;

            foreach (var item in userCodes)
            {
                _repositoryManager.AccessCodeRepository.Remove(item);
            }

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        public async Task<bool> SendMessageAsync(UserEmailDto userEmailDto, CancellationToken cancellationToken = default)
        {

            var user = await _repositoryManager.UserRepository.GetByIdAsync(userEmailDto.Id, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(userEmailDto.Id);
            }

            var userEmailExist = await _repositoryManager.UserRepository.GetByEmailAsync(userEmailDto.Email);

            if (userEmailExist != null && userEmailExist.Id != userEmailDto.Id)
            {
                throw new EmailAlreadyInUseException(userEmailDto.Email);
            }

            if (userEmailExist != null && userEmailExist.Id == userEmailDto.Id)
            {
                return false;
            }

            Console.WriteLine("\n---> Send Access code"); ;

            var accessCode = randomNumbers.Next(100000, 999999);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailConfig.From, "It step Administration"); ;
                mail.To.Add(userEmailDto.Email);
                mail.Subject = "Access code";
                mail.IsBodyHtml = true;
                mail.Body = $"</h1>Your access code: {accessCode}</h1>";
                //mail.Attachments.Add(new Attachment("D:\\Aloha.7z"));//--Uncomment this to send any attachment  

                // SmtpClient ���� � �� �� ����� ����� ��������� ����

                using (SmtpClient smtp = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password);//Real email and password

                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            _repositoryManager.AccessCodeRepository.Create(
                new AccessCode()
                {
                    Code = accessCode,
                    Email = userEmailDto.Email,
                    ExpiryDate = new DateTimeOffset(DateTime.Now).AddMinutes(30)
                });

             await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
            //_context.AccessCodes.Add(new AccessCode() { Code = accessCode, Email = emailRequest.Email, ExpiryDate = new DateTimeOffset(DateTime.Now).AddMinutes(30) });
            //_context.SaveChanges();

            return true;
        }


        public async Task ChangePassword(UserChangePasswordDto userChangePasswordDto, CancellationToken cancellationToken = default)
        {

            var user = await _repositoryManager.UserRepository.GetByIdAsync(userChangePasswordDto.IdUser, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(userChangePasswordDto.IdUser);
            }

            if (!CheckPassword(user, userChangePasswordDto.CurrentPassword))
            {
                throw new IncorrectPasswordException("Incorect password!!");
            }

            Console.WriteLine($"\n---> Changing password user: {userChangePasswordDto.IdUser}");

            user.Password = _hasher.HashPassword(null, userChangePasswordDto.NewPassword);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var user = await _repositoryManager.UserRepository.GetByIdAsync(id, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(id);
            }

            //If user have role Admin 
            var adminRole = await _repositoryManager.RoleRepository.GetByName("Admin");
            if(user.Roles.Contains(adminRole))
            {
                var admins = (await _repositoryManager.UserRepository.FindAllAsync(x => x.Roles.Where(x => x.Id == 1).Any())).ToList();

                if(admins.Count() <=1)
                {
                    throw new UserDeleteException();
                }
            }

            // gRPC service delete report by userId
            var reportResult = await _reportGrpcService.RemoveUserDataFromRepor(id);

            if (reportResult.Success)
            {
                _repositoryManager.UserRepository.Delete(user);
                await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new UserNotFoundException(user.Id, reportResult.Error);
            }

        }


        public async Task AddRoleAsync(UserRoleDto userRoleDto, CancellationToken cancellationToken = default)
        {

            var user = await _repositoryManager.UserRepository.GetByIdAsync(userRoleDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(userRoleDto.UserId);
            }

            var role = await _repositoryManager.RoleRepository.GetByName(userRoleDto.Role);

            if (role is null)
            {
                throw new RoleNotFoundException(userRoleDto.Role);
            }


            if (user.Roles.FirstOrDefault(x => x.Id == role.Id) != null)
            {
               throw new RoleException();
            }

            if (role.Name == "Student")
            {
                if (user.Roles.Any(x => x.Name == "Admin") || user.Roles.Any(x => x.Name == "Manager") || user.Roles.Any(x => x.Name == "Teacher"))
                {
                    throw new RoleException(role.Name);
                }
            }
            else
            {
                if (user.Roles.Any(x => x.Name == "Student"))
                {
                    throw new RoleException();
                }
            }

            Console.WriteLine($"\n---> Add role: {role.Name} to: {user.Id}");

            user.Roles.Add(role);
            //_userRepository.Update(existsUser);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }


        public async Task RemoveRoleAsync(UserRoleDto userRoleDto, CancellationToken cancellationToken = default)
        {
            var user = await _repositoryManager.UserRepository.GetByIdAsync(userRoleDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(userRoleDto.UserId);
            }

            var role = await _repositoryManager.RoleRepository.GetByName(userRoleDto.Role);

            if (role is null)
            {
                throw new RoleNotFoundException(userRoleDto.Role);
            }


            if (user.Roles.FirstOrDefault(x => x.Id == role.Id) == null)
            {
                throw new RoleDoesNotBelongToUserException(user.Id, role.Id);
            }

            Console.WriteLine($"\n---> Delete role: {role.Name} from: {user.Id}");

            user.Roles.Remove(role);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserExamDto>> GetUserExamsAsync(string id, CancellationToken cancellationToken = default)
        {
            var userExams = await _repositoryManager.UserExamsRepository.GetAllAsync(id);
            var userExamsDto = _mapper.Map<IEnumerable<UserExamDto>>(userExams);

            return userExamsDto;

        }

        public async Task AddExamToUserAsync(UserExamDto userExamDto, CancellationToken cancellationToken = default)
        {

            var user = await _repositoryManager.UserRepository.GetByIdAsync(userExamDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(userExamDto.UserId);
            }

            if (user.Roles.FirstOrDefault(x => x.Name == "Student") == null)
            {
                throw new UserAddToExamException(user.Id);
            }

            var userExam = await _repositoryManager.UserExamsRepository.GetByUserIdAndExamId(userExamDto.UserId, userExamDto.ExamId);

            if (userExam != null)
            {
                throw new ExamIsAlreadyExistException(userExamDto.ExamId);
            }


            // gRPC service check exam data in the report service
            var reportResult = await _reportGrpcService.IsExistExamRequest(userExamDto.UserId, userExamDto.ExamId);

            if (!reportResult.Success)
            {
                var newUserExam = new UserExams() 
                {
                    ExamId = userExamDto.ExamId, 
                    UserId = userExamDto.UserId 
                };

            _repositoryManager.UserExamsRepository.Insert(newUserExam);
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveExamFromUser(UserExamDto userExamDto, CancellationToken cancellationToken = default)
        {
            var userExam = await _repositoryManager.UserExamsRepository.GetByUserIdAndExamId(userExamDto.UserId, userExamDto.ExamId);

            if (userExam == null)
            {
                throw new ExamNotFoundInUserException(userExamDto.ExamId);
            }
                _repositoryManager.UserExamsRepository.Remove(userExam);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
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

        private string RandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYabcdefghijklmnpqrstuvwxy#$^+=!*()@%&0123456789";

            return new string(Enumerable.Repeat(chars, length).Select(x => x[randomPassword.Next(x.Length)]).ToArray());
        }
    }
}
