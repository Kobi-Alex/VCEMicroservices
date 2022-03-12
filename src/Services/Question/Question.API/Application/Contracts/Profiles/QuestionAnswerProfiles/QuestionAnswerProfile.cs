using System;
using AutoMapper;
using Question.Domain.Entities;
using Question.API.Application.Contracts.Dtos.QuestionAnswerDtos;


namespace Question.API.Application.Contracts.Profiles.QuestionAnswerProfiles
{
    public class QuestionAnswerProfile : Profile
    {
        public QuestionAnswerProfile()
        {
            CreateMap<QuestionAnswer, QuestionAnswerReadDto>();
            CreateMap<QuestionAnswerCreateDto, QuestionAnswer>();
            CreateMap<QuestionAnswerUpdateDto, QuestionAnswer>();
        }
    }
}