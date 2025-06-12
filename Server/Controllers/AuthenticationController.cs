using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> CreateAsync([FromBody] Register user)
        {
            if (user is null) return BadRequest("User model is empty");

            var response = await userAccount.CreateAsync(user);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignAsync([FromBody] Login user)
        {
            if (user is null) return BadRequest("User model is empty");

            var response = await userAccount.SignInAsync(user);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshToken token)
        {
            if (token is null) return BadRequest("Model is empty");

            var response = await userAccount.RefreshTokenAsync(token);
            return Ok(response);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUserAsync()
        {
            var users = await userAccount.GetUsers();
            if (users == null) return NotFound();

            return Ok(users);
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(ManageUser user) {
            var result = await userAccount.UpdateUser(user);

            return Ok(result);
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await userAccount.DeleteUser(id);

            return Ok(result);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            var roles = await userAccount.GetRoles();
            if(roles == null) return NotFound();

            return Ok(roles);
        }
    }
}
