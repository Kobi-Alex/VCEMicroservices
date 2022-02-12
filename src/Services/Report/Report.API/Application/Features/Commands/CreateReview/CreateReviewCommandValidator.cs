using System;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Report.API.Application.Features.Commands.CreateReview
{

    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator(ILogger<CreateReviewCommandValidator> logger)
        {
            RuleFor(command => command.ExamId).NotEmpty();
            RuleFor(command => command.ApplicantId).NotEmpty();

            logger.LogTrace("--> INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }

    
}
