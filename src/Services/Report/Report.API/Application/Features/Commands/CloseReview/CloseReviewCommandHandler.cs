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
using Report.API.Grpc.Interfaces;

namespace Report.API.Application.Features.Commands.CloseReview
{

    // Regular CommandHandler
    public class CloseReviewCommandHandler : IRequestHandler<CloseReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IExamGrpcService _examGrpcService;
        private readonly IApplicantGrpcService _applicantGrpcService;
        private readonly IEmailService _emailService;
        private readonly ILogger<CloseReviewCommandHandler> _logger;

        public CloseReviewCommandHandler(IReviewRepository reviewRepository, IExamGrpcService examGrpcService,
            IEmailService emailService, ILogger<CloseReviewCommandHandler> logger, IApplicantGrpcService applicantGrpcService)
        {
            _reviewRepository = reviewRepository;
            _examGrpcService = examGrpcService;
            _emailService = emailService;
            _logger = logger;
            _applicantGrpcService = applicantGrpcService;

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


            // gRPC request to Exam service
            // Getting examItem object from exam service.
            var examItem =  _examGrpcService.GetExamItemFromExamData(reviewToUpdate._examId);

            if (examItem is null)
            {
                throw new ExamItemNotFoundException(reviewToUpdate._examId);
            }

            // Calculate review scores
            reviewToUpdate.CalculateScores(examItem.CountQuestions);

            // gRPC Service Remove exam in applicant service database
             _applicantGrpcService.RemoveExamFromApplicantData(reviewToUpdate._applicantId, reviewToUpdate._examId);

            // TODO add E-mail content
            // Sending exam result to applicant email..
            await SendMail(reviewToUpdate);

            // Save data
            return await _reviewRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);


        }


        /// <summary>
        /// Send mail method
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        private async Task SendMail(Review review)
        {
            var user =  _applicantGrpcService.GetUserData(review._applicantId);
            var exam =  _examGrpcService.GetExamItemFromExamData(review._examId);

            string status = (review.GetGradeByPersentScore() != "F" && review.GetGradeByPersentScore() != null) ? "Test passed!" : "Test failed!";

            string body = $"<h1>Hi, {user.FirstName } {user.LastName}</h1>" +
                $"<h2>Your result by exam: {exam.Title} </h2>" +
                $"<h3>Grade: {review.GetGradeByPersentScore()}</h3>" +
                $"<h3>Date: {review.GetDateReview().ToLongDateString()}</h3>" +
                $"<h2>Status: {status}</h2>";


            var email = new Email()
            {
                //To = "steelalex.gk@gmail.com",
                To = user.Email,
                Body = body,
                Subject = "VCE result"
            };

            try
            {
                await _emailService.SendEmail(email);
                Console.WriteLine($"\n---> Message sended to: {email.To}");
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