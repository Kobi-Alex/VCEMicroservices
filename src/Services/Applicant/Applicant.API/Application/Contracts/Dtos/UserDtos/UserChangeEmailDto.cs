using System;
using System.ComponentModel.DataAnnotations;


namespace Applicant.API.Application.Contracts.Dtos.UserDtos
{
    public class UserChangeEmailDto
    {
        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "AccessCode is required")]
        public int AccessCode { get; set; }
    }
}