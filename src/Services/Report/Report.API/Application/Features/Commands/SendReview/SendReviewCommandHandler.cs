using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Report.API.Application.Features.Commands.Identified;
using Report.Domain.AggregatesModel.ReviewAggregate;
using Report.Infrastructure.Persistance.Idempotency;
using MediatR;

namespace Report.API.Application.Features.Commands.SendReview
{

    // Regular CommandHandler
    public class SendReviewCommandHandler : IRequestHandler<SendReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;

        public SendReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }


        /// <summary>
        /// Handler which processes the command when
        /// customer executes cancel order from app
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Task<bool> Handle(SendReviewCommand request, CancellationToken cancellationToken)
        {

            //TODO logik: send message, create final report for applicant, count exam result <----

            throw new NotImplementedException();
        }
    }


    // Use for Idempotency in Command process
    //public class SendReviewIdentifiedCommandHandler : IdentifiedCommandHandler<SendReviewCommand, bool>
    //{
    //    public SendReviewIdentifiedCommandHandler(
    //        IMediator mediator,
    //        IRequestManager requestManager,
    //        ILogger<IdentifiedCommandHandler<SendReviewCommand, bool>> logger)
    //        : base(mediator, requestManager, logger)
    //    {
    //    }

    //    protected override bool CreateResultForDuplicateRequest()
    //    {
    //        return true; // Ignore duplicate requests for processing order.
    //    }
    //}
}