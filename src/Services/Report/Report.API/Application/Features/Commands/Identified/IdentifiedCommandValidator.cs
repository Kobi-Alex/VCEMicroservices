using System;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Report.API.Application.Features.Commands.CloseReview;
using Report.API.Application.Features.Commands.Identified;

namespace Report.API.Application.Features.Commands.CancelReview
{
    public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CloseReviewCommand, bool>>
    {
        public IdentifiedCommandValidator(ILogger<IdentifiedCommandValidator> logger)
        {
            RuleFor(command => command.Id).NotEmpty();

            logger.LogTrace("----- INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }
}