using System;


namespace Exam.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IExamItemRepository ExamItemRepository { get; }
        IExamQuestionRepository ExamQuestionRepository { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}