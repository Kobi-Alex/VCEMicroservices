using MediatR;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Queries;
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
        private readonly IMediator _mediator;

        public QuestionsController(IServiceManager serviceManager, IMediator mediator)
        {
            _serviceManager = serviceManager;
            _mediator = mediator;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetQuestions(CancellationToken cancellationToken)
        //{
        //    Console.WriteLine("--> Getting questions...");

        //    var questions = await _serviceManager.QuestionService.GetAllAsync(cancellationToken);

        //    return Ok(questions);

        //    //return Ok(await _mediator.Send(new GetQuestionListQuery()));
        //}

        [HttpGet]
        public async Task<IActionResult> GetQuestions(CancellationToken cancellationToken)
            => Ok(await _serviceManager.QuestionService.GetAllAsync(cancellationToken));
    }

}
