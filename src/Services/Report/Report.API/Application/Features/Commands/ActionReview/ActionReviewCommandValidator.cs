using System;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Report.API.Application.Features.Commands.ActionReview
{

    public class ActionReviewCommandValidator : AbstractValidator<ActionReviewCommand>
    {
        public ActionReviewCommandValidator(ILogger<ActionReviewCommandValidator> logger)
        {
            RuleFor(command => command.ExamId).NotEmpty();
            RuleFor(command => command.UserId).NotEmpty();

            logger.LogTrace("--> INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }

}
