using System;
using System.Threading;
using System.Threading.Tasks;


namespace Applicant.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

}
