using System;
using MediatR;
using System.Collections.Generic;
using Question.API.Application.Queries;
using System.Threading.Tasks;
using System.Threading;
using Question.Domain.Services.Abstractions;
using Question.Domain.Contracts.Dtos;
using AutoMapper;
using Question.Domain.Domain.Repositories;
using Question.Domain.Domain.Entities;
using Question.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Question.API.Application.Handlers
{
    public class GetQuestionListHandler : IRequestHandler<GetQuestionListQuery, IEnumerable<QuestionReadDto>>
    {
        private readonly IServiceManager serviceManager;

        public GetQuestionListHandler(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        public async Task<IEnumerable<QuestionReadDto>> Handle(GetQuestionListQuery request, CancellationToken cancellationToken)
        {
            var questions = await serviceManager.QuestionService.GetAllAsync(cancellationToken);

            return await Task.FromResult(questions);
        }
    }


}
