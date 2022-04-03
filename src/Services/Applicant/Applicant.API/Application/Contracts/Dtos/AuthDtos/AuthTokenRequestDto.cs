using System;
using System.ComponentModel.DataAnnotations;


namespace Applicant.API.Application.Contracts.Dtos.AuthDtos
{
    public class AuthTokenRequestDto
    {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }

        [Required(ErrorMessage = "RefreshToken is required")]
        public string RefreshToken { get; set; }
    }
}
