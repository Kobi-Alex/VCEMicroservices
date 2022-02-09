using MediatR;
using Microsoft.Extensions.Logging;
using Report.API.Application.Exceptions;
using Report.Domain.AggregatesModel.ReviewAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Report.API.Application.Features.Commands.CreateReview
{
    //CQRS pattern comment: regular CommandHandler 
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<CreateReviewCommandHandler> _logger;

        // Using Dependency Injection to inject infrastructure persistence Repositories
        public CreateReviewCommandHandler(IReviewRepository reviewRepository, IMediator mediator, ILogger<CreateReviewCommandHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
        }

        public async Task<bool> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // DDD patterns comment: Add child entities and value-objects through the Review Aggregate-Root
            // methods and constructor so validations, invariants and business logic 
            // make sure that consistency is preserved across the whole aggregate
            
            var review = await _reviewRepository.GetReportByReviewIdAsync(request.Id);
            if (review == null)
            {
                review = new Review(request.ExamId, request.ApplicantId);
                //throw new ReviewNotFoundException(nameof(Review), request.Id);
                _logger.LogInformation($"Report {review.Id} is successfully created.");
            }

            //review.AddQuestionUnit(request.QuestionUnits.)

            //foreach (var item in request.QuestionUnits)
            //{

            //}

                _reviewRepository.Add(review);
            return await _reviewRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
