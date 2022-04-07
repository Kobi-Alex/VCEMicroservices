using GrpcApplicant;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Report.API.Grpc
{
    public class ApplicantGrpcService
    {
        private readonly ILogger<ApplicantGrpcService> _logger;
        private readonly ApplicantGrpc.ApplicantGrpcClient _applicantGrpcService;

        public ApplicantGrpcService(ILogger<ApplicantGrpcService> logger, ApplicantGrpc.ApplicantGrpcClient applicantGrpcService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicantGrpcService = applicantGrpcService ?? throw new ArgumentNullException(nameof(applicantGrpcService));
        }

        public async Task<RemoveExamResponce> RemoveExamFromApplicantData(string userId, int examId)
        {
            var request = new RemoveExamRequest { UserId = userId, ExamId = examId };

            return await _applicantGrpcService.RemoveExamFromApplicantDataAsync(request);
        }
    }
}