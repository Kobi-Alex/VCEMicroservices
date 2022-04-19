using Exam.API.Grpc.Interfaces;
using Grpc.Net.Client;
using GrpcApplicant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.API.Grpc
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
        public UserExamResponse CheckIfExamExistsInUsers(int examId)
        {
            Console.WriteLine($"---> Calling Applicant GRPC Service: {_configuration["GrpcApplicantSettings:ApplicantUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcApplicantSettings:ApplicantUrl"]);
            var client = new ApplicantGrpc.ApplicantGrpcClient(channel);
            try
            {
                UserExamRequest request = new UserExamRequest() { ExamId = examId };

                return client.CheckIfExamExistsInUsers(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }
    }



    //public class ApplicantGprcService
    //{
    //    private readonly ILogger<ApplicantGprcService> _logger;

    //    private readonly ApplicantGrpc.ApplicantGrpcClient _applicantGrpcClient;

    //    public ApplicantGprcService(ApplicantGrpc.ApplicantGrpcClient applicantGrpcClient, ILogger<ApplicantGprcService> logger)
    //    {
    //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //        _applicantGrpcClient = applicantGrpcClient ?? throw new ArgumentNullException(nameof(applicantGrpcClient));
    //    }

    //    public async Task<UserExamResponse> CheckIfExamExistsInUsers(int examId)
    //    {
    //        UserExamRequest request = new UserExamRequest() { ExamId = examId };

    //        return await _applicantGrpcClient.CheckIfExamExistsInUsersAsync(request);
    //    }
    //}
}

