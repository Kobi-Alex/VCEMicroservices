using System;
using AutoMapper;
using Exam.Domain.Entities;
using Exam.API.Application.Contracts.ExamItemDtos;

namespace Exam.API.Application.Contracts.Profiles
{
    public class ExamItemProfile : Profile
    {
        public ExamItemProfile()
        {
            //<PackageReference Include="Mapster" Version="7.2.0" />
            //Source -> Target
            CreateMap<ExamItem, ExamItemReadDto>();
            CreateMap<ExamItemCreateDto, ExamItem>();
            CreateMap<ExamItemUpdateDto, ExamItem>();
        }
    }
}
