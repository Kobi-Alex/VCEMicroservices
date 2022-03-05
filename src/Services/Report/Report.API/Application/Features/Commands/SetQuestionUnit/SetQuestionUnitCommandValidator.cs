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
            RuleFor(command => command.CurrentKeys).NotEmpty();

            logger.LogTrace("--> INSTANCE CREATED - {ClassName}", GetType().Name);

        }

        //RuleFor(command => command.CurrentKeys).Must(BeAValidChar)
            //.WithMessage("The current key must not contain the following characters(' ', ',', '.', ':', 'tab')");
        
        //private bool BeAValidChar(string arg)
        //{
        //    if (Regex.IsMatch(arg, @"^[a-zA-Z]+$"))
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}