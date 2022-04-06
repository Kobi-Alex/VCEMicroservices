using System;
using GrpcExam;
using Grpc.Core;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Exam.API.Application.Services.Interfaces;
using System.Linq;

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
            var examItem = await _serviceManager.ExamItemService.GetByIdIncludeExamQuestionsAsync(request.ExamId);

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

        public override async Task<ExamResponse> CheckIfQuestionExistsInExam(ExamRequest request, ServerCallContext context)
        {
            var exams = await _serviceManager.ExamItemService.GetAllByQuestionId(request.QuestionId);

            ExamResponse response = new ExamResponse();
           
            if (exams != null && exams.Count() > 0)
            {
                response.Exists = true;
                response.Exams.AddRange(exams.Select(x => x.Id));
                return await Task.FromResult(response);
            }

            return await Task.FromResult(response);
        }
    }
}
