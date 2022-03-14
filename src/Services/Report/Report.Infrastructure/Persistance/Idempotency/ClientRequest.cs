using System;


namespace Report.Infrastructure.Persistance.Idempotency
{
    public class ClientRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}
