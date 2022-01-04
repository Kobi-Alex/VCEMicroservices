using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Exceptions
{
    public sealed class QuestionAnswerNotFoundException : NotFoundException
    {
        public QuestionAnswerNotFoundException(int questionItemId)
            : base($"The question answer with the identifier {questionItemId} was not found")
        {
        }
    }
}