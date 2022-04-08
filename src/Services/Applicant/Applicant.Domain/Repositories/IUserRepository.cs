using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Applicant.Domain.Entities;
using System.Collections.Generic;


namespace Applicant.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> FindAllAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);

        Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);

        void Insert(User user);
        //void Update(User user);
        void Delete(User user);
    }
}
