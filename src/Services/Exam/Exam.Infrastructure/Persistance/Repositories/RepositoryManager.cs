using Exam.Domain.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Infrastructure.Persistance.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IExamRepository> _lazyExamRepository;      
        private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

        public RepositoryManager(ExamDbContext dbContext)
        {
            _lazyExamRepository = new Lazy<IExamRepository>(() => new ExamRepository(dbContext));
            _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
        }

        public IExamRepository ExamRepository => _lazyExamRepository.Value;
        public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
    }
}
