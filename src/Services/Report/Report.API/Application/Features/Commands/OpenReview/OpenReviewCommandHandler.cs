using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.Extensions.Logging;
using Report.API.Application.Exceptions;
using Report.Domain.AggregatesModel.ReviewAggregate;


namespace Report.API.Application.Features.Commands.OpenReview
{
    //CQRS pattern comment: regular CommandHandler 
    public class OpenReviewCommandHandler : IRequestHandler<OpenReviewCommand, int>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<OpenReviewCommandHandler> _logger;

        // Using Dependency Injection to inject infrastructure persistence Repositories
        public OpenReviewCommandHandler(IReviewRepository reviewRepository, ILogger<OpenReviewCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
        }

        public async Task<int> Handle(OpenReviewCommand request, CancellationToken cancellationToken)
        {
            // DDD patterns comment: Add child entities and value-objects through the Review Aggregate-Root
            // methods and constructor so validations, invariants and business logic 
            // make sure that consistency is preserved across the whole aggregate

            var review = await _reviewRepository.GetReportByApplicantIdAsync(request.ExamId, request.ApplicantId);

            if (review != null)
            {
                throw new ReviewIsExistException(request.ExamId.ToString());
            }

            // Create new review
            review = new Review(request.ExamId, request.ApplicantId);

            _reviewRepository.Add(review);
            await _reviewRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            // Cmd info
            _logger.LogInformation($"Report {review.Id} is successfully created!!");

            return review.Id;

        }
    }

}
