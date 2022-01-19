using System;
using System.Threading.Tasks;
using EventBus.Events;
using MassTransit;

namespace Exam.API.Application.IntegrationEvents.Events
{
    public class QuestionItemDeleteEvent : IntegrationEvent
    {
        public int Id { get; set; }
        public string Context { get; set; }
    }
}
