using System;
using AutoMapper;
using Microsoft.Extensions.Options;

using Applicant.API.Grpc;
using Applicant.Domain.Repositories;
using Applicant.API.Application.Services.Interfaces;
using Applicant.API.Application.Configurations;
using Microsoft.Extensions.Configuration;
using Applicant.API.Grpc.Interfaces;
using Applicant.API.Application.Contracts.Infrastructure;

namespace Applicant.API.Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccessCodeService> _lazyAccessCodeService;
        private readonly Lazy<IUserService> _lazyUserService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, 
            IOptionsMonitor<JwtConfig> optionsMonitor, IReportGrpcService reportGrpcService, IExamGrpcService examGrpcService, IEmailService emailService)
        {
            _lazyAccessCodeService = new Lazy<IAccessCodeService>(() => new AccessCodeService(repositoryManager, mapper, optionsMonitor,emailService));
            _lazyUserService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper, reportGrpcService, examGrpcService, emailService));
        }

        public IAccessCodeService AccessCodeService => _lazyAccessCodeService.Value;
        public IUserService UserService => _lazyUserService.Value;
    }

}