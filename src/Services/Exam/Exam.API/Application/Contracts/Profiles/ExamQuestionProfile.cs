using System;
using AutoMapper;
using Exam.Domain.Entities;
using Exam.API.Application.Contracts.ExamQuestionDtos;

namespace Exam.API.Application.Contracts.Profiles
{
    public class ExamQuestionProfile : Profile
    {
        public ExamQuestionProfile()
        {
            //Source -> Target
            CreateMap<ExamQuestion, ExamQuestionReadDto>();
            CreateMap<ExamQuestionCreateDto, ExamQuestion>();
        }
    }
}
