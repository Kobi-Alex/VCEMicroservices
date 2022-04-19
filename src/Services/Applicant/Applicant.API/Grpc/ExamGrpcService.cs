using Applicant.API.Grpc.Interfaces;
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
    public class ExamGrpcService : IExamGrpcService
    {
        private readonly ILogger<ExamGrpcService> _logger;
        private readonly IConfiguration _configuration;

        public ExamGrpcService(ILogger<ExamGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public bool CheckTest(int id)
        {
            Console.WriteLine($"---> calling Exam GRPC Service: {_configuration["GrpcExamSettings:ExamUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcExamSettings:ExamUrl"]);
            var client = new ExamGrpc.ExamGrpcClient(channel);

            try
            {
                var request = new TestRequests();

                client.CheckTest(request);

                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return false;
            }
        }

        public ExamItemModel GetExamItem(int idExam)
        {
            Console.WriteLine($"---> calling Exam GRPC Service: {_configuration["GrpcExamSettings:ExamUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcExamSettings:ExamUrl"]);
            var client = new ExamGrpc.ExamGrpcClient(channel);
            try
            {
                var request = new GetExamItem() { ExamId = idExam };

                return client.GetExamItemFromExamData(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }

            // var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform:Plat"]);
            // var client = new GrpcPlatform.GrpcPlatformClient(channel);

            // var request = new GetAllRequest();

            //try
            //{
            //    var reply = client.GetAllPlatforms(request);
            //    return new List<User>();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
            //    return null;
            //}

        }
    }



    //public class ExamGrpcService
    //{
    //    private readonly ILogger<ExamGrpcService> _logger;
    //    private readonly ExamGrpc.ExamGrpcClient _examGrpcClient;
    //    private readonly IConfiguration _configuration;

    //    public ExamGrpcService(ILogger<ExamGrpcService> logger, ExamGrpc.ExamGrpcClient examGrpcClient, IConfiguration configuration)
    //    {
    //        _logger = logger;
    //        _examGrpcClient = examGrpcClient;
    //        _configuration = configuration;
    //    }

    //    public  ExamItemModel GetExamItem(int idExam)
    //    {
    //        Console.WriteLine($"---> Calling GRPC Service {_configuration["GrpcExamSettings:ExamUrl"]}");


    //        var request = new GetExamItem() { ExamId = idExam };

    //        return  _examGrpcClient.GetExamItemFromExamData(request);
    //    }

    //    public async Task<bool> CheckTest(int id = 3)
    //    {
    //        var request = new TestRequests();

    //        await _examGrpcClient.CheckTestAsync(request);

    //        return true;
    //    }

    //}
}
