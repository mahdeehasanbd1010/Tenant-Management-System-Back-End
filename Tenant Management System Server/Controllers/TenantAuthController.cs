using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant_Management_System_Server.Models;
using Tenant_Management_System_Server.Services;

namespace Tenant_Management_System_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantAuthController : ControllerBase
    {
        private readonly TenantAuthService _tenantAuthService;
        private readonly HomeownerAuthService _homeownerAuthService;
        private readonly JwtSettings _jwtSettings;

        public TenantAuthController(TenantAuthService tenantAuthService, HomeownerAuthService homeownerAuthService, JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
            _homeownerAuthService = homeownerAuthService;
            _tenantAuthService = tenantAuthService;
        }

        [HttpGet("getAllTenant")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<TenantModel>> GetAllHomeowner() =>
        await _tenantAuthService.GetAsync();


        [HttpGet("getTenant/{id:length(24)}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<TenantModel>> Get(string id)
        {
            var tenant = await _tenantAuthService.GetAsync(id);

            if (tenant is null)
            {
                return NotFound();
            }

            return tenant;
        }


        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] TenantModel newTenant)
        {
            var response = await _tenantAuthService.SignUpAsync(newTenant);

            if (response is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {

            var response = await _tenantAuthService.LoginAsync(loginModel);

            if (!response)
            {
                return BadRequest("Invalid tenant request");
            }

            if (response)
            {
                var token = new UserTokens();
                var tenant = await _tenantAuthService.GetByUserNameAsync(loginModel.UserName);
                token = JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens()
                {
                    Email = tenant.Email,
                    GuidId = Guid.NewGuid(),
                    UserName = tenant.UserName,
                    FullName = tenant.FullName,
                    Id = tenant.Id,
                    UserType = tenant.UserType,
                }, _jwtSettings);

                return Ok(token);
            }

            return Unauthorized();
        }

        [HttpPut("{id:length(24)}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(string id, TenantModel updatedTenant)
        {
            var tenant = await _tenantAuthService.GetAsync(id);

            if (tenant is null)
            {
                return NotFound();
            }

            updatedTenant.Id = tenant.Id;

            await _tenantAuthService.UpdateAsync(id, updatedTenant);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(string id)
        {
            var tenant = await _tenantAuthService.GetAsync(id);

            if (tenant is null)
            {
                return NotFound();
            }

            await _tenantAuthService.RemoveAsync(id);

            return NoContent();
        }

    }
}
