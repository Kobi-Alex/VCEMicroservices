using System;
using System.Threading.Tasks;
using Applicant.Domain.Entities;
using System.Collections.Generic;


namespace Applicant.Domain.Repositories
{
    public interface IAccessCodeRepository
    {
        Task<IEnumerable<AccessCode>> GetAllByEmail(string email);
        void Create(AccessCode item);
        void Remove(AccessCode item);
        void RemoveRange(IEnumerable<AccessCode> accessCodes);
    }
}
