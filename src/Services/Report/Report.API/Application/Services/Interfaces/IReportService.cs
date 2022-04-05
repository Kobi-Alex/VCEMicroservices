using Report.API.Application.Features.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Report.API.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<Review>> GetAllReviewByExamId(int examId,CancellationToken cancellationToken = default);
    }
}
