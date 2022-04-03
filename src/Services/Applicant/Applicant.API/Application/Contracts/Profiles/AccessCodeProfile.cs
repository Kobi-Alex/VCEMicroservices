using System;
using AutoMapper;
using Applicant.Domain.Entities;
using Applicant.API.Application.Contracts.Dtos.AccesCoreDtos;


namespace Applicant.API.Application.Contracts.Profiles
{
    public class AccessCodeProfile : Profile
    {
        public AccessCodeProfile()
        {
            CreateMap<AccessCode, AccessCodeReadDto>();
            CreateMap<AccessCodeReadDto, AccessCode>();
            CreateMap<AccessCodeCreateDto, AccessCode>();
        }
    }
}
