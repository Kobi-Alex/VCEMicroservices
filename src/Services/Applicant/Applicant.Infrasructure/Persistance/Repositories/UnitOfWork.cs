using System;
using System.Threading;
using System.Threading.Tasks;
using Applicant.Domain.Repositories;


namespace Applicant.Infrasructure.Persistance.Repositories
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext) => _dbContext = dbContext;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _dbContext.SaveChangesAsync(cancellationToken);
    }

}
