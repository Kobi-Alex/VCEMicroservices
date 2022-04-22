using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Services.Interfaces;
using Question.API.Application.Contracts.Dtos.QuestionItemDtos;
using Question.API.Application.Paggination;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher, Student")]
        public async Task<IActionResult> GetAllQuestions(int page, int limit, int category, string context, int middleVal = 10, int cntBetween = 5, CancellationToken cancellationToken = default)
        {
            var questions = await _serviceManager.QuestionItemService
                .GetAllAsync(cancellationToken);

            if (category > 0)
            {
                questions = questions.Where(x => x.QuestionCategoryId == category).ToList();
            }

            if (!String.IsNullOrEmpty(context))
            {
                questions = questions.Where(x => x.Context.ToLower().Contains(context.ToLower()));
            }

            if (middleVal <= cntBetween) return BadRequest(new { Error = "MiddleVal must be more than cntBetween" });


            Console.WriteLine("--> Getting all questions...");
            return Ok(Pagination<QuestionItemReadDto>.GetData(currentPage: page, limit: limit, itemsData: questions, middleVal: middleVal, cntBetween: cntBetween));
            //return Ok(questions);
        }

        // GET api/Questions/1
        [HttpGet("{questionId:int}", Name = "GetQuestionById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher, Student")]
        public async Task<IActionResult> GetQuestionById(int questionId, CancellationToken cancellationToken)
        {
            var question = await _serviceManager.QuestionItemService
                .GetByIdAsync(questionId, cancellationToken);

            Console.WriteLine("--> Getting question by ID...");
            return Ok(question);
        }

        // POST api/Questions
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionItemCreateDto questionItemCreateDto, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var questionDto = await _serviceManager.QuestionItemService
               .CreateAsync(questionItemCreateDto, cancellationToken);

                Console.WriteLine("--> Creating new question...");
                return CreatedAtAction(nameof(GetQuestionById), new { questionId = questionDto.Id }, questionDto);
            }

            return BadRequest(GetModelStateErrors(ModelState.Values));
        }

        // PUT api/Questions/1
        [HttpPut("{questionId:int}", Name = "UpdateQuestion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> UpdateQuestion(int questionId, [FromBody] QuestionItemUpdateDto questionItemUpdateDto, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                await _serviceManager.QuestionItemService
                .UpdateAsync(questionId, questionItemUpdateDto, cancellationToken);

                Console.WriteLine($"--> Updating question by ID = {questionId}");
                return NoContent();

            }
            return BadRequest(GetModelStateErrors(ModelState.Values));
        }

        // DELETE api/Questions/5
        [HttpDelete("{questionId:int}", Name = "DeleteQuestion")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> DeleteQuestion(int questionId, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionItemService.DeleteAsync(questionId, cancellationToken);

            Console.WriteLine($"--> Question with ID = {questionId} has been removed!!");
            return NoContent();
        }

        /// <summary>
        /// Gets all modelstate errors
        /// </summary>
        private List<string> GetModelStateErrors(IEnumerable<ModelStateEntry> modelState)
        {
            var modelErrors = new List<string>();
            foreach (var ms in modelState)
            {
                foreach (var modelError in ms.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }

            return modelErrors;
        }
    }
}
