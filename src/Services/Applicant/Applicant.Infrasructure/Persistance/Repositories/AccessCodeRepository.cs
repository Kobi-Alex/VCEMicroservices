using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Applicant.Domain.Entities;
using Applicant.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Applicant.Infrasructure.Persistance.Repositories
{
    internal sealed class AccessCodeRepository : IAccessCodeRepository
    {
        private readonly AppDbContext _dbContext;
        public AccessCodeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext; 
        }


        public async Task<IEnumerable<AccessCode>> GetAllByEmail(string email)
        {
            return await _dbContext.AccessCodes
                   .Where(ac => ac.Email == email)
                   .OrderByDescending(ac => ac.ExpiryDate)
                   .ToListAsync();
        }

        public void Create(AccessCode item)
        {
            _dbContext.AccessCodes.Add(item);
        }

        public void Remove(AccessCode item)
        {
            _dbContext.AccessCodes.Remove(item);
        }

        public void RemoveRange(IEnumerable<AccessCode> accessCodes)
        {
            _dbContext.AccessCodes.RemoveRange(accessCodes);
        }
    }

}
