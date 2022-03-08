using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;
using Question.API.Application.Paggination;
using Question.API.Application.Services.Interfaces;

namespace Question.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class QuestionsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public QuestionsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET api/Questions
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions(int page, int limit, int? category, string context,CancellationToken cancellationToken)
        {
            var questions = await _serviceManager.QuestionItemService
                .GetAllAsync(cancellationToken);

            if(category != null)
            {
                questions = questions.Where(x => x.QuestionCategoryId == category).ToList();
            }

            if(!String.IsNullOrEmpty(context))
            {
                questions = questions.Where(x => x.Context.ToLower().Contains(context.ToLower()));
            }

            Console.WriteLine("--> Getting all questions...");
            return Ok(Pagination<QuestionItemReadDto>.GetData(page, limit, questions));
            //return Ok(questions);
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
