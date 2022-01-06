
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Question.API.Controllers
{

    [ApiController]
    [Route("api/categories/{categoryId:int}/questions/{questionId:int}/[controller]")]
    public class AnswersController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AnswersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnswers(int questionId, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting answers...");

            var answers = await _serviceManager.QuestionAnswerService
                .GetAllByQuestionItemIdAsync(questionId, cancellationToken);

            return Ok(answers);

        }

    }

}