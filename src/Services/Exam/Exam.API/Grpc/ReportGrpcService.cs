using Exam.API.Grpc.Interfaces;
using Grpc.Net.Client;
using GrpcReport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.API.Grpc
{
    public class ReportGrpcService : IReportGrpcService
    {
        private readonly ILogger<ReportGrpcService> _logger;
        private readonly IConfiguration _configuration;

        public ReportGrpcService(ILogger<ReportGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }

        public ReportResponse CheckIfExistsExamInReports(int examId)
        {
            Console.WriteLine($"---> Calling Report GRPC Service: {_configuration["GrpcReportSettings:ReportUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcReportSettings:ReportUrl"]);
            var client = new ReportGrpc.ReportGrpcClient(channel);

            try
            {
                var request = new ReportRequest() { ExamId = examId };

               return  client.CheckIfExistsExamInReports(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");

                return null;
            }


        }
    }

    //public class ReportGrpcService
    //{
    //    private readonly ILogger<ReportGrpcService> _logger;
    //    private readonly ReportGrpc.ReportGrpcClient _reportGrpcService;

    //    public ReportGrpcService(ReportGrpc.ReportGrpcClient reportGrpcService, ILogger<ReportGrpcService> logger)
    //    {
    //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //        _reportGrpcService = reportGrpcService ?? throw new ArgumentNullException(nameof(reportGrpcService));
    //    }

    //    public async Task<ReportResponse> CheckIfExistsExamInReports(int examId )
    //    {

    //        var request = new ReportRequest() { ExamId = examId };

    //        return await _reportGrpcService.CheckIfExistsExamInReportsAsync(request);
    //    }
    //}
}
