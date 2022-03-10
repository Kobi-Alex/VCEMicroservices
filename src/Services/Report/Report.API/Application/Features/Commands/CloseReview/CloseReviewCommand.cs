using System;
using MediatR;

namespace Report.API.Application.Features.Commands.CloseReview
{
    public class CloseReviewCommand : IRequest<bool>
    {
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public int ExamId { get; set; }

        public CloseReviewCommand()
        {
        }

        public CloseReviewCommand(int reviewId, string userId, int examId)
        {
            ReviewId = reviewId;
            UserId = userId;
            ExamId = examId;
        }
    }
}
