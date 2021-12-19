using Exam.Domain.Contracts.Dtos;
using Exam.Domain.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exam.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ExamsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetExams(CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting exams...");
            var exams = await _serviceManager.ExamService.GetAllAsync(cancellationToken);

            return Ok(exams);
        }

        [HttpGet("{examId:int}")]
        public async Task<IActionResult> GetExamById(int examId, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting exam by Id...");
            var examDto = await _serviceManager.ExamService.GetByIdAsync(examId, cancellationToken);

            return Ok(examDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExam([FromBody] ExamCreateDto examCreateDto)
        {
            Console.WriteLine("--> Creating exam...");
            var examDto = await _serviceManager.ExamService.CreateAsync(examCreateDto);

            return CreatedAtAction(nameof(GetExamById), new { examId = examDto.Id }, examDto);
        }

    }
}
