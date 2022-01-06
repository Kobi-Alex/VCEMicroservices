using MediatR;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Queries;
using Question.API.Application.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Question.API.Controllers
{

    [ApiController]
    [Route("api/categories/{categoryId:int}/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IMediator _mediator;

        public QuestionsController(IServiceManager serviceManager, IMediator mediator)
        {
            _serviceManager = serviceManager;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(int categoryId, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting questions...");

            var questions = await _serviceManager.QuestionItemService
                .GetAllByQuestionCategoryIdAsync(categoryId, cancellationToken);

            return Ok(questions);

            //return Ok(await _mediator.Send(new GetQuestionListQuery()));
        }

        /*      [HttpGet]
              public async Task<IActionResult> GetQuestions(CancellationToken cancellationToken)
                  => Ok(await _serviceManager.QuestionService.GetAllAsync(cancellationToken));*/
    }

}
