using System;
using FluentValidation;
using Microsoft.Extensions.Logging;


namespace Report.API.Application.Features.Commands.OpenReview
{

    public class OpenReviewCommandValidator : AbstractValidator<OpenReviewCommand>
    {
        public OpenReviewCommandValidator(ILogger<OpenReviewCommandValidator> logger)
        {
            RuleFor(command => command.ExamId).NotEmpty();
            RuleFor(command => command.ApplicantId).NotEmpty();
            logger.LogTrace("--> INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }

}
