using System;
using GrpcExam;
using Grpc.Core;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Exam.API.Application.Services.Interfaces;
using System.Linq;
using Exam.Domain.Repositories;

namespace Exam.API.Grpc
{
    //gRPC comment: regular method
    public class ExamGrpcService: ExamGrpc.ExamGrpcBase
    {
        //private readonly IServiceManager _serviceManager;
        private readonly ILogger<ExamGrpcService> _logger;
        private readonly IRepositoryManager _repositoryManager;

        public ExamGrpcService(IRepositoryManager repositoryManager, ILogger<ExamGrpcService> logger)
        {
            _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public override async Task<ExamItemModel> GetExamItemFromExamData(GetExamItem request, ServerCallContext context)
        {
            Console.WriteLine($"Request: {request.ExamId}");
            // Get data from exam service DB by exam ID
            //var examItem = await _serviceManager.ExamItemService.GetByIdIncludeExamQuestionsAsync(request.ExamId);
            var examItem = await _repositoryManager.ExamItemRepository.GetByIdIncludeExamQustionsAsync(request.ExamId);

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
            //var exams = await _serviceManager.ExamItemService.GetAllByQuestionId(request.QuestionId);
            var exams = await _repositoryManager.ExamItemRepository.FindAll(x => x.ExamQuestions.Where(x => x.QuestionItemId ==request.QuestionId).Any());

            ExamResponse response = new ExamResponse();
           
            if (exams != null && exams.Count() > 0)
            {
                response.Exists = true;
                response.Exams.AddRange(exams.Select(x => x.Id));
                return await Task.FromResult(response);
            }

            return await Task.FromResult(response);
        }

        public override async Task<ExamQuestionsResponse> GetExamQuestions(GetExamItem request, ServerCallContext context)
        {
            var exam = await _repositoryManager.ExamItemRepository.GetByIdIncludeExamQustionsAsync(request.ExamId);


            var response = new ExamQuestionsResponse();

            if (exam == null)
            {
                response.Exists = false;
                return await Task.FromResult(response);
            }

            response.Exists = true;
            response.Questions.AddRange(exam.ExamQuestions.Select(x => x.QuestionItemId));


            return await Task.FromResult(response);
        }
    }
}
