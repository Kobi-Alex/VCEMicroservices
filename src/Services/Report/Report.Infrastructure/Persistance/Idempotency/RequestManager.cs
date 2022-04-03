using System;
using System.Threading.Tasks;
using Report.Domain.Exceptions;


namespace Report.Infrastructure.Persistance.Idempotency
{
    public class RequestManager : IRequestManager
    {

        private readonly ReportDbContext _context;

        public RequestManager(ReportDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateRequestForCommandAsync<T>(int id)
        {
            var exists = await ExistAsync(id);

            var request = exists ?
                throw new ReportDomainException($"Request with {id} already exists") :
                new ClientRequest()
                {
                    Id = id,
                    Name = typeof(T).Name,
                    Time = DateTime.Now
                };

            _context.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            var request = await _context.FindAsync<ClientRequest>(id);
            return request != null;
        }

        public void Remove(int id)
        {
            var clientRequest = _context.Find<ClientRequest>(id);

            if (clientRequest is null)
            {
                throw new ReportDomainException($"Request with {id} was not found");
            }

            _context.Remove(clientRequest);
        }
    }
}
