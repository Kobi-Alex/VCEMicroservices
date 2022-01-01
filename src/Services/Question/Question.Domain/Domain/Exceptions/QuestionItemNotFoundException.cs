
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Domain.Exceptions
{
    public sealed class QuestionItemNotFoundException: NotFoundException
    {
        public QuestionItemNotFoundException(int Id)
           : base($"The question with the identifier {Id} was not found")
        {
        }
    }
}
