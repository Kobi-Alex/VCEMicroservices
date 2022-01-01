
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Domain.Exceptions
{
    public sealed class QuestionNotFoundException: NotFoundException
    {
        public QuestionNotFoundException(int Id)
           : base($"The question with the identifier {Id} was not found")
        {
        }

        public QuestionNotFoundException(string category)
            : base($"The question with the {category} was not found")
        {
        }
    }
}
