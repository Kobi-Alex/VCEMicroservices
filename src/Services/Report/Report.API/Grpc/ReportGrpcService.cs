using Grpc.Core;
using GrpcReport;
using MediatR;
using Microsoft.Extensions.Logging;
using Report.API.Application.Features.Commands.RemoveReview;
using Report.API.Application.Features.Queries;
using Report.API.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Report.API.Grpc
{

    public class ReportGrpcService : ReportGrpc.ReportGrpcBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<ReportGrpcService> _loggerReport;
        private readonly ILogger<QuestionGrpcService> _loggerQuestion;
        private readonly IMediator _mediator;
        private readonly IReviewQueries _reviewQueries;

        public ReportGrpcService(IMediator mediator, IReviewQueries reviewQueries, IServiceManager serviceManager, ILogger<ReportGrpcService> loggerReport, ILogger<QuestionGrpcService> loggerQuestion)
        {
            _serviceManager = serviceManager ?? throw new ArgumentNullException(nameof(serviceManager));
            _loggerReport = loggerReport ?? throw new ArgumentNullException(nameof(loggerReport));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _loggerQuestion = loggerQuestion ?? throw new ArgumentNullException(nameof(loggerQuestion));
            _reviewQueries = reviewQueries ?? throw new ArgumentNullException(nameof(reviewQueries));
        }

        public override async Task<ReportResponse> CheckIfExistsExamInReports(ReportRequest request, ServerCallContext context)
        {
            var res = await _serviceManager.ReportService.GetAllReviewByExamId(request.ExamId);

            if (res != null && res.Count() > 0)
            {
                return await Task.FromResult(new ReportResponse { Exists = true });
            }

            return await Task.FromResult(new ReportResponse { Exists = false });
        }

        public override async Task<UserDataResponse> RemoveUserDataFromReport(RemoveUserData request, ServerCallContext context)
        {
            try
            {
                var reviews = await _reviewQueries.GetReportByUserIdAsync(request.UserId);

                if (reviews is null)
                {
                    return new UserDataResponse
                    {
                        Success = false,
                        Error = "User not found!!"
                    };
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

        }
    }
}