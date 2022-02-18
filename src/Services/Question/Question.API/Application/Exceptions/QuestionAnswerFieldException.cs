
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.API.Application.Exceptions
{
    public sealed class QuestionAnswerFieldException: FieldExistException
    {
        public QuestionAnswerFieldException(string key)
           : base($"Attention!! The answer with the same char key --> {key} is exist")
        {
        }
    }
}
