using System;
using AutoMapper;
using Exam.Domain.Repositories;
using Exam.API.Application.Services.Interfaces;
using Exam.API.Grpc;
using Exam.API.Grpc.Interfaces;

namespace Exam.API.Application.Services
{
    // Our service instances are only going to be created when we access them for the first time,
    // and not before that.
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IExamItemService> _lazyExamItemService;
        private readonly Lazy<IExamQuestionService> _lazyExamQuestionService;
        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, IReportGrpcService reportGrpcService, IApplicantGrpcService applicantGprcService, IQuestionGrpcService questionGrpcService)
        {
            _lazyExamItemService = new Lazy<IExamItemService>(() => new ExamItemService(repositoryManager, mapper, reportGrpcService, applicantGprcService));
            _lazyExamQuestionService = new Lazy<IExamQuestionService>(() => new ExamQuestionService(repositoryManager, mapper, reportGrpcService, questionGrpcService));
        }


        public IExamItemService ExamItemService => _lazyExamItemService.Value;
        public IExamQuestionService ExamQuestionService => _lazyExamQuestionService.Value;
    }
}
