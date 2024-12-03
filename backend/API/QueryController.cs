using Microsoft.AspNetCore.Mvc;
using PManager.Core.Services;
using PManager.Core.Interfaces;

namespace PManager.API
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("find/{login}")]
        public async Task<IActionResult> FindPassword(string login)
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
    }
}
