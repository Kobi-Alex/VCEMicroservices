using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Data.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<RefreshToken> Get(string token, CancellationToken cancellationToken = default);

        void Create(RefreshToken refreshToken);
        void Delete(RefreshToken refreshToken);

    }
}
