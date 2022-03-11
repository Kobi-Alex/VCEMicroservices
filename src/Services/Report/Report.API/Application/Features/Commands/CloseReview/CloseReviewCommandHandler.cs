using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Report.Domain.AggregatesModel.ReviewAggregate;
using Report.Infrastructure.Persistance.Idempotency;
using Report.API.Application.Features.Commands.Identified;
using MediatR;
using Report.API.Grpc;
using Report.API.Application.Exceptions;

namespace Report.API.Application.Features.Commands.CloseReview
{

    // Regular CommandHandler
    public class CloseReviewCommandHandler : IRequestHandler<CloseReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ExamGrpcService _examGrpcService;

        public CloseReviewCommandHandler(IReviewRepository reviewRepository, ExamGrpcService examGrpcService)
        {
            _reviewRepository = reviewRepository;
            _examGrpcService = examGrpcService;
        }


        /// <summary>
        /// Handler which processes the command when 
        /// applicant executes send a request that response result
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Return true or false</returns>
        public async Task<bool> Handle(CloseReviewCommand request, CancellationToken cancellationToken)
        {
            // Getting review by application and exam ID
            //var reviewToUpdate = await _reviewRepository.GetReportByApplicantIdAsync(request.ExamId, request.UserId.ToString());

            // Getting review by review ID 
            var reviewToUpdate = await _reviewRepository.GetReportByReviewIdAsync(request.ReviewId);

            // Check is null
            if(reviewToUpdate is null)
            {
                return false;
            }

            // gRPC request to Exam service
            // Getting examItem object from exam service.
            var examItem = await _examGrpcService.GetExamItemFromExamData(request.ExamId);

            if (examItem is null)
            {
                throw new ExamItemNotFoundException(request.ExamId);
            }

            // Calculate review scores
            reviewToUpdate.CalculateScores(examItem.CountQuestions);

            // Save data
            return await _reviewRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }


    // Use for Idempotency in Command process
    public class ActionReviewIdentifiedCommandHandler : IdentifiedCommandHandler<CloseReviewCommand, bool>
    {
        public ActionReviewIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<CloseReviewCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for processing order.
        }
    }
}