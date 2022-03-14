using System;
using System.Threading.Tasks;


namespace Report.Infrastructure.Persistance.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(int id);
        Task CreateRequestForCommandAsync<T>(int id);
    }
}
