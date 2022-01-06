using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.API.Application.Exceptions
{
    public sealed class QuestionCategoryDoesNotBelongToQuestionItemException : BadRequestException
    {
        public QuestionCategoryDoesNotBelongToQuestionItemException(int questionItemId, int questionCategoryId)
            :base($"The question category with the identifier {questionCategoryId} does not belong to the question item" +
                 $"with the identifier {questionItemId}")
        {

        }
    }
}