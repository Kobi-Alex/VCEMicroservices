using System;
using MediatR;


namespace Report.API.Application.Features.Commands.OpenReview
{
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    // References on Immutable Commands:  
    // http://cqrs.nu/Faq
    // https://docs.spine3.org/motivation/immutability.html 

    public class OpenReviewCommand : IRequest<int>
    {
        public int ExamId { get; private set; }
        public string ApplicantId { get; private set; }

        public OpenReviewCommand(int examId, string applicantId)
        {
            ExamId = examId;
            ApplicantId = applicantId;
        }
    }
}