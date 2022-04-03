using System;
using System.ComponentModel.DataAnnotations;


namespace Applicant.API.Application.Contracts.Dtos.AuthDtos
{
    public class AuthLoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
