using Microsoft.AspNetCore.Authorization;
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
        private readonly JwtSettings _jwtSettings;

        public HomeownerAuthController(HomeownerAuthService homeownerAuthService, JwtSettings jwtSettings) 
        {
            _homeownerAuthService = homeownerAuthService;
            _jwtSettings = jwtSettings;
        }


        [HttpGet("getAllHomeowner")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpGet("getHomeowner/{homeownerUserName}")]
        public async Task<ActionResult<HomeownerModel>> GetByUserName(string homeownerUserName)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(homeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            return homeowner;
        }

        [HttpPost("updateHomeowner/{homeownerUserName}")]
        public async Task<ActionResult<HomeownerModel>> UpdateByUserName(string homeownerUserName, [FromBody] HomeownerModel updatedHomeowner)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(homeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            homeowner = updatedHomeowner;

            await _homeownerAuthService.UpdateByUserNameAsync(homeownerUserName, homeowner);

            return Ok();
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
                return BadRequest("Invalid homeowner request");
            }

            if (response)
            {
                var token = new UserTokens();
                var homeowner = await _homeownerAuthService.GetByUserNameAsync(loginModel.UserName);
                token = JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens()
                {
                    Email = homeowner.Email,
                    GuidId = Guid.NewGuid(),
                    UserName = homeowner.UserName,
                    FullName = homeowner.FullName,
                    Id = homeowner.Id,
                    UserType = homeowner.UserType
                }, _jwtSettings);

                return Ok(token);
            }
            

            return Unauthorized();
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
