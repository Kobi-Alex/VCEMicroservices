using System;
using MediatR;

namespace Report.API.Application.Features.Commands.CancelReview
{
    public class CancelReviewCommand : IRequest<bool>
    {
        public int ExamId { get; set; }
        public int UserId { get; set; }

        public CancelReviewCommand()
        {

        }

        public CancelReviewCommand(int examId, int userId)
        {
            ExamId = examId;
            UserId = userId;
        }
    }
}
