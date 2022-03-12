using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Services.Interfaces;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;


namespace Question.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public QuestionsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        // GET api/Questions
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions(CancellationToken cancellationToken)
        {
            var questions = await _serviceManager.QuestionItemService
                .GetAllAsync(cancellationToken);

            Console.WriteLine("--> Getting all questions...");
            return Ok(questions);
        }

        // GET api/Questions/1
        [HttpGet("{questionId:int}", Name = "GetQuestionById")]
        public async Task<IActionResult> GetQuestionById(int questionId, CancellationToken cancellationToken)
        {
            var question = await _serviceManager.QuestionItemService
                .GetByIdAsync(questionId, cancellationToken);

            Console.WriteLine("--> Getting question by ID...");
            return Ok(question);
        }

        // POST api/Questions
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionItemCreateDto questionItemCreateDto, CancellationToken cancellationToken)
        {
            var questionDto = await _serviceManager.QuestionItemService
                .CreateAsync(questionItemCreateDto, cancellationToken);

            Console.WriteLine("--> Creating new question...");
            return CreatedAtAction(nameof(GetQuestionById), new { questionId = questionDto.Id }, questionDto);
        }

        // PUT api/Questions/1
        [HttpPut("{questionId:int}", Name = "UpdateQuestion")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] QuestionItemUpdateDto questionItemUpdateDto, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionItemService
                .UpdateAsync(questionId, questionItemUpdateDto, cancellationToken);

            Console.WriteLine($"--> Updating question by ID = {questionId}");
            return NoContent();
        }

        // DELETE api/Questions/5
        [HttpDelete("{questionId:int}", Name = "DeleteQuestion")]
        public async Task<IActionResult> DeleteQuestion(int questionId, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionItemService.DeleteAsync(questionId, cancellationToken);

            Console.WriteLine($"--> Question with ID = {questionId} has been removed!!");
            return NoContent();
        }

    }
}
