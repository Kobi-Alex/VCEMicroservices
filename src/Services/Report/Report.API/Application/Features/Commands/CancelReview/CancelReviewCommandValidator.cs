using System;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Report.API.Application.Features.Commands.CancelReview
{

    public class CancelReviewCommandValidator : AbstractValidator<CancelReviewCommand>
    {
        public CancelReviewCommandValidator(ILogger<CancelReviewCommandValidator> logger)
        {
            RuleFor(command => command.ExamId).NotEmpty();
            RuleFor(command => command.UserId).NotEmpty();

            logger.LogTrace("--> INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }

}
