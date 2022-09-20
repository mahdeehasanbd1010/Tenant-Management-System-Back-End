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

        public TenantAuthController(TenantAuthService tenantAuthService) =>
           _tenantAuthService = tenantAuthService;

        [HttpGet("getAllTenant")]
        public async Task<List<TenantModel>> GetAllHomeowner() =>
        await _tenantAuthService.GetAsync();


        [HttpGet("getTenant/{id:length(24)}")]
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
                return BadRequest();
            }

            return Ok(200);
        }

        [HttpPut("{id:length(24)}")]
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
