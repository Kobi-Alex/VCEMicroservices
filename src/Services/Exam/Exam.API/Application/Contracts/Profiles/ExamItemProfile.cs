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
            //Source -> Target
            CreateMap<ExamItem, ExamItemReadDto>();
            CreateMap<ExamItemCreateDto, ExamItem>();
            CreateMap<ExamItemUpdateDto, ExamItem>();
        }
    }
}
