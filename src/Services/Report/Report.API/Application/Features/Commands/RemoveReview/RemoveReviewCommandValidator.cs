using System;
using FluentValidation;
using Microsoft.Extensions.Logging;


namespace Report.API.Application.Features.Commands.RemoveReview
{

    public class RemoveReviewCommandValidator : AbstractValidator<RemoveReviewCommand>
    {
        public RemoveReviewCommandValidator(ILogger<RemoveReviewCommandValidator> logger)
        {
            RuleFor(command => command.ReviewId).NotEmpty();
            logger.LogTrace("--> INSTANCE REMOVED - {ClassName}", GetType().Name);
        }
    }
}