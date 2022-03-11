using System;
using GrpcExam;
using Grpc.Core;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Exam.API.Application.Services.Abstractions;

namespace Exam.API.Grpc
{
    //gRPC comment: regular method
    public class ExamGrpcService: ExamGrpc.ExamGrpcBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<ExamGrpcService> _logger;

        public ExamGrpcService(IServiceManager serviceManager, ILogger<ExamGrpcService> logger)
        {
            _serviceManager = serviceManager ?? throw new ArgumentNullException(nameof(serviceManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public override async Task<ExamItemModel> GetExamItemFromExamData(GetExamItem request, ServerCallContext context)
        {
            // Get data from exam service DB by exam ID
            var examItem = await _serviceManager.ExamItemService.GetByIdAsync(request.ExamId);

            // Create response ExamItem Model
            var response = new ExamItemModel();

            // Fields filling...
            response.Title = examItem.Title;
            response.Description = examItem.Description;
            response.PassingScore = (double)examItem.PassingScore;
            response.CountQuestions = examItem.ExamQuestions.Count;

            // Return object
            return await Task.FromResult(response);
        }
    }
}
