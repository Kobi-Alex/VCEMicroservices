using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Domain.Domain.Exeptions
{
    public sealed class ExamNotFoundException : NotFoundException
    {
        public ExamNotFoundException(int Id)
            : base($"The exam with the identifier {Id} was not found")
        {
        }

        public ExamNotFoundException(string description) 
            : base($"The exam with the identifier {description} was not found")
        {
        }
    }
}