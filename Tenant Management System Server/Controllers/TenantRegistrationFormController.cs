using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant_Management_System_Server.Models;
using Tenant_Management_System_Server.Models.TenantRegistrationForm;
using Tenant_Management_System_Server.Services;

namespace Tenant_Management_System_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantRegistrationFormController : ControllerBase
    {
        private readonly TenantRegistrationFormService _tenantRegistrationFormService = null!;
        private readonly TenantAuthService _tenantAuthService = null!;
        private readonly JwtSettings _jwtSettings;

        public TenantRegistrationFormController(TenantRegistrationFormService tenantRegistrationFormService, TenantAuthService tenantAuthService, JwtSettings jwtSettings)
        {
            _tenantRegistrationFormService = tenantRegistrationFormService;
            _tenantAuthService = tenantAuthService;
            _jwtSettings = jwtSettings;
        }

        /*[HttpGet("getTenantInfo/{id:length(24)}")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<TenantRegistrationFormModel>> Get(string id)
        {
            var tenantRegistrationForm = await _tenantRegistrationFormService.GetAsync(id);

            if (tenantRegistrationForm is null)
            {
                return NotFound();
            }

            return tenantRegistrationForm;
        }*/

        [HttpGet("getTenantInfo/{tenantUserName}")]
        public async Task<ActionResult<TenantRegistrationFormModel>> Get(string tenantUserName)
        {
            var tenantRegistrationForm = await _tenantRegistrationFormService.GetByUserNameAsync(tenantUserName);

            if (tenantRegistrationForm is null)
            {
                return NotFound();
            }

            return tenantRegistrationForm;
        }


        [HttpPost("save")]
        public async Task<IActionResult> SaveInfo([FromBody] TenantRegistrationFormModel newTenantRegistrationFormModel)
        {
            var tenantRegistrationFormModel = await _tenantRegistrationFormService.GetByUserNameAsync(newTenantRegistrationFormModel.UserName);

            if(tenantRegistrationFormModel != null)
            {
                return BadRequest();
            }

            var tenant = await _tenantAuthService.GetByUserNameAsync(newTenantRegistrationFormModel.UserName);
            tenant.IsTenantFormFillUp = true;

            await _tenantAuthService.UpdateByUserNameAsync(newTenantRegistrationFormModel.UserName, tenant);

            var response = await _tenantRegistrationFormService.SaveInfoAsync(newTenantRegistrationFormModel);

            if (response is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }
    }
}
