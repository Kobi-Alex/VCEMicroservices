using System;

namespace Question.Domain.Repositories
{
    public interface IRepositoryManager
    {
        IQuestionItemRepository QuestionItemRepository { get; }
        IQuestionCategoryRepository QuestionCategoryRepository { get; }
        IQuestionAnswerRepository QuestionAnswerRepository { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}
