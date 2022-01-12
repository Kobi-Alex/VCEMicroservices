using System;
using System.Threading;
using System.Threading.Tasks;
using Exam.API.Application.Contracts.ExamItemDtos;
using Exam.API.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Exam.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ExamController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET api/exam/items
        [HttpGet]
        [Route("items")]
        public async Task<IActionResult> GetExams(CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting exams...");
            var exams = await _serviceManager.ExamItemService.GetAllAsync(cancellationToken);

            return Ok(exams);
        }

        // GET api/exam/items/1
        [HttpGet]
        [Route("items/{examId:int}")]
        public async Task<IActionResult> GetExamById(int examId, CancellationToken cancellationToken)
        {
            Console.WriteLine($"--> Getting exam by Id = {examId}");
            var examDto = await _serviceManager.ExamItemService.GetByIdAsync(examId, cancellationToken);

            return Ok(examDto);
        }

        // POST api/exam/items
        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateExam([FromBody] ExamItemCreateDto examCreateDto)
        {
            Console.WriteLine("--> Creating exam...");
            var examDto = await _serviceManager.ExamItemService.CreateAsync(examCreateDto);

            return CreatedAtAction(nameof(GetExamById), new { examId = examDto.Id }, examDto);
        }



    }
}
