using System;
using Question.Domain.Repositories;


namespace Question.Infrastructure.Persistance.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IQuestionItemRepository> _lazyQuestionItemRepository;
        private readonly Lazy<IQuestionCategoryRepository> _lazyQuestionCategoryRepository;
        private readonly Lazy<IQuestionAnswerRepository> _lazyQuestionAnswerRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

        public RepositoryManager(QuestionDbContext dbContext)
        {
            _lazyQuestionItemRepository = new Lazy<IQuestionItemRepository>(() => new QuestionItemRepository(dbContext));
            _lazyQuestionCategoryRepository = new Lazy<IQuestionCategoryRepository>(() => new QuestionCategoryRepository(dbContext));
            _lazyQuestionAnswerRepository = new Lazy<IQuestionAnswerRepository>(() => new QuestionAnswerRepository(dbContext));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
        }

        public IQuestionItemRepository QuestionItemRepository => _lazyQuestionItemRepository.Value;
        public IQuestionCategoryRepository QuestionCategoryRepository => _lazyQuestionCategoryRepository.Value;
        public IQuestionAnswerRepository QuestionAnswerRepository => _lazyQuestionAnswerRepository.Value;
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
