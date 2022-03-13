using System;
using EventBus.Events;


namespace Exam.API.Application.IntegrationEvents.Events
{
    public class QuestionItemDeleteEvent : IntegrationEvent
    {
        public int Id { get; set; }
        public string Context { get; set; }
    }
}
