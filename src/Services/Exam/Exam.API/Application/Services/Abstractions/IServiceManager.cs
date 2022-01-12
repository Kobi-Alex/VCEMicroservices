using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.API.Application.Services.Abstractions
{
    public interface IServiceManager
    {
        IExamItemService ExamItemService { get; }
        IExamQuestionService ExamQuestionService { get; }
    }
}
