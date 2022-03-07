using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Report.API.Application.Features.Queries
{
    public class ReviewQueries : IReviewQueries
    {
        private string _connectionString = string.Empty;

        public ReviewQueries(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($"'{nameof(connectionString)}' cannot be null or whitespace.", nameof(connectionString));
            }

            _connectionString = connectionString;
        }


        //Dapper comment
        //Get all reports by exam id, include question Units
        public async Task<IEnumerable<Review>> GetReportsByExamIdAsync(int examId)
        {
            var query = "SELECT* " +
                        "FROM report.reviews r JOIN report.questionUnits qu ON r.Id = qu.ReviewId " +
                        "WHERE r.ExamId = @examId";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var reviewDict = new Dictionary<int, Review>();

                var reviews = await connection.QueryAsync<Review, QuestionUnit, Review>(
                    query, (review, questionUnit) =>
                    {
                        if(!reviewDict.TryGetValue(review.Id, out var currentReview))
                        {
                            currentReview = review;
                            reviewDict.Add(currentReview.Id, currentReview);
                        }

                        currentReview.QuestionUnits.Add(questionUnit);
                        return currentReview;

                    }, param: new { examId }
                );

                return reviews.Distinct().ToList();
            }
        }

        //Dapper comment
        //Get all reports by exam id and user id, include question Units
        public async Task<IEnumerable<Review>> GetReportsByExamIdAndUserIdAsync(int examId, string userId)
        {
            var query = "SELECT* " +
                        "FROM report.reviews r JOIN report.questionUnits qu ON r.Id = qu.ReviewId " +
                        "WHERE r.ExamId = @examId and r.ApplicantId = @userId";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var reviewDict = new Dictionary<int, Review>();

                var reviews = await connection.QueryAsync<Review, QuestionUnit, Review>(
                    query, (review, questionUnit) =>
                    {
                        if (!reviewDict.TryGetValue(review.Id, out var currentReview))
                        {
                            currentReview = review;
                            reviewDict.Add(currentReview.Id, currentReview);
                        }

                        currentReview.QuestionUnits.Add(questionUnit);
                        return currentReview;

                    }, param: new { examId, userId }
                );

                return reviews.Distinct().ToList();
            }
        }


        //Dapper comment
        //Get all reports by user id, include question Units
        public async Task<IEnumerable<Review>> GetReportByUserIdAsync(string userId)
        {
            var query = "SELECT* " +
                        "FROM report.reviews r JOIN report.questionUnits qu ON r.Id = qu.ReviewId " +
                        "WHERE r.ApplicantId = @userId";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var reviewDict = new Dictionary<int, Review>();

                var reviews = await connection.QueryAsync<Review, QuestionUnit, Review>(
                    query, (review, questionUnit) =>
                    {
                        if (!reviewDict.TryGetValue(review.Id, out var currentReview))
                        {
                            currentReview = review;
                            reviewDict.Add(currentReview.Id, currentReview);
                        }

                        currentReview.QuestionUnits.Add(questionUnit);
                        return currentReview;

                    }, param: new { userId }
                );

                return reviews.Distinct().ToList();
            }
        }


        // Dapper comment
        // Get all reports
        public async Task<IEnumerable<Review>> GetAll()
        {
            var query = "SELECT * FROM report.reviews";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var companies = await connection.QueryAsync<Review>(query);
                
                return companies.ToList();
            }
        }
    }
}
