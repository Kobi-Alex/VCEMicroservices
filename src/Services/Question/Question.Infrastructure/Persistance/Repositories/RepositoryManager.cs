using Question.Domain.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Infrastructure.Persistance.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IQuestionRepository> _lazyQuestionRepository;
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

        public RepositoryManager(QuestionDbContext dbContext)
        {
            _lazyQuestionRepository = new Lazy<IQuestionRepository>(() => new QuestionRepository(dbContext));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
        }

        public IQuestionRepository QuestionRepository => _lazyQuestionRepository.Value;
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
