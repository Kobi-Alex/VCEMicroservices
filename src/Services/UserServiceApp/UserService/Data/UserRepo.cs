using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Data
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserRepo(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._mapper = mapper;
        }


        public async Task<IdentityResult> AddRoleToUserAsync(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByIdAsync(userRoleDto.UserId);

            var roleFind = await _roleManager.FindByNameAsync(userRoleDto.Role);

            
            var result = await _userManager.AddToRoleAsync(user, userRoleDto.Role);

            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            //_context.Users.Add(user);

            var result = await _userManager.CreateAsync(user,password);

            return result;


            //var userRole = _context.Roles.FirstOrDefault(r => r.Name == "Student");

            //_context.UserRoles.Add(new IdentityUserRole<string>() { RoleId = userRole.Id, UserId = user.Id });


            //var result = await _userManager.CreateAsync(user, password);

            //var userRole = await _roleManager.FindByNameAsync("Student");

            //await _userManager.AddToRoleAsync(user, userRole.Name);

            //return result;
        }

        public async Task<IdentityResult> DeleteRoleFromUserAsync(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByIdAsync(userRoleDto.UserId);
            var result = await _userManager.RemoveFromRoleAsync(user, userRoleDto.Role);

            return result;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var deleteUser = await _userManager.FindByIdAsync(id);

            var userRoles = await _userManager.GetRolesAsync(deleteUser);

            var adminRole = userRoles.FirstOrDefault(x => x == "Admin");
            if (adminRole != null)
            {
                var adminRoleId = _context.Roles.Where(x => x.Name == "Admin").FirstOrDefault();

                if (_context.UserRoles.Where(x => x.RoleId == adminRoleId.Id).ToList().Count() <= 1)
                {
                    return false;
                }
            }

            var result = await _userManager.DeleteAsync(deleteUser);

            return result.Succeeded;
        }

        public async Task<User> FindAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<User>> FindAllAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.Where(predicate).ToListAsync();
        }

        public  async Task<User> GetUserByEmailAsync(string email)
        {
            //return _context.Users.FirstOrDefault(x => x.Email == email);
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public async Task<IdentityResult> UpdateUserAsync(string id, UserUpdateDto user)
        {
            var existUser = await _userManager.FindByIdAsync(id);

            _mapper.Map(user,existUser);
            existUser.NormalizedEmail = existUser.Email.ToUpper();
            existUser.NormalizedUserName = existUser.UserName.ToUpper();
            existUser.UpdatedAt = new DateTimeOffset(DateTime.Now);

            Console.WriteLine(existUser.CreatedAt);
            var result = await _userManager.UpdateAsync(existUser);

            return result;
        }

        public async Task<IdentityResult> ChangePasswordAsync(UserChangePassword userChangePassword)
        {
            var existUser =  _context.Users.FirstOrDefault(x => x.Id == userChangePassword.IdUser);

            var result = await _userManager.ChangePasswordAsync(existUser, userChangePassword.CurrentPassword, userChangePassword.NewPassword);

            return result;
        }

        public async Task<string> GetRolesUserAsync(User user)
        {
            var result = await _userManager.GetRolesAsync(user);
            var roles = String.Join(", ", result.ToArray());

            return roles;
        }
    }
}
