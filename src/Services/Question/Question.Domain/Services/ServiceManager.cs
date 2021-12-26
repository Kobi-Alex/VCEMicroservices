using AutoMapper;
using Question.Domain.Domain.Repositories;
using Question.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Domain.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IQuestionService> _lazyQuestionService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _lazyQuestionService = new Lazy<IQuestionService>(() => new QuestionService(repositoryManager, mapper));
        }
        public IQuestionService QuestionService => _lazyQuestionService.Value;
    }
}
