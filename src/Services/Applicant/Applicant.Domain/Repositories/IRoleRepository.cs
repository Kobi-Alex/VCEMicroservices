using System;
using System.Threading.Tasks;
using Applicant.Domain.Entities;
using System.Collections.Generic;


namespace Applicant.Domain.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetById(int id);
        Task<Role> GetByName(string name);
        void Create(Role role);
        bool Exists(string name);
    }

}
