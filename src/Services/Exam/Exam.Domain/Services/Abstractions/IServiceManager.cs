using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Domain.Services.Abstractions
{
    public interface IServiceManager
    {
        IExamService ExamService { get; }
    }
}
