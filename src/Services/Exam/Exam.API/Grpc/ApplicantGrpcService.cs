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
        private GrpcChannel channel;
        private ApplicantGrpc.ApplicantGrpcClient client;
        public ApplicantGrpcService(ILogger<ApplicantGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
             channel = GrpcChannel.ForAddress(_configuration["GrpcApplicantSettings:ApplicantUrl"]);
             client = new ApplicantGrpc.ApplicantGrpcClient(channel);
        }
        public UserExamResponse CheckIfExamExistsInUsers(int examId)
        {
            Console.WriteLine($"---> Calling Applicant GRPC Service: {_configuration["GrpcApplicantSettings:ApplicantUrl"]}");

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
}

