using System;
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

        public ReportGrpcService(IMediator mediator, ILogger<QuestionGrpcService> logger, IReviewQueries reviewQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reviewQueries = reviewQueries ?? throw new ArgumentNullException(nameof(reviewQueries));
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
                    //await _mediator.Send(command);
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
    }
}
