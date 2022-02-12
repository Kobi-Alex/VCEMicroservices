using System;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Report.API.Application.Features.Commands.SetQuestionUnit;

namespace Report.API.Application.Features.Commands.CreateReview
{

    public class SetQuestionUnitCommandValidator : AbstractValidator<SetQuestionUnitCommand>
    {
        public SetQuestionUnitCommandValidator(ILogger<SetQuestionUnitCommandValidator> logger)
        {
            RuleFor(command => command.ExamId).NotEmpty();
            RuleFor(command => command.ApplicantId).NotEmpty();
            RuleFor(command => command.QuestionId).NotEmpty();
            RuleFor(command => command.Name).NotEmpty().MinimumLength(3);
            RuleFor(command => command.CurrentKeys).NotEmpty();
            RuleFor(command => command.AnswerKeys).NotEmpty();
            RuleFor(command => command.TotalNumberAnswer).NotEmpty();

            logger.LogTrace("--> INSTANCE CREATED - {ClassName}", GetType().Name);
        }
    }

    
}
