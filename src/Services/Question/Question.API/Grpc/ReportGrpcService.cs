using Grpc.Net.Client;
using GrpcReport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Question.API.Grpc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Question.API.Grpc
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

        public ReportResponse CheckIfExistsExamInReports(int examId)
        {
            Console.WriteLine($"---> Calling Exam GRPC Service: {_configuration["GrpcReportSettings:ReportUrl"]}");

            try
            {
                var request = new ReportRequest() { ExamId = examId };

                return client.CheckIfExistsExamInReports(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }
    }
}
