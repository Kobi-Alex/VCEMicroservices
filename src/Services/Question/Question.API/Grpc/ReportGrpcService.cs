using System;
using System.Threading.Tasks;
using GrpcReport;
using Microsoft.Extensions.Logging;

namespace Question.API.Grpc
{
    public class ReportGrpcService
    {
        private readonly ILogger<ReportGrpcService> _logger;
        private readonly ReportGrpc.ReportGrpcClient _reportGrpcService;

        public ReportGrpcService(ILogger<ReportGrpcService> logger, ReportGrpc.ReportGrpcClient reportGrpcService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reportGrpcService = reportGrpcService ?? throw new ArgumentNullException(nameof(reportGrpcService));
        }


        public async Task<PermissionResponse> GetPermissionToDeleteQuestion()
        {
            var request = new PermissionRequest { };

            return await _reportGrpcService.GetPermissionToDeleteQuestionAsync(request);
        }
    }
}