using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> FindAllAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);
        Task<User> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default);

        void Create(User user, string password);
        void Update(User user);
        void Delete(User user);

        void AddRole(User user,Role role);
        void DeleteRole(User user, Role role);

        void ChangePassword(User user, string newPassword);

        bool CheckPassword(User user, string password);

        Task<IEnumerable<User>> GetAdmins();
    }
}
