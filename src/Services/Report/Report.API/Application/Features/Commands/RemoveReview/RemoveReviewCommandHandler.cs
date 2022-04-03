using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.Extensions.Logging;

using Report.API.Application.Exceptions;
using Report.Domain.AggregatesModel.ReviewAggregate;
using Report.Infrastructure.Persistance.Idempotency;


namespace Report.API.Application.Features.Commands.RemoveReview
{
    public class RemoveReviewCommandHandler : IRequestHandler<RemoveReviewCommand, bool>
    {

        private readonly IRequestManager _requestManager; 
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<RemoveReviewCommand> _logger;

        public RemoveReviewCommandHandler(IRequestManager requestManager, IReviewRepository reviewRepository, ILogger<RemoveReviewCommand> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
            _requestManager = requestManager ?? throw new ArgumentNullException(nameof(requestManager)); 
        }

        public async Task<bool> Handle(RemoveReviewCommand request, CancellationToken cancellationToken)
        {
            // Finding review by Id;
            var review = await _reviewRepository.GetReportByReviewIdAsync(request.ReviewId);

            // Check review
            if (review is null)
            {
                throw new ReviewNotFoundException(nameof(Review), request);
            }

            // Remove client request
            _requestManager.Remove(review.Id);

            // Remove review
            _reviewRepository.Remove(review);
            _logger.LogInformation($"Report {review.Id} is successfully removed!!");

            return await _reviewRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}