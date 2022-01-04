using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Exceptions
{
    public sealed class QuestionAnswerDoesNotBelongToQuestionItemException : BadRequestException
    {
        public QuestionAnswerDoesNotBelongToQuestionItemException(int questionItemId, int questionAnswerId)
            : base($"The question answer with the identifier {questionAnswerId} does not belong to the question item" +
                 $"with the identifier {questionItemId}")
        {

        }
    }
}
