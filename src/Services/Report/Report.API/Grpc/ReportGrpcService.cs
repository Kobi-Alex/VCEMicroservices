using Grpc.Core;
using GrpcReport;
using Microsoft.Extensions.Logging;
using Report.API.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using MediatR;
using Grpc.Core;
using GrpcReport;

using Report.API.Application.Features.Queries;
using Report.API.Application.Features.Commands.RemoveReview;


namespace Report.API.Grpc
{
    public class ReportGrpcService : ReportGrpc.ReportGrpcBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<QuestionGrpcService> _logger;
        private readonly IReviewQueries _reviewQueries;
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<ReportGrpcService> _logger;

        public ReportGrpcService(IMediator mediator, ILogger<QuestionGrpcService> logger, IReviewQueries reviewQueries)
        public ReportGrpcService(IServiceManager serviceManager, ILogger<ReportGrpcService> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _serviceManager = serviceManager ?? throw new ArgumentNullException(nameof(serviceManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reviewQueries = reviewQueries ?? throw new ArgumentNullException(nameof(reviewQueries));
        }

        public override async Task<UserDataResponse> RemoveUserDataFromReport(RemoveUserData request, ServerCallContext context)
        public override async Task<ReportResponse> CheckIfExistsExamInReports(ReportRequest request, ServerCallContext context)
        {
            try
            {
                var reviews = await _reviewQueries.GetReportByUserIdAsync(request.UserId);
            var res = await _serviceManager.ReportService.GetAllReviewByExamId(request.ExamId);

                if (reviews is null)
            if (res != null && res.Count() > 0)
                {
                    return new UserDataResponse
                    {
                        Success = false,
                        Error = "User not found!!"
                    };
              return  await Task.FromResult(new ReportResponse { Exists = true });
                }

                foreach (var item in reviews)
                {
                    var command = new RemoveReviewCommand(item.Id);
                    await _mediator.Send(command);
                }

                return new UserDataResponse
                {
                    Success = true,
                    Error = ""
                };

            }
            catch (Exception ex)
            {
                return new UserDataResponse
                {
                    Success = false,
                    Error = $"{ ex.Message }"
                };
            }

        }

        public override async Task<IsExistExamResponse> IsExistExamFromReport(IsExistExamRequest request, ServerCallContext context)
        {
            try
            {
                var review = await _reviewQueries.GetReportByExamIdAndUserIdAsync(request.ExamId, request.UserId);

                return new IsExistExamResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new IsExistExamResponse
                {
                    Success = false,
                    Error = ex.Message
                };
            }

            return await Task.FromResult(new ReportResponse { Exists = false });
        }
    }
}
