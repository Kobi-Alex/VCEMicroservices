using System;


namespace Question.API.Application.Services.Interfaces
{
    // Service manager interface
    public interface IServiceManager
    {
        IQuestionCategoryService QuestionCategoryService { get; }
        IQuestionItemService QuestionItemService { get; }
        IQuestionAnswerService QuestionAnswerService { get; }
    }
}
