using System;
using AutoMapper;
using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;
using Question.Domain.Entities;


namespace Question.API.Application.Contracts.Profiles.QuestionCategoriesProfiles
{
    public class QuestionCategoryProfile : Profile
    {
        public QuestionCategoryProfile()
        {
            CreateMap<QuestionCategory, QuestionCategoryReadDto>();
            CreateMap<QuestionCategoryCreateDto, QuestionCategory>();
            CreateMap<QuestionCategoryUpdateDto, QuestionCategory>();
        }
    }
}
