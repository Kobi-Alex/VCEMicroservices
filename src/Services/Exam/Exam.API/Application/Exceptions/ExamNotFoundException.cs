using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.API.Application.Exceptions
{
    public sealed class ExamNotFoundException : NotFoundException
    {
        public ExamNotFoundException(int Id)
            : base($"The exam with the identifier {Id} was not found")
        {
        }

        public ExamNotFoundException(string name) 
            : base($"The exam with the name {name} was not found")
        {
        }
    }
}