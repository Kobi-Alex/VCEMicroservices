using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dto.User
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
