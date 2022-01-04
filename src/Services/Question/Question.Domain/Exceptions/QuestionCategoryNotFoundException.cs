using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Exceptions
{
    public sealed class QuestionCategoryNotFoundException : NotFoundException
    {
        public QuestionCategoryNotFoundException(int questionItemId)
            : base($"The question category with the identifier {questionItemId} was not found")
        {
        }
    }
}
