using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Report.API.Application.Features.Queries;
using MediatR;
using Report.API.Application.Features.Commands.CancelReview;
using Report.API.Application.Features.Commands.SetQuestionUnit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Report.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReportController> _logger;
        private readonly IReviewQueries _reviewQueries;

        public ReportController(IMediator mediator, IReviewQueries reviewQueries, ILogger<ReportController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _reviewQueries = reviewQueries ?? throw new ArgumentNullException(nameof(reviewQueries));
        }

        // GET api/report/items/1
        [Route("items/{examId:int}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager,Student")]

        public async Task<ActionResult> GetReportsByExamIdAsync(int examId)
        {
            try
            {
                var reports = await _reviewQueries.GetReportsByExamIdAsync(examId);
                return Ok(reports);
            }
            catch
            {
                return NotFound();
            }
        }

        // GET api/report/items/1/applicants/3
        [Route("items{examId:int}/applicants/{userId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager,Student")]
        public async Task<ActionResult> GetReportsByExamIdAndUserIdAsync(int examId, string userId)
        {
            try
            {
                var reports = await _reviewQueries.GetReportsByExamIdAndUserIdAsync(examId, userId);
                return Ok(reports);
            }
            catch
            {
                return NotFound();
            }
        }


        // POST api/report/items
        // Create and add new applicant answer or update
        [Route("items")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
        public async Task<IActionResult> CreateQuestionUnit([FromBody] SetQuestionUnitCommand setQuestionUnitCommand, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Adding current answer...");

            await _mediator.Send(setQuestionUnitCommand, cancellationToken);

            return Ok();
        }


        ////PUT api/
        //[Route("cancel")]
        //[HttpPut]
        //public async Task<IActionResult> CancelReviewAsync([FromBody] CancelReviewCommand cancelReviewCommand, CancellationToken cancellationToken)
        //{
        //    bool commandResult = false;

        //    //if ((cancelReviewCommand.UserId, out Guid guid) && guid != Guid.Empty))
        //    //{

        //    //}

        //    return Ok();
        //}


    }
}
