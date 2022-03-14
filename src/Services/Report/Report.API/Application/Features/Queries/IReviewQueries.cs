using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Report.API.Application.Features.Queries
{
    //Interface for Dapper
    public interface IReviewQueries
    {
        Task<IEnumerable<Review>> GetAll();
        Task<IEnumerable<Review>> GetReportsById(int Id);
        Task<IEnumerable<Review>> GetReportsByExamIdAsync(int examId);
        Task<IEnumerable<Review>> GetReportByUserIdAsync(string userId);
        Task<IEnumerable<Review>> GetReportsByExamIdAndUserIdAsync(int examId, string userId);
    }
}