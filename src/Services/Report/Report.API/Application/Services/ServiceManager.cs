using AutoMapper;
using Report.API.Application.Features.Queries;
using Report.API.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report.API.Application.Services
{
    // Our service instances are only going to be created when we access them for the first time,
    // and not before that.
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IReportService> _lazyReportService;


        public ServiceManager(IReviewQueries _reviewQueries)
        {
            _lazyReportService = new Lazy<IReportService>(() => new ReportService(_reviewQueries));
        }


        public IReportService ReportService => _lazyReportService.Value;
    }
}
