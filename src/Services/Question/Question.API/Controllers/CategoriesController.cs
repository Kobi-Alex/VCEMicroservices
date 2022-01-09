using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;
using Question.API.Application.Services.Interfaces;

namespace Question.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        // GET api/Categories
        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            var categories = await _serviceManager.QuestionCategoryService.GetAllAsync(cancellationToken);

            Console.WriteLine("--> Getting categories...");
            return Ok(categories);
        }


        // GET api/Categories/5
        [HttpGet("{id}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var category = await _serviceManager.QuestionCategoryService.GetByIdAsync(id, cancellationToken);

            Console.WriteLine("--> Getting category by ID...");
            return Ok(category);
        }


        // POST api/Categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] QuestionCategoryCreateDto questionCategoryCreateDto)
        {
            var categoryDto = await _serviceManager.QuestionCategoryService.CreateAsync(questionCategoryCreateDto);

            Console.WriteLine("--> Creating new category");
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id}, categoryDto);
        }


        // PUT api/Categories/5
        [HttpPut("{id}", Name = "UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] QuestionCategoryUpdateDto questionCategoryUpdateDto, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionCategoryService.UpdateAsync(id, questionCategoryUpdateDto, cancellationToken);

            Console.WriteLine($"--> Updating category by ID = {id}");
            return NoContent();
        }
    }
}