using System;
using AutoMapper;
using Report.Domain.AggregatesModel.ReviewAggregate;
using Report.API.Application.Contracts.Dtos.ReviewDtos;

namespace Report.API.Application.Contracts.Profiles.ReviewProfiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            //CreateMap<QuestionItem, QuestionItemReadDto>();
            //CreateMap<QuestionItemCreateDto, QuestionItem>();
            //CreateMap<QuestionItemUpdateDto, QuestionItem>();
            //CreateMap<QuestionItem, QuestionItemDeleteEvent>();
        }
    }
}
