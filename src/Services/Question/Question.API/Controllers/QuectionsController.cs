using Microsoft.AspNetCore.Mvc;
using Question.Domain.Services.Abstractions;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Question.API.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public QuestionsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetExams(CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting questions...");
            var questions = await _serviceManager.QuestionService.GetAllAsync(cancellationToken);

            return Ok(questions);
        }
    }

}
