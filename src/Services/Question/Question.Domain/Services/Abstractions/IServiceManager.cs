using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Services.Abstractions
{
    public interface IServiceManager
    {
        IQuestionService QuestionService { get; }
    }
}
