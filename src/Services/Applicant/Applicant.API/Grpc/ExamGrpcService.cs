using GrpcExam;
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

        public ExamGrpcService(ILogger<ExamGrpcService> logger, ExamGrpc.ExamGrpcClient examGrpcClient)
        {
            _logger = logger;
            _examGrpcClient = examGrpcClient;
        }

        public async Task<ExamItemModel> GetExamItemAsync(int idExam)
        {

            var request = new GetExamItem() { ExamId = idExam };

            return await _examGrpcClient.GetExamItemFromExamDataAsync(request);
        }

    }
}
