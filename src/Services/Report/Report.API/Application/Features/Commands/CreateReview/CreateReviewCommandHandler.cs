using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Report.Domain.AggregatesModel.ReviewAggregate;
using MediatR;
using Report.API.Application.Exceptions;

namespace Report.API.Application.Features.Commands.CreateReview
{
    //CQRS pattern comment: regular CommandHandler 
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<CreateReviewCommandHandler> _logger;

        // Using Dependency Injection to inject infrastructure persistence Repositories
        public CreateReviewCommandHandler(IReviewRepository reviewRepository, ILogger<CreateReviewCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
        }

        public async Task<bool> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // DDD patterns comment: Add child entities and value-objects through the Review Aggregate-Root
            // methods and constructor so validations, invariants and business logic 
            // make sure that consistency is preserved across the whole aggregate
            
            
            var review = new Review(request.ExamId, request.ApplicantId);
            _logger.LogInformation($"Report {review.Id} is successfully created.");

            if(review == null)
            {
                throw new ReviewNullException(nameof(review));
            }

            _reviewRepository.Add(review);
            return await _reviewRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
