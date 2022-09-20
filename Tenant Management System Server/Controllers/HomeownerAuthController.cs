using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant_Management_System_Server.Models;
using Tenant_Management_System_Server.Services;

namespace Tenant_Management_System_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeownerAuthController : ControllerBase
    {
        private readonly HomeownerAuthService _homeownerAuthService;

        public HomeownerAuthController(HomeownerAuthService homeownerAuthService) =>
            _homeownerAuthService = homeownerAuthService;

        [HttpGet("getAllHomeowner")]
        public async Task<List<HomeownerModel>> GetAllHomeowner() =>
        await _homeownerAuthService.GetAsync();


        [HttpGet("getHomeowner/{id:length(24)}")]
        public async Task<ActionResult<HomeownerModel>> Get(string id)
        {
            var homeowner = await _homeownerAuthService.GetAsync(id);

            if (homeowner is null)
            {
                return NotFound();
            }

            return homeowner;
        }


        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] HomeownerModel newHomeowner)
        {
            var response = await _homeownerAuthService.SignUpAsync(newHomeowner);

            if (response is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {

            var response = await _homeownerAuthService.LoginAsync(loginModel);

            if (!response)
            {
                return BadRequest();
            }

            return Ok(200);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, HomeownerModel updatedHomeowner)
        {
            var homeowner = await _homeownerAuthService.GetAsync(id);

            if (homeowner is null)
            {
                return NotFound();
            }

            updatedHomeowner.Id = homeowner.Id;

            await _homeownerAuthService.UpdateAsync(id, updatedHomeowner);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var homeowner = await _homeownerAuthService.GetAsync(id);

            if (homeowner is null)
            {
                return NotFound();
            }

            await _homeownerAuthService.RemoveAsync(id);

            return NoContent();
        }
    }
}
