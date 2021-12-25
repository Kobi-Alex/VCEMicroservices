using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Domain.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IExamRepository ExamRepository { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}