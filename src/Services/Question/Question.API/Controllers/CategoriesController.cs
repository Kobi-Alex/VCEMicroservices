using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;
using Question.API.Application.Services.Interfaces;

namespace Question.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting categories...");

            var categories = await _serviceManager.QuestionCategoryService.GetAllAsync(cancellationToken);

            return Ok(categories);
        }


        [HttpGet("{categoryId}", Name = "GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(int categoryId, CancellationToken cancellationToken)
        {
            Console.WriteLine("--> Getting category by ID...");

            var category = await _serviceManager.QuestionCategoryService.GetByIdAsync(categoryId, cancellationToken);

            return Ok(category);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] QuestionCategoryCreateDto questionCategoryCreateDto)
        {
            var categoryDto = await _serviceManager.QuestionCategoryService.CreateAsync(questionCategoryCreateDto);

            Console.WriteLine("--> Creating new category");

            return CreatedAtAction(nameof(GetCategoryById), new { categoryId = categoryDto.Id}, categoryDto);
        }

    }

}