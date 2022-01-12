using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.Domain.Repositories;

namespace Exam.Infrastructure.Persistance.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IExamItemRepository> _lazyExamItemRepository;      
        private readonly Lazy<IExamQuestionRepository> _lazyExamQuestionRepository;      
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

        public RepositoryManager(ExamDbContext dbContext)
        {
            _lazyExamItemRepository = new Lazy<IExamItemRepository>(() => new ExamItemRepository(dbContext));
            _lazyExamQuestionRepository = new Lazy<IExamQuestionRepository>(() => new ExamQuestionRepository(dbContext));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
        }

        public IExamItemRepository ExamItemRepository => _lazyExamItemRepository.Value;
        public IExamQuestionRepository ExamQuestionRepository => _lazyExamQuestionRepository.Value;
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
