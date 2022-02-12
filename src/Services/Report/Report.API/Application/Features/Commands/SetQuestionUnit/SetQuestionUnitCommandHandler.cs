using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Report.API.Application.Exceptions;
using Report.API.Application.Features.Commands.CreateReview;
using Report.Domain.AggregatesModel.ReviewAggregate;

namespace Report.API.Application.Features.Commands.SetQuestionUnit
{
    //CQRS pattern comment: regular CommandHandler 

    public class SetQuestionUnitCommandHandler : IRequestHandler<SetQuestionUnitCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<SetQuestionUnitCommandHandler> _logger;

        // Using Dependency Injection to inject infrastructure persistence Repositories
        public SetQuestionUnitCommandHandler(IMediator mediator, IReviewRepository reviewRepository, ILogger<SetQuestionUnitCommandHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
        }

        public async Task<bool> Handle(SetQuestionUnitCommand request, CancellationToken cancellationToken)
        {
            // DDD patterns comment: Add child entities and value-objects through the Review Aggregate-Root
            // methods and constructor so validations, invariants and business logic 
            // make sure that consistency is preserved across the whole aggregate


            //If review is NULL than crete new Review class
            if (await _reviewRepository.GetReportByApplicantIdAsync(request.ExamId, request.ApplicantId) == null)
            {
                //request CreateReviewCommand
                await _mediator.Send(new CreateReviewCommand(request.ExamId, request.ApplicantId), cancellationToken);
            }

            var review = await _reviewRepository.GetReportByApplicantIdAsync(request.ExamId, request.ApplicantId);

            if (review == null)
            {
                throw new ReviewNotFoundException(nameof(Review), request);
            }

            //Add questionUnits data for review aggregate;
            review.AddQuestionUnit(request.Name, request.AnswerKeys, request.CurrentKeys,
                request.TotalNumberAnswer, request.QuestionId);

            _reviewRepository.Update(review);

            return await _reviewRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
