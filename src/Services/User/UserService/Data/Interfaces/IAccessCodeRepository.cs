using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserService.Dto.AccessCode;
using UserService.Models;

namespace UserService.Data.Interfaces
{
    public interface IAccessCodeRepository
    {
        public Task<IEnumerable<AccessCode>> GetByEmail(string email);

        public void Create(AccessCode codeCreateDto);

        public void RemoveByEmail(string email);

        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
