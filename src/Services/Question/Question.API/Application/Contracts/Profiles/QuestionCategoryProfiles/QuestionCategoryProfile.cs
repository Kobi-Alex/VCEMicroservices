using System;
using AutoMapper;
using Question.Domain.Entities;
using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;


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
