using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Applicant.Domain.Entities;
using Applicant.Domain.Repositories;

using Microsoft.EntityFrameworkCore;


namespace Applicant.Infrasructure.Persistance.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {

        private readonly AppDbContext _dbContext;
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<User> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<IEnumerable<User>> FindAllAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(x => x.Roles)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(x => x.Roles)
                .ToListAsync(cancellationToken);
        }

        public async Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public void Insert(User user)
        {
            _dbContext.Users.Add(user);
        }

        public void Delete(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public void SetNewPassword(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            user.Password = password;

            _dbContext.Update(user);
        }
    }
}
