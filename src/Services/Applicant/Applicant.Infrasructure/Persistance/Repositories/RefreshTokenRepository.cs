using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Applicant.Domain.Entities;
using Applicant.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Applicant.Infrasructure.Persistance.Repositories
{
    internal sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _dbContext;
        public RefreshTokenRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<RefreshToken> Get(string token, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);
        }

        public void Create(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Add(refreshToken);
        }

        public void Delete(RefreshToken refreshToken)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
        }

    }

}
