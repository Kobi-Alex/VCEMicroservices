using System;
using System.Collections.Generic;
using MediatR;
using Question.Domain.Contracts.Dtos;
using Question.Domain.Domain.Entities;

namespace Question.API.Application.Queries
{
    public record GetQuestionListQuery : IRequest<IEnumerable<QuestionReadDto>>;

}