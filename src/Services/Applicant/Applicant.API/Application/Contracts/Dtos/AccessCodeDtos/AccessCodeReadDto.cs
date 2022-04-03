using System;

namespace Applicant.API.Application.Contracts.Dtos.AccesCoreDtos
{
    public class AccessCodeReadDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int Code { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
    }

}
