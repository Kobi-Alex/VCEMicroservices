using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Report.Domain.SeedWork;
using Report.Domain.AggregatesModel.ReviewAggregate;

namespace Report.Infrastructure.Persistance.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReportDbContext _dbContext;

        public IUnitOfWork UnitOfWork
        {
            get { return _dbContext;}
        }

        public ReviewRepository(ReportDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<Review> GetReportByReviewIdAsync(int reviewId)
        {

            var review = await _dbContext.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewId);


            if (review == null)
            {
                review = _dbContext.Reviews
                    .Local.FirstOrDefault(r => r.Id == reviewId);
            }

            if(review != null)
            {
                await _dbContext.Entry(review)
                    .Collection(u => u.QuestionUnits).LoadAsync();
            }


            return review;
        }

        public async Task<Review> GetReportByApplicantIdAsync(int examId, string userId)
        {
            
            var review = await _dbContext.Reviews
               .Include(x=>x.QuestionUnits)
               .FirstOrDefaultAsync(r=>r._examId == examId && r._applicantId == userId);


            if (review == null)
            {
                review = _dbContext.Reviews
                    .Local.FirstOrDefault(r => r._examId == examId && r._applicantId == userId);
            }

            if (review != null)
            {
                await _dbContext.Entry(review)
                    .Collection(u => u.QuestionUnits).LoadAsync();
            }


            return review;
        }

        public Review Add (Review review)
        {
            return _dbContext.Reviews.Add(review).Entity;
        }

        public void Update(Review review)
        {
            _dbContext.Entry(review).State = EntityState.Modified;
        }

        public void Remove(Review review)
        {
            _dbContext.Reviews.Remove(review);
        }

    }
}