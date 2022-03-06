using System;
using MediatR;

namespace Report.API.Application.Features.Commands.ActionReview
{
    public class ActionReviewCommand : IRequest<bool>
    {
        public int ExamId { get; set; }
        public string UserId { get; set; }

        public ActionReviewCommand()
        {
        }

        public ActionReviewCommand(int examId, string userId)
        {
            ExamId = examId;
            UserId = userId;
        }
    }
}
