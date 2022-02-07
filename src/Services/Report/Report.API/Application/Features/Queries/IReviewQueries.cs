using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.API.Application.Features.Queries
{
    public interface IReviewQueries
    {
        Task<IEnumerable<Review>> GetReportsByExamIdAsync(int examId);
        Task<IEnumerable<Review>> GetReportsByExamIdAndUserIdAsync(int examId, string userId);
        //Task<Review> GetReportByUserIdAsync(string userId);
    }
}