using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            EventId = Guid.NewGuid();
            EventCreationDate = DateTime.UtcNow;
        }

        public Guid EventId { get; private set; }

        public DateTime EventCreationDate { get; private set; }

        public IntegrationEvent(Guid eventId, DateTime eventCreateDate)
        {
            EventId = eventId;
            EventCreationDate = eventCreateDate;
        }

    }
}
