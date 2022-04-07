using System;
using AutoMapper;
using Microsoft.Extensions.Options;

using Applicant.API.Grpc;
using Applicant.Domain.Repositories;
using Applicant.API.Application.Services.Interfaces;
using Applicant.API.Application.Configurations;


namespace Applicant.API.Application.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccessCodeService> _lazyAccessCodeService;
        private readonly Lazy<IUserService> _lazyUserService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, EmailConfiguration emailConfig, 
            IOptionsMonitor<JwtConfig> optionsMonitor, ReportGrpcService reportGrpcService)
        {
            _lazyAccessCodeService = new Lazy<IAccessCodeService>(() => new AccessCodeService(repositoryManager, mapper, optionsMonitor, emailConfig));
            _lazyUserService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper, emailConfig, reportGrpcService));
        }

        public IAccessCodeService AccessCodeService => _lazyAccessCodeService.Value;
        public IUserService UserService => _lazyUserService.Value;
    }

}