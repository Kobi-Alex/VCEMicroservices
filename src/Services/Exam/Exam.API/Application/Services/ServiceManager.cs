using System;
using AutoMapper;
using Exam.Domain.Repositories;
using Exam.API.Application.Services.Abstractions;


namespace Exam.API.Application.Services
{
    // Our service instances are only going to be created when we access them for the first time,
    // and not before that.
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IExamItemService> _lazyExamItemService;
        private readonly Lazy<IExamQuestionService> _lazyExamQuestionService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _lazyExamItemService = new Lazy<IExamItemService>(() => new ExamItemService(repositoryManager, mapper));
            _lazyExamQuestionService = new Lazy<IExamQuestionService>(() => new ExamQuestionService(repositoryManager, mapper));
        }


        public IExamItemService ExamItemService => _lazyExamItemService.Value;
        public IExamQuestionService ExamQuestionService => _lazyExamQuestionService.Value;
    }
}
