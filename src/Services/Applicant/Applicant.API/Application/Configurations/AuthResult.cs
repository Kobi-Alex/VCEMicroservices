using System;
using Applicant.API.Application.Contracts.Dtos.UserDtos;


namespace Applicant.API.Application.Configurations
{
    public class AuthResult
    {
        public UserReadDto User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
