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

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnswersController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AnswersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        // GET api/Answers
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles="Teacher, Student")]
        public async Task<IActionResult> GetAllAnswers(int? questionId, CancellationToken cancellationToken)
        {
            var answers = await _serviceManager.QuestionAnswerService.GetAllAsync(cancellationToken);

            if(questionId != null)
            {
                answers = answers.Where(x => x.QuestionItemId == questionId);
            }

            Console.WriteLine("---> Count: " + answers.ToList().Count().ToString());

            Console.WriteLine("--> Getting answers...");
            return Ok(answers);
        }


        //// GET api/Answers/1
        [HttpGet]
        [Route("q/{questionId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher, Student")]

        public async Task<IActionResult> GetAllAnswersByQuestionId(int questionId, CancellationToken cancellationToken)
        {
            var answer = await _serviceManager.QuestionAnswerService
                .GetAllByQuestionItemIdAsync(questionId, cancellationToken);

            Console.WriteLine($"--> Getting answer by question ID = {questionId} ...");
            return Ok(answer);
        }


        // GET api/Answers/1
        [HttpGet("{answerId:int}", Name = "GetAnswerById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher, Student")]
        public async Task<IActionResult> GetAnswerById(int answerId, CancellationToken cancellationToken)
        {
            var answer = await _serviceManager.QuestionAnswerService.GetByIdAsync(answerId, cancellationToken);

            Console.WriteLine($"--> Getting answer by ID = {answerId} ...");
            return Ok(answer);
        }


        // POST api/Answers
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]

        public async Task<IActionResult> CreateAnswer([FromBody] QuestionAnswerCreateDto questionAnswerCreateDto, CancellationToken cancellationToken)
        {
            var answerDto = await _serviceManager.QuestionAnswerService.CreateAsync(questionAnswerCreateDto, cancellationToken);

            Console.WriteLine("--> Add new answer...");
            return CreatedAtAction(nameof(GetAnswerById), new { answerId = answerDto.Id }, answerDto);
        }


        // PUT api/Answers/1
        [HttpPut("{answerId:int}", Name = "UpdateAnswer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]

        public async Task<IActionResult> UpdateAnswer(int answerId, [FromBody] QuestionAnswerUpdateDto questionItemUpdateDto, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionAnswerService.UpdateAsync(answerId, questionItemUpdateDto, cancellationToken);

            Console.WriteLine($"--> Updating answer by ID = {answerId}");
            return NoContent();
        }

        // DELETE api/Answers/1
        [HttpDelete("{answerId:int}", Name = "DeleteAnswer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]

        public async Task<IActionResult> DeleteAnswer(int answerId, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionAnswerService.DeleteAsync(answerId, cancellationToken);

            Console.WriteLine($"--> Delete answer by ID = {answerId}");
            return NoContent();
        }

    }
}