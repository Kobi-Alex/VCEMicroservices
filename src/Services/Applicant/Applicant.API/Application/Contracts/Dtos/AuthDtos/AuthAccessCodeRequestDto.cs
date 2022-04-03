using System;

namespace Applicant.API.Application.Contracts.Dtos.AuthDtos
{
    public class AuthAccessCodeRequestDto
    {
        public string Email { get; set; }
        public int Code { get; set; }
    }
}
