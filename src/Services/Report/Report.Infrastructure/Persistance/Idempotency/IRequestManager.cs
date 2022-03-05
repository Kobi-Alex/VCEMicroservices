using System;
using System.Threading.Tasks;

namespace Report.Infrastructure.Persistance.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(string id);
        Task CreateRequestForCommandAsync<T>(string id);
    }
}
