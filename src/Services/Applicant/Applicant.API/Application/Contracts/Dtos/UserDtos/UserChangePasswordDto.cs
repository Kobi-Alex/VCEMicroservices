using System;
using System.ComponentModel.DataAnnotations;


namespace Applicant.API.Application.Contracts.Dtos.UserDtos
{
    public class UserChangePasswordDto
    {
        [Required(ErrorMessage = "IdUser is required")]
        public string IdUser { get; set; }

        [Required(ErrorMessage = "CurrentPassword is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Passwords must be at least 6 characters. Passwords must have at least one digit ('0'-'9'). Passwords must have at least one lowercase ('a'-'z').")]

        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "NewPassword is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Passwords must be at least 6 characters. Passwords must have at least one digit ('0'-'9'). Passwords must have at least one lowercase ('a'-'z').")]

        public string NewPassword { get; set; }
    }

}