using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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


    }

}

