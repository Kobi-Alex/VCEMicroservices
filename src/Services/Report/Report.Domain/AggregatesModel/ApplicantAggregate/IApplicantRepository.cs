using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Report.Domain.SeedWork;
using System.Collections.Generic;

namespace Report.Domain.AggregatesModel.ApplicantAggregate
{
    public interface IApplicantRepository : IRepository<Applicant>
    {
        Task<Applicant> GetByIdAsync(string id);
        Task<Applicant> GetAsync(string applicantIdentityGuid);

        Applicant Insert(Applicant applicant);
        void Update(Applicant applicant);
    }
}
