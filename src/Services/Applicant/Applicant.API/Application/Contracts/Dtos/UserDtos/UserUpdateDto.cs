using System;
using System.ComponentModel.DataAnnotations;


namespace Applicant.API.Application.Contracts.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
