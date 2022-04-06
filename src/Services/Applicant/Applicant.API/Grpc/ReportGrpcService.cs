using System;
using GrpcReport;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Applicant.API.Grpc
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


        public async Task<UserDataResponse> RemoveUserDataFromRepor(string userId)
        {
            var request = new RemoveUserData { UserId = userId };

            return await _reportGrpcService.RemoveUserDataFromReportAsync(request);
        }
    }
}