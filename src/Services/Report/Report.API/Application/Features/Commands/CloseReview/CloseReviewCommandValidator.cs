using System;
using FluentValidation;
using Microsoft.Extensions.Logging;


namespace Report.API.Application.Features.Commands.CloseReview
{

    public class CloseReviewCommandValidator : AbstractValidator<CloseReviewCommand>
    {
        public CloseReviewCommandValidator(ILogger<CloseReviewCommandValidator> logger)
        {
            RuleFor(command => command.ReviewId).NotEmpty();

            logger.LogTrace("--> INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }

}
