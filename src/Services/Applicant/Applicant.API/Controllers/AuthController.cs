using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Applicant.API.Application.Services.Interfaces;
using Applicant.API.Application.Contracts.Dtos.AuthDtos;


namespace Applicant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IServiceManager _serviceManager;
        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDto authRegisterDto)
        {
            if (ModelState.IsValid)
            {
                //We can utilise the model
                var auth = await _serviceManager.AccessCodeService.RegisterUserAsync(authRegisterDto);
                return Ok(auth);
            }

            return BadRequest(new { Error = "Invalid payload" });
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto authLoginDto)
        {
            if (ModelState.IsValid)
            {
                var jwtToken = await _serviceManager.AccessCodeService.LoginUserAsync(authLoginDto);
                return Ok(jwtToken);
            }

            return BadRequest(new { Error = "Invalid payload" });
        }


        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] AuthTokenRequestDto tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _serviceManager.AccessCodeService.RefreshTokenAsync(tokenRequest);

                if (result.Success)
                {
                    Console.WriteLine("\n---> RefreshToken");
                    return Ok(result);
                }
                else
                {
                    return StatusCode(226, result.Error);
                    //return BadRequest(result);
                }
            }
            
            return BadRequest(new { Error = "Invalid payload" });
        }

        [HttpPost]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] AuthRegisterDto emailRequest)
        {
            //Send message on the email

            if (ModelState.IsValid)
            {
                Console.WriteLine($"\n---> Send Access code ...");
                await _serviceManager.AccessCodeService.AccessCodeAsync(emailRequest);

                return Ok();
            }

            return BadRequest(new
            {
                Error = "Invalid data"
            });

        }
    }
}
