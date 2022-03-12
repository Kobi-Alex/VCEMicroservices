using System;

namespace Exam.API.Application.Services.Interfaces
{
    public interface IServiceManager
    {
        IExamItemService ExamItemService { get; }
        IExamQuestionService ExamQuestionService { get; }
    }
}
