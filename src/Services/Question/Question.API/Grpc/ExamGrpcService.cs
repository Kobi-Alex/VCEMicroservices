using GrpcExam;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Question.API.Grpc
{
    public class ExamGrpcService
    {
        private readonly ILogger<ExamGrpcService> _logger;
        private readonly ExamGrpc.ExamGrpcClient _examGrpcService;

        public ExamGrpcService(ExamGrpc.ExamGrpcClient examGrpcService, ILogger<ExamGrpcService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _examGrpcService = examGrpcService ?? throw new ArgumentNullException(nameof(examGrpcService));
        }


        public async Task<ExamItemModel> GetExamItemFromExamData(int examId)
        {
            var request = new GetExamItem { ExamId = examId };

            return await _examGrpcService.GetExamItemFromExamDataAsync(request);
        }

        public async Task<ExamResponse> CheckIfQuestionExistsInExam(int questionId)
        {
            var request = new ExamRequest { QuestionId = questionId };

            return await _examGrpcService.CheckIfQuestionExistsInExamAsync(request);
        }


    }
}
