using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IExamItemRepository ExamItemRepository { get; }
        IExamQuestionRepository ExamQuestionRepository { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}