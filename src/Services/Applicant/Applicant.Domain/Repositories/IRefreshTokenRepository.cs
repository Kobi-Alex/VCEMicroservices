using System;
using System.Threading;
using System.Threading.Tasks;
using Applicant.Domain.Entities;


namespace Applicant.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> Get(string token, CancellationToken cancellationToken = default);
        void Create(RefreshToken refreshToken);
        void Delete(RefreshToken refreshToken);
    }

}
