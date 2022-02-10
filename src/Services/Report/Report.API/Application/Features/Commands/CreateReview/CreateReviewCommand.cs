using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Report.API.Application.Features.Commands.CreateReview
{
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    // References on Immutable Commands:  
    // http://cqrs.nu/Faq
    // https://docs.spine3.org/motivation/immutability.html 

    [DataContract]
    public class CreateReviewCommand 
        : IRequest<bool>
    {
        [DataMember]
        private readonly List<QuestionUnitDTO> _questionUnits;

        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public int ExamId { get; private set; }

        [DataMember]
        public string ApplicantId { get; private set; }

        [DataMember]
        public IEnumerable<QuestionUnitDTO> QuestionUnits => _questionUnits;


        public CreateReviewCommand()
        {
            _questionUnits = new List<QuestionUnitDTO>();

        }

        public CreateReviewCommand(int id, int examId, string userId)
            :this()
        {
            Id = id;
            ExamId = examId;
            ApplicantId = userId;
        }



        public record QuestionUnitDTO
        {
            public int QuestionId { get; init; }
            public string Name { get; init; }
            public string AnswerKeys { get; init; }
            public string CurrentKeys { get; init; }
            public int TotalNumberAnswer { get; init; }
        }

    }
}
