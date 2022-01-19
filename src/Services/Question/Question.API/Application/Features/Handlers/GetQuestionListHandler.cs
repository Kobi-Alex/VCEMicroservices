using System;
using System.Collections.Generic;
using Question.API.Application.Queries;
using System.Threading.Tasks;
using System.Threading;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;
using Question.API.Application.Services.Interfaces;

namespace Question.API.Application.Handlers
{
    //public class GetQuestionListHandler : IRequestHandler<GetQuestionListQuery, IEnumerable<QuestionItemReadDto>>
    //{
    //    private readonly IServiceManager serviceManager;

    //    public GetQuestionListHandler(IServiceManager serviceManager)
    //    {
    //        this.serviceManager = serviceManager;
    //    }

    //    public async Task<IEnumerable<QuestionItemReadDto>> Handle(GetQuestionListQuery request, CancellationToken cancellationToken)
    //    {
    //        var questions = await serviceManager.QuestionItemService.GetAllByQuestionCategoryIdAsync(2, cancellationToken);

    //        return await Task.FromResult(questions);
    //    }
    //}


}
