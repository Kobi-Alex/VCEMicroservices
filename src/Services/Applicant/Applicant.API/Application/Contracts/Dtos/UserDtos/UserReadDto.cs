using System;

namespace Applicant.API.Application.Contracts.Dtos.UserDtos
{
    public class UserReadDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AdditionalInfo { get; set; }
        public string Roles { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

    }

}