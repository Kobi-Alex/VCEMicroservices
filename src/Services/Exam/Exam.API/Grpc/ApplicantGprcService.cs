using GrpcApplicant;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.API.Grpc
{
    public class ApplicantGprcService
    {
        private readonly ILogger<ApplicantGprcService> _logger;

        private readonly ApplicantGrpc.ApplicantGrpcClient _applicantGrpcClient;

        public ApplicantGprcService(ApplicantGrpc.ApplicantGrpcClient applicantGrpcClient, ILogger<ApplicantGprcService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicantGrpcClient = applicantGrpcClient ?? throw new ArgumentNullException(nameof(applicantGrpcClient));
        }

        public async Task<UserExamResponse> CheckIfExamExistsInUsers(int examId)
        {
            UserExamRequest request = new UserExamRequest() { ExamId = examId };

            return await _applicantGrpcClient.CheckIfExamExistsInUsersAsync(request);
        }
    }
}

//private readonly ILogger<ReportGrpcService> _logger;
//private readonly ReportGrpc.ReportGrpcClient _reportGrpcService;

//public ReportGrpcService(ReportGrpc.ReportGrpcClient reportGrpcService, ILogger<ReportGrpcService> logger)
//{
//    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//    _reportGrpcService = reportGrpcService ?? throw new ArgumentNullException(nameof(reportGrpcService));
//}

//public async Task<ReportResponse> CheckIfExistsExamInReports(int examId)
//{
//    //var request = new GetExamItem { ExamId = examId };

//    //return await _examGrpcService.GetExamItemFromExamDataAsync(request);

//    var request = new ReportRequest() { ExamId = examId };

//    return await _reportGrpcService.CheckIfExistsExamInReportsAsync(request);
//}