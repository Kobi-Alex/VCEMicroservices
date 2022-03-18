using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Question.API.Application.Services.Interfaces;
using Question.API.Application.Contracts.Dtos.QuestionCategoryDtos;
using Question.API.Application.Paggination;


namespace Question.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CategoriesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        // GET api/Categories
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> GetCategories(int page, string filter, int limit, int middleVal = 10, int cntBetween = 5, CancellationToken cancellationToken = default)
        {
            var categories = await _serviceManager.QuestionCategoryService.GetAllAsync(cancellationToken);

            Console.WriteLine("--> Getting categories...");


            if (middleVal <= cntBetween) return BadRequest(new { Error = "MiddleVal must be more than cntBetween" });


            return Ok(Pagination<QuestionCategoryReadDto>.GetData(currentPage: page,limit: limit, itemsData:categories, middleVal:middleVal, cntBetween:cntBetween));
            //return Ok(categories);
        }


        // GET api/Categories/5
        [HttpGet("{id}", Name = "GetCategoryById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var category = await _serviceManager.QuestionCategoryService.GetByIdAsync(id, cancellationToken);

            Console.WriteLine("--> Getting category by ID...");
            return Ok(category);
        }


        // POST api/Categories
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> CreateCategory([FromBody] QuestionCategoryCreateDto questionCategoryCreateDto)
        {
            var categoryDto = await _serviceManager.QuestionCategoryService.CreateAsync(questionCategoryCreateDto);

            Console.WriteLine("--> Creating new category");
            return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id}, categoryDto);
        }


        // PUT api/Categories/5
        [HttpPut("{id}", Name = "UpdateCategory")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Teacher")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] QuestionCategoryUpdateDto questionCategoryUpdateDto, CancellationToken cancellationToken)
        {
            await _serviceManager.QuestionCategoryService.UpdateAsync(id, questionCategoryUpdateDto, cancellationToken);

            Console.WriteLine($"--> Updating category by ID = {id}");
            return NoContent();
        }
    }
}