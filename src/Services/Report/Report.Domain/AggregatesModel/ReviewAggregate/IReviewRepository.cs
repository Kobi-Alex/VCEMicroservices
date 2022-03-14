using System;
using Report.Domain.SeedWork;
using System.Threading.Tasks;


namespace Report.Domain.AggregatesModel.ReviewAggregate
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<Review> GetReportByReviewIdAsync(int reviewId);
        Task<Review> GetReportByApplicantIdAsync(int examId, string userId);

        Review Add(Review review);
        void Update(Review review);
        void Remove(Review review);

    }
}
    