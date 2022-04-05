using Report.API.Application.Features.Queries;
using Report.API.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Report.API.Application.Services
{
    internal sealed class ReportService : IReportService
    {
        private readonly IReviewQueries _reviewQueries;

        public ReportService(IReviewQueries reviewQueries)
        {
            _reviewQueries = reviewQueries;
        }
        public async Task<IEnumerable<Review>> GetAllReviewByExamId(int examId, CancellationToken cancellationToken = default)
        {

            var list = await _reviewQueries.GetReportsByExamIdAsync(examId);

            return list;
        }
    }
}
