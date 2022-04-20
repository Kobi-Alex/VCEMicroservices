using System;
using GrpcReport;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Applicant.API.Grpc.Interfaces;
using Microsoft.Extensions.Configuration;
using Grpc.Net.Client;

namespace Applicant.API.Grpc
{
    public class ReportGrpcService : IReportGrpcService
    {
        private readonly ILogger<ReportGrpcService> _logger;
        private readonly IConfiguration _configuration;
        private GrpcChannel channel;
        private ReportGrpc.ReportGrpcClient client;


        public ReportGrpcService(ILogger<ReportGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
            channel = GrpcChannel.ForAddress(_configuration["GrpcReportSettings:ReportUrl"]);
            client = new ReportGrpc.ReportGrpcClient(channel);
        }
        public IsExistExamResponse IsExistExamRequest(string userId, int examId)
        {
            Console.WriteLine($"---> calling Report GRPC Service: {_configuration["GrpcReportSettings:ReportUrl"]}");

            //var channel = GrpcChannel.ForAddress(_configuration["GrpcReportSettings:ReportUrl"]);
            //var client = new ReportGrpc.ReportGrpcClient(channel);

            try
            {
                var request = new IsExistExamRequest { UserId = userId, ExamId = examId };

                return client.IsExistExamFromReport(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }

        public UserDataResponse RemoveUserDataFromReport(string userId)
        {
            Console.WriteLine($"---> calling Report GRPC Service: {_configuration["GrpcReportSettings:ReportUrl"]}");

            //var channel = GrpcChannel.ForAddress(_configuration["GrpcReportSettings:ReportUrl"]);
            //var client = new ReportGrpc.ReportGrpcClient(channel);

            try
            {
                var request = new RemoveUserData { UserId = userId };

                return client.RemoveUserDataFromReport(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }
    }
}