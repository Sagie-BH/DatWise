using Application.Services;
using Core.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DatWiseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }


        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> ValidateUserCredentials([FromBody] UserCredentials credentials)
        {
            var existingUser = await _userService.ValidateUserCredentialsAsync(credentials.Email, credentials.Password);
            return Ok(new { existingUser.ID, existingUser.Name, existingUser.Company });
        }
    }
}
