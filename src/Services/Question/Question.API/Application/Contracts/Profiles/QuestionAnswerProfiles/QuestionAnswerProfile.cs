using AutoMapper;
using Question.API.Application.Contracts.Dtos.QuestionAnswerDtos;
using Question.Domain.Entities;
using System;


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