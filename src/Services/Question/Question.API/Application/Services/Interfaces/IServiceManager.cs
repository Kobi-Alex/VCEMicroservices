using System;

namespace Question.API.Application.Services.Interfaces
{
    public interface IServiceManager
    {
        IQuestionCategoryService QuestionCategoryService { get; }
        IQuestionItemService QuestionItemService { get; }
        IQuestionAnswerService QuestionAnswerService { get; }
    }
}
