using GrpcReport;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcReport;
using Microsoft.Extensions.Logging;

namespace Question.API.Grpc
{
    public class ReportGrpcService
    {
        private readonly ILogger<ReportGrpcService> _logger;
        private readonly ReportGrpc.ReportGrpcClient _reportGrpcService;

        public ReportGrpcService(ReportGrpc.ReportGrpcClient reportGrpcService, ILogger<ReportGrpcService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reportGrpcService = reportGrpcService ?? throw new ArgumentNullException(nameof(reportGrpcService));
        }

        public async Task<ReportResponse> CheckIfExistsExamInReports(int examId)
        {
            //var request = new GetExamItem { ExamId = examId };

            //return await _examGrpcService.GetExamItemFromExamDataAsync(request);

            var request = new ReportRequest() { ExamId = examId };

            return await _reportGrpcService.CheckIfExistsExamInReportsAsync(request);
        }
    }
}