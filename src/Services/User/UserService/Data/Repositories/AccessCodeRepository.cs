using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserService.Data.Interfaces;
using UserService.Dto.AccessCode;
using UserService.Models;

namespace UserService.Data.Repositories
{
    public class AccessCodeRepository : IAccessCodeRepository
    {
        private readonly AppDbContext _context;

        public AccessCodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(AccessCode accessCode)
        {
            _context.AccessCodes.Add(accessCode);
        }

        public async Task<IEnumerable<AccessCode>> GetByEmail(string email)
        {
            return await _context.AccessCodes.Where(x => x.Email == email).OrderByDescending(x => x.ExpiryDate).ToListAsync();
        }

        public void RemoveByEmail(string email)
        {
            var userAccessCodes = _context.AccessCodes.Where(x => x.Email == email);

            _context.RemoveRange(userAccessCodes);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return (await _context.SaveChangesAsync(cancellationToken) >= 0);
        }
    }
}
