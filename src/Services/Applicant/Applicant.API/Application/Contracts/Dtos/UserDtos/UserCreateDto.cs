using System;
using System.ComponentModel.DataAnnotations;


namespace Applicant.API.Application.Contracts.Dtos.UserDtos
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Passwords must be at least 6 characters. Passwords must have at least one digit ('0'-'9'). Passwords must have at least one lowercase ('a'-'z').")]

        //public string Password { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
        public string AdditionaInfo { get; set; }
    }

}
