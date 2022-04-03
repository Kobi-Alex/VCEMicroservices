using System;
using MediatR;


namespace Report.API.Application.Features.Commands.RemoveReview
{
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    // References on Immutable Commands:  
    // http://cqrs.nu/Faq
    // https://docs.spine3.org/motivation/immutability.html 

    public class RemoveReviewCommand : IRequest<bool>
    {
        public int ReviewId { get; set; }

        public RemoveReviewCommand(int reviewId)
        {
            ReviewId = reviewId;
        }
    }
}
