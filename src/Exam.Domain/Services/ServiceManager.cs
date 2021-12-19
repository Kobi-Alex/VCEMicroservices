using Exam.Domain.Domain.Repositories;
using Exam.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Domain.Services
{
    // Our service instances are only going to be created when we access them for the first time,
    // and not before that.
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IExamService> _lazyExamService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyExamService = new Lazy<IExamService>(() => new ExamService(repositoryManager));
        }

        public IExamService ExamService => _lazyExamService.Value;
    }
}
