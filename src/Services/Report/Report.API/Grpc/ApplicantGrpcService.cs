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
        private GrpcChannel channel;
        private ApplicantGrpc.ApplicantGrpcClient client;


        public ApplicantGrpcService(ILogger<ApplicantGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
             channel = GrpcChannel.ForAddress(_configuration["GrpcApplicantSettings:ApplicantUrl"]);
             client = new ApplicantGrpc.ApplicantGrpcClient(channel);
        }



        public GetUserDataResponse GetUserData(string userId)
        {
            Console.WriteLine($"---> Calling Applicant GRPC Service: {_configuration["GrpcApplicantSettings:ApplicantUrl"]}");

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
}