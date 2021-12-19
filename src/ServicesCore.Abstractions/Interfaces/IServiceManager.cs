using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesCore.Abstractions.Interfaces
{
    public interface IServiceManager
    {
        IExamService ExamService { get; }
    }
}
