using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Report.Domain.SeedWork;
using Report.Domain.AggregatesModel.ReviewAggregate;

namespace Report.Infrastructure.Persistance.Repositories
{
    internal sealed class ReviewRepository : IReviewRepository
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


        public async Task<Review> GetReportByIdAsync(int reviewId)
        {

            var review = await _dbContext.Reviews
                .Include(x => x.QuestionUnits)
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

        public async Task<Review> GetReportByExamIdAsync(int examId)
        {

            var review = await _dbContext.Reviews
                .Include(x => x.QuestionUnits)
                .FirstOrDefaultAsync(r => r.GetExamId == examId);


            if (review == null)
            {
                review = _dbContext.Reviews
                    .Local.FirstOrDefault(r => r.GetExamId == examId);
            }

            if (review != null)
            {
                await _dbContext.Entry(review)
                    .Collection(u => u.QuestionUnits).LoadAsync();
            }


            return review;
        }

        public async Task<IEnumerable<Review>> GetReportByUserIdAsync(string userId)
        {
            var reviewList = await _dbContext.Reviews
                .Where(r => r.GetApplicantId == userId)
                .Include(u => u.QuestionUnits)
                .ToListAsync();


            if (reviewList == null)
            {
                reviewList = _dbContext.Reviews
                    .Local.Where(r => r.GetApplicantId == userId).ToList();
            }

            if (reviewList != null)
            {
                foreach (var item in reviewList)
                {
                    await _dbContext.Entry(item).Collection(u => u.QuestionUnits).LoadAsync();
                }
            }

            return reviewList;

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