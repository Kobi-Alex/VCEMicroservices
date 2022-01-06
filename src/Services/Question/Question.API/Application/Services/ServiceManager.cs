using System;
using AutoMapper;
using Question.API.Application.Services.Interfaces;
using Question.Domain.Repositories;

namespace Question.API.Application.Services
{
    //The interesting part with the ServiceManager implementation is that we are leveraging the power
    //of the Lazy class to ensure the lazy initialization of our services.
    //This means that our service instances are only going to be created when we access them for the first time,
    //and not before that.

    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IQuestionItemService> _lazyQuestionItemService;
        private readonly Lazy<IQuestionCategoryService> _lazyQuestionCategoryService;
        private readonly Lazy<IQuestionAnswerService> _lazyQuestionAnswerService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _lazyQuestionItemService = new Lazy<IQuestionItemService>(()          => new QuestionItemService(repositoryManager, mapper));
            _lazyQuestionCategoryService = new Lazy<IQuestionCategoryService>(()  => new QuestionCategoryService(repositoryManager, mapper));
            _lazyQuestionAnswerService = new Lazy<IQuestionAnswerService>(()      => new QuestionAnswerService(repositoryManager, mapper));
        }

        public IQuestionCategoryService QuestionCategoryService => _lazyQuestionCategoryService.Value;
        public IQuestionItemService QuestionItemService => _lazyQuestionItemService.Value;
        public IQuestionAnswerService QuestionAnswerService => _lazyQuestionAnswerService.Value;
    }
}