using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserService.Data.Interfaces;
using UserService.Models;

namespace UserService.Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(RefreshToken refreshToken)
        {
            _context.Add(refreshToken);
        }

        public void Delete(RefreshToken refreshToken)
        {
            _context.Remove(refreshToken);
        }

        public async Task<RefreshToken> Get(string token, CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return (await _context.SaveChangesAsync(cancellationToken) >= 0);
        }
    }
}
