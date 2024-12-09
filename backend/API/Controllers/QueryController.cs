using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PManager.Core.Interfaces;
using PManager.API.Models;

namespace PManager.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueryController : ControllerBase
    {
        private readonly IQueryManager _queryManager;

        public QueryController(IQueryManager queryManager)
        {
            _queryManager = queryManager;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SavePassword([FromBody] PasswordModel model)
        {
            await _queryManager.SavePasswordAsync(model.Login, model.Password);
            return Ok("Password saved successfully.");
        }

        [HttpGet("get/{login}")]
        public async Task<IActionResult> GetPassword(string login)
        {
            var password = await _queryManager.FindPasswordAsync(login);

            if (password == null)
            {
                return NotFound("Password not found.");
            }
            return Ok(password);
        }

        [HttpPost("change")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordModel model)
        {
            await _queryManager.ChangePasswordAsync(model.Login, model.Password);
            return Ok("Password changed successfully.");
        }

        [HttpGet("get/list")]
        public async Task<IActionResult> GetAllPasswords()
        {
            try
            {
                var passwords = await _queryManager.GetAllPasswordsAsync();

                if (passwords == null || !passwords.Any())
                {
                    return NotFound("No passwords found.");
                }

                return Ok(passwords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving passwords: {ex.Message}");
            }
        }

    }
}
