using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.API.Application.Exceptions
{
    public sealed class QuestionAnswerNotFoundException : NotFoundException
    {
        public QuestionAnswerNotFoundException(int questionId)
            : base($"The question answer with the identifier {questionId} was not found")
        {
        }
    }
}