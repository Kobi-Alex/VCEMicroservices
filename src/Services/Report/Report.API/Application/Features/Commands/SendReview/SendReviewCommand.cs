using System;
using MediatR;

namespace Report.API.Application.Features.Commands.SendReview
{
    public class SendReviewCommand : IRequest<bool>
    {
        public int ExamId { get; set; }
        public int UserId { get; set; }

        public SendReviewCommand()
        {
        }

        public SendReviewCommand(int examId, int userId)
        {
            ExamId = examId;
            UserId = userId;
        }
    }
}
