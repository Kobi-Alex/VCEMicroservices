using Grpc.Net.Client;
using GrpcApplicant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Report.API.Grpc.Interfaces;
using System;
using System.Threading.Tasks;

namespace Report.API.Grpc
{

    public class ApplicantGrpcService : IApplicantGrpcService
    {
        private readonly ILogger<ApplicantGrpcService> _logger;
        private readonly IConfiguration _configuration;

        public ApplicantGrpcService(ILogger<ApplicantGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }



        public GetUserDataResponse GetUserData(string userId)
        {
            Console.WriteLine($"---> Calling Applicant GRPC Service: {_configuration["GrpcApplicantSettings:ApplicantUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcApplicantSettings:ApplicantUrl"]);
            var client = new ApplicantGrpc.ApplicantGrpcClient(channel);

            try
            {
                var request = new GetUserDataRequest() { UserId = userId };

                return client.GetUseData(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }

        }

        public RemoveExamResponce RemoveExamFromApplicantData(string userId, int examId)
        {
            Console.WriteLine($"---> Calling Applicant GRPC Service: {_configuration["GrpcApplicantSettings:ApplicantUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcApplicantSettings:ApplicantUrl"]);
            var client = new ApplicantGrpc.ApplicantGrpcClient(channel);

            try
            {
                var request = new RemoveExamRequest { UserId = userId, ExamId = examId };

                return client.RemoveExamFromApplicantData(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }
    }


    //public class ApplicantGrpcService
    //{
    //    private readonly ILogger<ApplicantGrpcService> _logger;
    //    private readonly ApplicantGrpc.ApplicantGrpcClient _applicantGrpcService;

    //    public ApplicantGrpcService(ILogger<ApplicantGrpcService> logger, ApplicantGrpc.ApplicantGrpcClient applicantGrpcService)
    //    {
    //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //        _applicantGrpcService = applicantGrpcService ?? throw new ArgumentNullException(nameof(applicantGrpcService));
    //    }

    //    public async Task<RemoveExamResponce> RemoveExamFromApplicantData(string userId, int examId)
    //    {
    //        var request = new RemoveExamRequest { UserId = userId, ExamId = examId };

    //        return await _applicantGrpcService.RemoveExamFromApplicantDataAsync(request);
    //    }

    //    public async Task<GetUserDataResponse> GetUserDataAsync(string userId)
    //    {
    //        var request = new GetUserDataRequest() {UserId = userId };

    //        return await _applicantGrpcService.GetUseDataAsync(request);
    //    }
    //}
}