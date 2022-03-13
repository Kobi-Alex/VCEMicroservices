using System;
using System.Threading;
using System.Threading.Tasks;


namespace Question.Domain.Repositories
{
    // Unit of work interface
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
