using System;

namespace Applicant.API.Application.Contracts.Dtos.AccesCoreDtos
{
    public class AccessCodeCreateDto
    {
        public string Email { get; set; }
        public int Code { get; set; }
    }
}