using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MediatR;

using Report.API.Application.Features.Queries;
using Report.API.Application.Features.Commands.SetQuestionUnit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Report.API.Application.Features.Commands.Identified;
using Report.API.Application.Features.Commands.OpenReview;
using Report.API.Application.Features.Commands.CloseReview;
using Report.API.Application.Paggination;
using System.Linq;


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

        // GET api/report/items
        // Get all reports
        [Route("items")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]

        public async Task<ActionResult> GetAllAsync(int page, int limit,  string user, int exam, DateTime? date, int middleVal = 10, int cntBetween = 5)
        {
            try
            {
                var reports = (await _reviewQueries.GetAll()).OrderByDescending(x => x.Id);    

                if(!string.IsNullOrEmpty(user))
                {
                    reports = reports.Where(x => x.ApplicantId == user).OrderByDescending(x => x.Id);
                }

                if(exam>0)
                {
                    reports = reports.Where(x => x.ExamId == exam).OrderByDescending(x => x.Id); ;
                }

                if(date != null)
                {
                    reports = reports.Where(x => x.ReportDate.ToShortDateString() == Convert.ToDateTime(date).ToShortDateString()).OrderByDescending(x => x.Id);
                }

                if (middleVal <= cntBetween) return BadRequest(new { Error = "MiddleVal must be more than cntBetween" });

                return Ok(Pagination<Review>.GetData(currentPage: page, limit: limit, itemsData: reports, middleVal: middleVal, cntBetween:cntBetween));
                //return Ok(reports);
            }
            catch
            {
                return NotFound();
            }
        }


        // GET api/report/items/1
        // Get all reports by exam Id
        [Route("{reportId:int}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager,Student")]

        public async Task<ActionResult> GetReportsByIdAsync(int reportId)
        {
            try
            {
                var reports = await _reviewQueries.GetReportsById(reportId);
                return Ok(reports);
            }
            catch
            {
                return NotFound();
            }
        }


        // GET api/report/items/1
        // Get all reports by exam Id
        [Route("items/exams/{examId:int}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager,Student")]

        public async Task<ActionResult> GetReportsByExamIdAsync(int examId, int page, int limit, DateTime? date)
        {
            try
            {
                var reports = (await _reviewQueries.GetReportsByExamIdAsync(examId)).OrderByDescending(x=>x.Id);

                if (date != null)
                {
                    reports = reports.Where(x => x.ReportDate.ToShortDateString() == Convert.ToDateTime(date).ToShortDateString()).OrderByDescending(x => x.Id);
                }


                return Ok(Pagination<Review>.GetData(reports, page, limit));
                //return Ok(reports);
            }
            catch
            {
                return NotFound();
            }
        }


        // GET api/report/items/applicants/a1875c21-b82e-4e87-962b-9777c351f989
        // Get all reports by user Id 
        [Route("items/applicants/{userId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager,Student")]
        public async Task<ActionResult> GetReportsByApplicantIdAsync(string userId)
        {
            try
            {
                var reports = await _reviewQueries.GetReportByUserIdAsync(userId);
                return Ok(reports);
            }
            catch
            {
                return NotFound();
            }
        }


        // GET api/report/items/1/applicants/3
        // Get all reports by exam and user Id
        [Route("items/exam/{examId:int}/applicant/{appId}")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager,Student")]
        public async Task<ActionResult> GetReportsByExamIdAndUserIdAsync(int examId, string appId)
        {
            try
            {
                var report = await _reviewQueries.GetReportByExamIdAndUserIdAsync(examId, appId);
                return Ok(report);
            }
            catch
            {
                return NotFound();
            }
        }


        // POST api/report/items
        // Create new report
        [Route("openreport")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
        public async Task<IActionResult> OpenReportAsync([FromBody] OpenReviewCommand command, CancellationToken cancellationToken)
        {
            var reportId = await _mediator.Send(command, cancellationToken);

            Console.WriteLine("--> Open repotr...");
            return Ok(reportId);
        }


        // POST api/report/items
        // add new applicant answer(update)
        [Route("currentanswer")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
        public async Task<IActionResult> SetAnswerInReportAsync([FromBody] SetQuestionUnitCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            Console.WriteLine("--> Adding current answer...");
            return Ok();
        }


        //PUT api/report/action
        //Generate review(In the exam end!!)
        [Route("closereport")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]
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

    }
}