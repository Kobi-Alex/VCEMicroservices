using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;
using Question.API.Application.Services.Interfaces;

namespace Question.API.Controllers
{

    [Route("api/categories/{categoryId:int}/[controller]")]
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
        //[HttpGet]
        //public async Task<IActionResult> GetAllQuestions(CancellationToken cancellationToken)
        //{
        //    var questions = await _serviceManager.QuestionItemService
        //        .GetAllQuestionAsync(cancellationToken);

        //    Console.WriteLine("--> Getting all questions with out Id...");
        //    return Ok(questions);
        //}


        // GET api/Categories/5/Questions
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> GetQuestions(int categoryId, CancellationToken cancellationToken)
        {
            var questions = await _serviceManager.QuestionItemService
                .GetAllByQuestionCategoryIdAsync(categoryId, cancellationToken);

            Console.WriteLine("--> Getting questions...");
            return Ok(questions);

            //return Ok(await _mediator.Send(new GetQuestionListQuery()));
        }

        /*[HttpGet]
         * public async Task<IActionResult> GetQuestions(CancellationToken cancellationToken)
         *      => Ok(await _serviceManager.QuestionService.GetAllAsync(cancellationToken));*/


        // GET api/Categories/5/Questions/1
        [HttpGet("{questionId:int}", Name = "GetQuestionById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> GetQuestionById(int categoryId, int questionId, CancellationToken cancellationToken)
        {
            var question = await _serviceManager.QuestionItemService.GetByIdAsync(categoryId, questionId, cancellationToken);

            Console.WriteLine("--> Getting question by ID...");
            return Ok(question);
        }


        // POST api/Categories/5/Questions
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> CreateQuestion(int categoryId, [FromBody] QuestionItemCreateDto questionItemCreateDto, CancellationToken cancellationToken)
        {
            var questionDto = await _serviceManager.QuestionItemService.CreateAsync(categoryId, questionItemCreateDto, cancellationToken);

            Console.WriteLine("--> Creating new question...");
            return CreatedAtAction(nameof(GetQuestionById), new { categoryId = questionDto.QuestionCategoryId, questionId = questionDto.Id }, questionDto);
        }


        // PUT api/Categories/5/Questions/1
        [HttpPut("{questionId:int}", Name = "UpdateQuestion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> UpdateQuestion(int categoryId, int questionId, [FromBody] QuestionItemUpdateDto questionItemUpdateDto, CancellationToken cancellationToken)
        {

            await _serviceManager.QuestionItemService.UpdateAsync(categoryId, questionId, questionItemUpdateDto, cancellationToken);

            Console.WriteLine($"--> Updating question by ID = {questionId}");
            return NoContent();
        }


        // DELETE api/Categories/5/Questions/1
        [HttpDelete("{questionId:int}", Name = "DeleteQuestion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> DeleteQuestion(int categoryId, int questionId, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionItemService.DeleteAsync(categoryId, questionId, cancellationToken);

            Console.WriteLine($"--> Question with ID = {questionId} has been removed!!");
            return NoContent();
        }

    }
}
