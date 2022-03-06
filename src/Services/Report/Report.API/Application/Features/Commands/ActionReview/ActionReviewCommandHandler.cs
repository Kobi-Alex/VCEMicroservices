using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Report.API.Application.Features.Commands.Identified;
using Report.Domain.AggregatesModel.ReviewAggregate;
using Report.Infrastructure.Persistance.Idempotency;
using MediatR;

namespace Report.API.Application.Features.Commands.ActionReview
{

    // Regular CommandHandler
    public class ActionReviewCommandHandler : IRequestHandler<ActionReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;

        public ActionReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }


        /// <summary>
        /// Handler which processes the command when 
        /// applicant executes send a request that response result
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Return true or false</returns>
        public async Task<bool> Handle(ActionReviewCommand request, CancellationToken cancellationToken)
        {
            // Getting review by application and exam ID
            var reviewToUpdate = await _reviewRepository.GetReportByApplicantIdAsync(request.ExamId, request.UserId.ToString());

            // Check is null
            if(reviewToUpdate is null)
            {
                return false;
            }

            // Calculate review scores
            reviewToUpdate.CalculateScores();

            // Save data
            return await _reviewRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }


    // Use for Idempotency in Command process
    public class ActionReviewIdentifiedCommandHandler : IdentifiedCommandHandler<ActionReviewCommand, bool>
    {
        public ActionReviewIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<ActionReviewCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for processing order.
        }
    }
}