using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UserService.Data.Interfaces;
using UserService.Models;

namespace UserService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private PasswordHasher<User> _hasher;

        public UserRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _hasher = new PasswordHasher<User>();
        }

        public void Create(User user, string password)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }


            user.Password = _hasher.HashPassword(null, password);

            _context.Users.Add(user);
        }

        public void Update(User user)
        {
            _context.Update(user);
        }

        public void Delete(User user)
        {
            _context.Remove(user);
        }

        public async Task<IEnumerable<User>> FindAllAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Roles).Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<User> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Roles).FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(x => x.Roles).ToListAsync(cancellationToken);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return (await _context.SaveChangesAsync(cancellationToken) >= 0);
        }

        public void AddRole(User user, Role role)
        {
            if (role != null)
            {
                user.Roles.Add(role);
            }

            user.Roles.Add(role);
        }

        public void DeleteRole(User user, Role role)
        {
            if (role != null)
            {
                user.Roles.Remove(role);
            }
        }

        public void ChangePassword(User user, string newPassword)
        {
            user.Password = _hasher.HashPassword(null, newPassword);

            _context.Update(user);
        }

        public bool CheckPassword(User user, string password)
        {
            var res = _hasher.VerifyHashedPassword(null, user.Password, password);

            switch (res)
            {
                case PasswordVerificationResult.Failed:
                    return false;
                case PasswordVerificationResult.Success:
                    return true;
                case PasswordVerificationResult.SuccessRehashNeeded:
                    return true;
                default:
                    return false;
            }
        }

        public async Task<IEnumerable<User>> GetAdmins()
        {
            var listAdmins = new List<User>();

            var adminRole = await _context.Roles.FirstOrDefaultAsync(x => x.Name == "Admin");

            foreach (var item in await _context.Users.Include(r => r.Roles).ToListAsync())
            {
                if (item.Roles.FirstOrDefault(x => x.Id == adminRole.Id) != null)
                {
                    listAdmins.Add(item);
                }
            }

            return listAdmins;
        }
    }
}
