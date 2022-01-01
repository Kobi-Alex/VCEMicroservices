using AutoMapper;
using Question.Domain.Contracts.Dtos;
using Question.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Contracts.Profiles
{
    public class QuestionProfile: Profile
    {
        public QuestionProfile()
        {
            CreateMap<QuestionItem, QuestionReadDto>();
            CreateMap<QuestionCreateDto, QuestionItem>();
            CreateMap<QuestionUpdateDto, QuestionItem>();
        }
    }
}
