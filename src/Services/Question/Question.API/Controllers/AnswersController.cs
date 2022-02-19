using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Contracts.Dtos.QuestionAnswerDtos;
using Question.API.Application.Services.Interfaces;

namespace Question.API.Controllers
{

    [Route("api/Categories/{categoryId:int}/Questions/{questionId:int}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnswersController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AnswersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        // GET api/Categories/5/Questions/1/Answers
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> GetAnswers(int categoryId, int questionId, CancellationToken cancellationToken)
        {
            var answers = await _serviceManager.QuestionAnswerService
                .GetAllByQuestionItemIdAsync(categoryId, questionId, cancellationToken);

            Console.WriteLine("---> Count: " + answers.ToList().Count().ToString());

            Console.WriteLine("--> Getting answers...");
            return Ok(answers);
        }


        // GET api/Categories/5/Questions/1/Answers/1
        [HttpGet("{answerId:int}", Name = "GetAnswerById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> GetAnswerById(int categoryId, int questionId, int answerId, CancellationToken cancellationToken)
        {
            var answer = await _serviceManager.QuestionAnswerService.GetByIdAsync(categoryId, questionId, answerId, cancellationToken);

            Console.WriteLine($"--> Getting answer by ID = {answerId} ...");
            return Ok(answer);
        }


        // POST api/Categories/5/Questions/1/Answers
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> CreateAnswer(int categoryId, int questionId, [FromBody] QuestionAnswerCreateDto questionAnswerCreateDto, CancellationToken cancellationToken)
        {
            var answerDto = await _serviceManager.QuestionAnswerService.CreateAsync(categoryId, questionId, questionAnswerCreateDto, cancellationToken);

            Console.WriteLine("--> Add new answer...");
            return CreatedAtAction(nameof(GetAnswerById), new {categoryId = categoryId, questionId = answerDto.QuestionItemId, answerId = answerDto.Id}, answerDto);
        }


        // PUT api/Categories/5/Questions/5/Answer/1
        [HttpPut("{answerId:int}", Name = "UpdateAnswer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> UpdateAnswer(int categoryId, int questionId, int answerId, [FromBody] QuestionAnswerUpdateDto questionItemUpdateDto, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionAnswerService.UpdateAsync(categoryId, questionId, answerId, questionItemUpdateDto, cancellationToken);

            Console.WriteLine($"--> Updating answer by ID = {answerId}");
            return NoContent();
        }


        // DELETE api/Categories/5/Questions/5/Answer/1
        [HttpDelete("{answerId:int}", Name = "DeleteAnswer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> DeleteAnswer (int categoryId, int questionId, int answerId, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionAnswerService.DeleteAsync(categoryId, questionId, answerId, cancellationToken);

            Console.WriteLine($"--> Delete answer by ID = {answerId}");
            return NoContent();
        }

    }
}