using System;
using System.Threading;
using System.Threading.Tasks;

using Question.Domain.Repositories;


namespace Question.Infrastructure.Persistance.Repositories
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly QuestionDbContext _dbContext;

        public UnitOfWork(QuestionDbContext dbContext) => _dbContext = dbContext;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _dbContext.SaveChangesAsync(cancellationToken);

    }
}
