using System;
using System.Threading.Tasks;

using Grpc.Core;
using GrpcApplicant;
using AutoMapper;

using Microsoft.Extensions.Logging;
using Applicant.API.Application.Services.Interfaces;
using Applicant.API.Application.Contracts.Dtos.UserDtos;
using System.Linq;

namespace Applicant.API.Grpc
{
    public class ApplicantGrpcService : ApplicantGrpc.ApplicantGrpcBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<ApplicantGrpcService> _logger;
        private readonly IMapper _mapper;

        public ApplicantGrpcService(IServiceManager serviceManager, ILogger<ApplicantGrpcService> logger, IMapper mapper)
        {
            _serviceManager = serviceManager ?? throw new ArgumentNullException(nameof(serviceManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<RemoveExamResponce> RemoveExamFromApplicantData(RemoveExamRequest request, ServerCallContext context)
        {
            try
            {
                var userExamDto = _mapper.Map<UserExamDto>(request);
                await _serviceManager.UserService.RemoveExamFromUser(userExamDto);

                return new RemoveExamResponce
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new RemoveExamResponce
                {
                    Success = false,
                    Error = ex.Message
                };
            }

        }

        public override async Task<GetUserDataResponse> GetUseData(GetUserDataRequest request, ServerCallContext context)
        {
            GetUserDataResponse response = null;

            var user = await _serviceManager.UserService.GetByIdAsync(request.UserId);

            if(user!=null)
            {
                response =  _mapper.Map(user,response );

                return response;
            }

            return response;
        }

        public override async Task<UserExamResponse> CheckIfExamExistsInUsers(UserExamRequest request, ServerCallContext context)
        {
            var userExams = await _serviceManager.UserService.GetExamUsersAsync(request.ExamId);

            UserExamResponse response = new UserExamResponse() { Exists = false };

            if(userExams != null && userExams.Count()>0)
            {
                response.Exists = true;
                return response;
            }

            return response;
        }
    }
}