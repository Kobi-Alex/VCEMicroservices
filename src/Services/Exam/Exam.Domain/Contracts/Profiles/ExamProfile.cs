using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Exam.Domain.Contracts.Dtos;
using Exam.Domain.Domain.Entities;

namespace Exam.Domain.Contracts.Profiles
{
    public class ExamProfile : Profile
    {
        public ExamProfile()
        {
            //<PackageReference Include="Mapster" Version="7.2.0" />
            //Source -> Target
            CreateMap<ExamItem, ExamReadDto>();
            CreateMap<ExamCreateDto, ExamItem>();
            CreateMap<ExamUpdateDto, ExamItem>();
        }
    }
}
