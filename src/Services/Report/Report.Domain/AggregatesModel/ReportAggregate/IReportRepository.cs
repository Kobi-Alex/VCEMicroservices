using Report.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Domain.AggregatesModel.ReportAggregate
{
    public interface IReportRepository : IRepository<Report>
    {
        Task<Report> GetByIdAsync(int reportId);
        Report Add(Report report);
        void Update(Report report);
    }

}
