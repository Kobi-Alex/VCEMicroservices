using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Exam.API.Application.Services.Interfaces;
using Exam.API.Application.Contracts.ExamItemDtos;
using Exam.API.Application.Contracts.ExamQuestionDtos;
using Exam.API.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Paggination;



namespace Exam.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ExamController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ExamController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        // GET api/exam/items
        [Route("items")]
        [HttpGet]
        [Route("items")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher, Manager, Student")]
        public async Task<IActionResult> Exams(int page, string title, string status, int limit ,CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting exams...");
            var exams = await _serviceManager.ExamItemService.GetAllAsync(cancellationToken);

            if(title != null)
            {
                exams = exams.Where(x => x.Title.ToLower().Contains(title.ToLower()));
            }

            if(status != null)
            {
                exams = exams.Where(x => x.Status.ToString().ToLower() == status.ToLower());
            }


            return Ok(Pagination<ExamItemReadDto>.GetData(page,limit,exams));
            //return Ok(exams);
        }


        // GET api/exam/items/1
        [Route("items/{examId:int}")]
        [HttpGet]
        [Route("items/{examId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher, Manager, Student")]
        public async Task<IActionResult> ExamById(int examId, CancellationToken cancellationToken)
        {
            Console.WriteLine($"--> Getting exam by Id = {examId}");
            var examDto = await _serviceManager.ExamItemService.GetByIdAsync(examId, cancellationToken);

            return Ok(examDto);
        }


        // POST api/exam/items
        [Route("items")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> CreateExam([FromBody] ExamItemCreateDto examCreateDto, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Creating exam...");
            var examDto = await _serviceManager.ExamItemService.CreateAsync(examCreateDto, cancellationToken);

            return CreatedAtAction(nameof(ExamById), new { examId = examDto.Id }, examDto);
        }
        // POST api/exam/items
        [Route("items/{examId:int}")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> UpdateExam(int examId, [FromBody] ExamItemUpdateDto examUpdateDto, CancellationToken cancellationToken)
        {

            Console.WriteLine($"--> Updating exam {examId}: ...");

            await _serviceManager.ExamItemService.UpdateAsync(examId, examUpdateDto, cancellationToken);

            return NoContent();
        }


        // PUT api/exam/items/1
        [Route("items/{examId:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateExam(int examId, [FromBody] ExamItemUpdateDto examItemUpdateDto, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Update exam...");
            await _serviceManager.ExamItemService.UpdateAsync(examId, examItemUpdateDto, cancellationToken);

            return NoContent();
        }


        // GET api/exam/items/1
        [Route("items/{examId:int}")]
        [HttpDelete]
        [Route("items/{examId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> DeleteExam(int examId, CancellationToken cancellationToken)
        {
            Console.WriteLine($"--> Delete Exam...");
            await _serviceManager.ExamItemService.DeleteAsync(examId, cancellationToken);

            return NoContent();
        }


        // GET api/[controller]/items/5/questions
        [Route("items/{examId:int}/questions")]
        public async Task<IActionResult> QuestionsByExamItemId(int examId, int page, int limit, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting questions...");
            var questions = await _serviceManager.ExamQuestionService.GetAllByExamItemIdAsync(examId, cancellationToken);

            return Ok(Pagination<ExamQuestionReadDto>.GetData(page, limit, questions));
            //return Ok(questions);
        }


        // GET api/[controller]/items/5/question/1
        [Route("items/{examId:int}/questions/{questionId:int}")]
        [HttpGet]
        public async Task<IActionResult> QuestionById(int examId, int questionId, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting question by Id...");
            var question = await _serviceManager.ExamQuestionService.GetByIdAsync(examId, questionId, cancellationToken);

            return Ok(question);
        }


        // POST api/[controller]/items/5/questions
        [Route("items/{examId:int}/questions")]
        [HttpPost]
        [Route("items/{examId:int}/questions")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> CreateQuestionAsync(int examId, [FromBody] ExamQuestionCreateDto questionCreateDto, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Creating question...");

            var questionDto = await _serviceManager.ExamQuestionService.CreateAsync(examId, questionCreateDto, cancellationToken);

            return CreatedAtAction(nameof(QuestionById), new { examId = questionDto.ExamItemId, questionId = questionDto.Id }, questionDto);
        }


        // GET api/[controller]/items/5/question/1
        [Route("items/{examId:int}/questions/{questionId:int}")]
        [HttpDelete]
        [Route("items/{examId:int}/questions/{questionId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> DeleteQuestion(int examId, int questionId, CancellationToken cancellationToken)
        {
            Console.WriteLine($"--> Delete question by Id = {questionId}");

            await _serviceManager.ExamQuestionService.DeleteAsync(examId, questionId, cancellationToken);

            return NoContent();
        }
    }
}