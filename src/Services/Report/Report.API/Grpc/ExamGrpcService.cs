using System;
using GrpcExam;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Report.API.Grpc.Interfaces;
using Microsoft.Extensions.Configuration;
using Grpc.Net.Client;

namespace Report.API.Grpc
{

    public class ExamGrpcService : IExamGrpcService
    {
        private readonly ILogger<ExamGrpcService> _logger;
        private readonly IConfiguration _configuration;

        public ExamGrpcService(ILogger<ExamGrpcService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }

        public ExamResponse CheckIfQuestionExistsInExam(int questionId)
        {
            Console.WriteLine($"---> Calling Exam GRPC Service: {_configuration["GrpcExamSettings:ExamUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcExamSettings:ExamUrl"]);
            var client = new ExamGrpc.ExamGrpcClient(channel);

            try
            {
                var request = new ExamRequest { QuestionId = questionId };

                return client.CheckIfQuestionExistsInExam(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }

        public ExamItemModel GetExamItemFromExamData(int examId)
        {
            Console.WriteLine($"---> Calling Exam GRPC Service: {_configuration["GrpcExamSettings:ExamUrl"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcExamSettings:ExamUrl"]);
            var client = new ExamGrpc.ExamGrpcClient(channel);

            try
            {
                var request = new GetExamItem { ExamId = examId };

                return client.GetExamItemFromExamData(request);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not call Grpc Server: {ex.Message}");
                return null;
            }
        }
    }

    //public class ExamGrpcService
    //{
    //    private readonly ILogger<ExamGrpcService> _logger;
    //    private readonly ExamGrpc.ExamGrpcClient _examGrpcService;

    //    public ExamGrpcService(ExamGrpc.ExamGrpcClient examGrpcService, ILogger<ExamGrpcService> logger)
    //    {
    //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    //        _examGrpcService = examGrpcService ?? throw new ArgumentNullException(nameof(examGrpcService));
    //    }


    //    public async Task<ExamItemModel> GetExamItemFromExamData(int examId)
    //    {
    //        var request = new GetExamItem { ExamId = examId };

    //        return await _examGrpcService.GetExamItemFromExamDataAsync(request);
    //    }

    //    public async Task<ExamResponse> CheckIfQuestionExistsInExam(int questionId)
    //    {
    //        var request = new ExamRequest { QuestionId = questionId };

    //        return await _examGrpcService.CheckIfQuestionExistsInExamAsync(request);
    //    }


    //}
}