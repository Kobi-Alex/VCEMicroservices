using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.Extensions.Logging;

using Report.API.Grpc;
using Report.API.Application.Models;
using Report.API.Application.Exceptions;
using Report.Domain.AggregatesModel.ReviewAggregate;
using Report.Infrastructure.Persistance.Idempotency;
using Report.API.Application.Contracts.Infrastructure;
using Report.API.Application.Features.Commands.Identified;


namespace Report.API.Application.Features.Commands.CloseReview
{

    // Regular CommandHandler
    public class CloseReviewCommandHandler : IRequestHandler<CloseReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ExamGrpcService _examGrpcService;
        private readonly IEmailService _emailService;
        private readonly ILogger<CloseReviewCommandHandler> _logger;

        public CloseReviewCommandHandler(IReviewRepository reviewRepository, ExamGrpcService examGrpcService,
            IEmailService emailService, ILogger<CloseReviewCommandHandler> logger)
        {
            _reviewRepository = reviewRepository;
            _examGrpcService = examGrpcService;
            _emailService = emailService;
            _logger = logger;

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
            if (reviewToUpdate is null)
            {
                return false;
            }


            if (reviewToUpdate.QuestionUnits.Count != 0 )
            {

                // gRPC request to Exam service
                // Getting examItem object from exam service.
                var examItem = await _examGrpcService.GetExamItemFromExamData(reviewToUpdate._examId);

                if (examItem is null)
                {
                    throw new ExamItemNotFoundException(reviewToUpdate._examId);
                }

                // Calculate review scores
                reviewToUpdate.CalculateScores(examItem.CountQuestions);

                // TODO Email content
                // Sending exam result to applicant email..
                await SendMail(reviewToUpdate);

                // Save data
                return await _reviewRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }

            return true;

        }


        /// <summary>
        /// Send mail method
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        private async Task SendMail(Review review)
        {
            var email = new Email()
            {
                To = "steelalex.gk@gmail.com",
                Body = "<strong> Exam result: </strong>",
                Subject = "VCE result"
            };

            try
            {
                await _emailService.SendEmail(email);
                Console.WriteLine("--> Message sended");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Review {review.Id} failed due to an error with the mail service: {ex.Message}");
            }
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