using System;
using System.Collections.Generic;
using MediatR;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;

namespace Question.API.Application.Queries
{
    public record GetQuestionListQuery : IRequest<IEnumerable<QuestionItemReadDto>>;

}