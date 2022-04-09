using System;
using AutoMapper;
using GrpcApplicant;
using Applicant.Domain.Entities;
using Applicant.API.Application.Contracts.Dtos.UserDtos;

namespace Applicant.API.Application.Contracts.Profiles
{

    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            // Source -> Targer
            CreateMap<User, UserReadDto>();
            CreateMap<UserReadDto, User>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserExamDto, User>();
            CreateMap<User, UserExamDto>();
            CreateMap<RemoveExamRequest, UserExamDto>();
            CreateMap<UserExamDto, RemoveExamRequest>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<UserExams, UserReadDto>();
            CreateMap<UserExams, UserExamDto>();
            CreateMap<UserReadDto, GetUserDataResponse>();
        }

    }
}
