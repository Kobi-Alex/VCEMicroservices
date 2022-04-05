using Grpc.Core;
using GrpcReport;
using Microsoft.Extensions.Logging;
using Report.API.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report.API.Grpc
{
    public class ReportGrpcService : ReportGrpc.ReportGrpcBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<ReportGrpcService> _logger;

        public ReportGrpcService(IServiceManager serviceManager, ILogger<ReportGrpcService> logger)
        {
            _serviceManager = serviceManager ?? throw new ArgumentNullException(nameof(serviceManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<ReportResponse> CheckIfExistsExamInReports(ReportRequest request, ServerCallContext context)
        {
            var res = await _serviceManager.ReportService.GetAllReviewByExamId(request.ExamId);

            if (res != null && res.Count() > 0)
            {
              return  await Task.FromResult(new ReportResponse { Exists = true });
            }

            return await Task.FromResult(new ReportResponse { Exists = false });
        }
    }
}
