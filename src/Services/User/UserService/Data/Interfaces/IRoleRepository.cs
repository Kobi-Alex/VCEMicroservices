using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Data.Interfaces
{
    public interface IRoleRepository
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

        bool Exists(string name);

        void Create(Role role);

        Task<Role> GetByName(string name);

        Task<Role> GetById(int id);
    }
}
