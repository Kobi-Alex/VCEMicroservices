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

        public ReportGrpcService(ILogger<ReportGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }
        public IsExistExamResponse IsExistExamRequest(string userId, int examId)
        {
            Console.WriteLine($"---> calling Report GRPC Service: {_configuration["GrpcReportSettings:ReportUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcReportSettings:ReportUrl"]);
            var client = new ReportGrpc.ReportGrpcClient(channel);

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

            //var channel = GrpcChannel.ForAddress(_configuration["GrpcExamSettings:ExamUrl"]);
            //var client = new ExamGrpc.ExamGrpcClient(channel);
            //try
            //{
            //    var request = new GetExamItem() { ExamId = idExam };

            //    return client.GetExamItemFromExamData(request);
            //}
            //catch (Exception ex)
            //{

            //    Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
            //    return null;
            //}
        }

        public UserDataResponse RemoveUserDataFromReport(string userId)
        {
            Console.WriteLine($"---> calling Report GRPC Service: {_configuration["GrpcReportSettings:ReportUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcReportSettings:ReportUrl"]);
            var client = new ReportGrpc.ReportGrpcClient(channel);

            try
            {
                var request = new RemoveUserData { UserId = userId };

                return  client.RemoveUserDataFromReport(request);
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

    //    public ReportGrpcService(ILogger<ReportGrpcService> logger, ReportGrpc.ReportGrpcClient reportGrpcService)
    //    {
    //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //        _reportGrpcService = reportGrpcService ?? throw new ArgumentNullException(nameof(reportGrpcService));
    //    }


    //    public async Task<UserDataResponse> RemoveUserDataFromRepor(string userId)
    //    {
    //        var request = new RemoveUserData { UserId = userId };

    //        return await _reportGrpcService.RemoveUserDataFromReportAsync(request);
    //    }

    //    public async Task<IsExistExamResponse> IsExistExamRequest (string userId, int examId)
    //    {
    //        var request = new IsExistExamRequest { UserId = userId, ExamId = examId };

    //        return await _reportGrpcService.IsExistExamFromReportAsync(request);
    //    }
    //}
}