using System;


namespace Question.Domain.Repositories
{
    // Repository manager interface
    public interface IRepositoryManager
    {
        IQuestionItemRepository QuestionItemRepository { get; }
        IQuestionCategoryRepository QuestionCategoryRepository { get; }
        IQuestionAnswerRepository QuestionAnswerRepository { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}
