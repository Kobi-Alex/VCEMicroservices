using System;
using System.Linq;
using System.Threading.Tasks;
using Applicant.Domain.Entities;
using Applicant.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Applicant.Infrasructure.Persistance.Repositories
{
    internal sealed class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _dbContext;
        public RoleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Role> GetById(int id)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Role> GetByName(string name)
        {
            return await _dbContext.Roles
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public void Create(Role role)
        {
            _dbContext.Roles.Add(role);
        }

        public bool Exists(string name)
        {
            return _dbContext.Roles
                .Any(x => x.Name == name);
        }

    }

}
