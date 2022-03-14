using System;
using MediatR;


namespace Report.API.Application.Features.Commands.CloseReview
{
    public class CloseReviewCommand : IRequest<bool>
    {
        public int ReviewId { get; set; }

        public CloseReviewCommand()
        {
        }

        public CloseReviewCommand(int reviewId)
        {
            ReviewId = reviewId;
        }
    }
}
