using System;
using System.Threading.Tasks;

namespace Exam.API.Application.IntegrationEvents
{
    public interface IExamIntegrationEventService
    {
        Task SaveEventAndCatalogContextChangesAsync();
    }
}
