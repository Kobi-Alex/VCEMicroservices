using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Report.API.Application.Features.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Report.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {

        private readonly ILogger<ReportController> _logger;
        private readonly IReviewQueries _reviewQueries;

        public ReportController(IReviewQueries reviewQueries, ILogger<ReportController> logger)
        {
            _reviewQueries = reviewQueries;
            _logger = logger;
        }


        [Route("{examId:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(Review), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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



        [Route("{examId:int}/applicants{userId}")]
        [HttpGet]
        [ProducesResponseType(typeof(Review), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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


    }
}
