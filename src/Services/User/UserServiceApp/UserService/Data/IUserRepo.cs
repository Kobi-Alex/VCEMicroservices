using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();

        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByEmailAsync(string email);

        Task<IEnumerable<User>> FindAllAsync(Expression<Func<User, bool>> predicate);
        Task<User> FindAsync(Expression<Func<User, bool>> predicate);

        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(string id, UserUpdateDto user);
        Task<bool> DeleteUserAsync(string id);

        Task<IdentityResult> AddRoleToUserAsync(UserRoleDto userRoleDto);
        Task<IdentityResult> DeleteRoleFromUserAsync(UserRoleDto userRoleDto);

        Task<IdentityResult> ChangePasswordAsync(UserChangePassword userChangePassword);

        Task<string> GetRolesUserAsync(User user);
    }
}
