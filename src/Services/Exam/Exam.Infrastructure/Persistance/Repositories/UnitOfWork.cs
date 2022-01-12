using System;
using System.Threading;
using System.Threading.Tasks;
using Exam.Domain.Repositories;

namespace Exam.Infrastructure.Persistance.Repositories
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ExamDbContext _dbContext;

        public UnitOfWork(ExamDbContext dbContext) => _dbContext = dbContext;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _dbContext.SaveChangesAsync(cancellationToken);
    }
}
