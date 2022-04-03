using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MediatR;

using Report.API.Application.Features.Queries;
using Report.API.Application.Features.Commands.Identified;
using Report.API.Application.Features.Commands.OpenReview;
using Report.API.Application.Features.Commands.CloseReview;
using Report.API.Application.Features.Commands.RemoveReview;
using Report.API.Application.Features.Commands.SetQuestionUnit;

namespace Report.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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



        // GET api/Report/items
        // Get all reports
        [Route("items")]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var reports = await _reviewQueries.GetAll();
            return Ok(reports);

        }


        // GET api/Report/items/1
        // Get all reports by exam Id
        [Route("{reportId:int}")]
        [HttpGet]
        public async Task<ActionResult> GetReportsByIdAsync(int reportId)
        {
            var reports = await _reviewQueries.GetReportsById(reportId);
            return Ok(reports);

        }


        // GET api/Report/items/1
        // Get all reports by exam Id
        [Route("items/exams/{examId:int}")]
        [HttpGet]
        public async Task<ActionResult> GetReportsByExamIdAsync(int examId)
        {
            var reports = await _reviewQueries.GetReportsByExamIdAsync(examId);
            return Ok(reports);

        }


        // GET api/Report/items/applicants/a1875c21-b82e-4e87-962b-9777c351f989
        // Get all reports by user Id 
        [Route("items/applicants/{userId}")]
        [HttpGet]
        public async Task<ActionResult> GetReportsByApplicantIdAsync(string userId)
        {
            var reports = await _reviewQueries.GetReportByUserIdAsync(userId);
            return Ok(reports);

        }


        // GET api/Report/items/1/applicants/3
        // Get all reports by exam and user Id
        [Route("items/exam/{examId:int}/applicant/{appId}")]
        [HttpGet]
        public async Task<ActionResult> GetReportByExamIdAndUserIdAsync(int examId, string appId)
        {
            var report = await _reviewQueries.GetReportByExamIdAndUserIdAsync(examId, appId);
            return Ok(report);

        }


        // POST api/Report/items
        // Create new report
        [Route("openreport")]
        [HttpPost]
        public async Task<IActionResult> OpenReportAsync([FromBody] OpenReviewCommand command, CancellationToken cancellationToken)
        {
            var reportId = await _mediator.Send(command, cancellationToken);

            Console.WriteLine("--> Open repotr...");
            return Ok(reportId);

        }


        // POST api/Report/items
        // add new applicant answer(update)
        [Route("currentanswer")]
        [HttpPost]
        public async Task<IActionResult> SetAnswerInReportAsync([FromBody] SetQuestionUnitCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            Console.WriteLine("--> Adding current answer...");
            return Ok();

        }


        //PUT api/Report/action
        //Generate review(In the exam end!!)
        [Route("closereport")]
        [HttpPut]
        public async Task<IActionResult> CloseReportAsync([FromBody] CloseReviewCommand command, CancellationToken cancellationToken)
        {
            bool commandResult = false;
            // Get reportId by userId and last exam date 

            if (!commandResult)
            {
                var requestActionReview = new IdentifiedCommand<CloseReviewCommand, bool>(command, command.ReviewId);

                _logger.LogInformation(
               "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
               requestActionReview.GetType(),
               nameof(requestActionReview.Command.ReviewId),
               requestActionReview.Command.ReviewId,
               requestActionReview);

                commandResult = await _mediator.Send(requestActionReview, cancellationToken);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();

        }


        // FOR TEST
        // DELETE api/Report/items/remove
        [Route("items/remove")]
        [HttpDelete]
        public async Task<IActionResult> DeleteReviewAsync([FromBody] RemoveReviewCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            Console.WriteLine($"--> Delete review by ID = {command.ReviewId}");
            return NoContent();
        }

    }
}