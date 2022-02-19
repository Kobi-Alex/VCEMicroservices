using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Report.API.Application.Features.Commands.Identified;
using Report.Domain.AggregatesModel.ReviewAggregate;
using Report.Infrastructure.Persistance.Idempotency;
using MediatR;

namespace Report.API.Application.Features.Commands.CancelReview
{

    // Regular CommandHandler
    public class CancelReviewCommandHandler : IRequestHandler<CancelReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;

        public CancelReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }


        /// <summary>
        /// Handler which processes the command when
        /// customer executes cancel order from app
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Task<bool> Handle(CancelReviewCommand request, CancellationToken cancellationToken)
        {

            //TODO logik: send message, create final report for applicant, count exam result <----

            throw new NotImplementedException();
        }
    }


    // Use for Idempotency in Command process
    public class CancelOrderIdentifiedCommandHandler : IdentifiedCommandHandler<CancelReviewCommand, bool>
    {
        public CancelOrderIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CancelReviewCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for processing order.
        }
    }
}