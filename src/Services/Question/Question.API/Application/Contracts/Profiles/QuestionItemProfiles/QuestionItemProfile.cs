using System;
using AutoMapper;
using Question.Domain.Entities;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;

namespace Question.API.Application.Contracts.Profiles.QuestionItemProfiles
{
    public class QuestionItemProfile : Profile
    {
        public QuestionItemProfile()
        {
            CreateMap<QuestionItem, QuestionItemReadDto>();
            CreateMap<QuestionItemCreateDto, QuestionItem>();
            CreateMap<QuestionItemUpdateDto, QuestionItem>();
        }
    }
}
