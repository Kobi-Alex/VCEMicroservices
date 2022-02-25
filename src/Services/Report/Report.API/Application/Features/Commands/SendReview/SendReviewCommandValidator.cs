using System;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Report.API.Application.Features.Commands.SendReview
{

    public class SendReviewCommandValidator : AbstractValidator<SendReviewCommand>
    {
        public SendReviewCommandValidator(ILogger<SendReviewCommandValidator> logger)
        {
            RuleFor(command => command.ExamId).NotEmpty();
            RuleFor(command => command.UserId).NotEmpty();

            logger.LogTrace("--> INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }

}
