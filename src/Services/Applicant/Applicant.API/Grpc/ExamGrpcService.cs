using Grpc.Net.Client;
using GrpcExam;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applicant.API.Grpc
{
    public class ExamGrpcService
    {
        private readonly ILogger<ExamGrpcService> _logger;
        private readonly ExamGrpc.ExamGrpcClient _examGrpcClient;
        private readonly IConfiguration _configuration;

        public ExamGrpcService(ILogger<ExamGrpcService> logger, ExamGrpc.ExamGrpcClient examGrpcClient, IConfiguration configuration)
        {
            _logger = logger;
            _examGrpcClient = examGrpcClient;
            _configuration = configuration;
        }

        public  ExamItemModel GetExamItem(int idExam)
        {
            Console.WriteLine($"---> Calling GRPC Service {_configuration["GrpcExamSettings:ExamUrl"]}");


            var request = new GetExamItem() { ExamId = idExam };

            return  _examGrpcClient.GetExamItemFromExamData(request);
        }

        public async Task<bool> CheckTest(int id = 3)
        {
            var request = new TestRequests();

            await _examGrpcClient.CheckTestAsync(request);

            return true;
        }

    }
}
