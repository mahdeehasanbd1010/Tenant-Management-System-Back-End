﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly JwtSettings _jwtSettings;

        public TenantRegistrationFormController(TenantRegistrationFormService tenantRegistrationFormService, JwtSettings jwtSettings)
        {
            _tenantRegistrationFormService = tenantRegistrationFormService;
            _jwtSettings = jwtSettings;
        }

        [HttpGet("getTenantInfo/{id:length(24)}")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<TenantRegistrationFormModel>> Get(string id)
        {
            var tenantRegistrationForm = await _tenantRegistrationFormService.GetAsync(id);

            if (tenantRegistrationForm is null)
            {
                return NotFound();
            }

            return tenantRegistrationForm;
        }


        [HttpPost("save")]
        public async Task<IActionResult> SaveInfo([FromBody] TenantRegistrationFormModel newTenantRegistrationFormModel)
        {

            var response = await _tenantRegistrationFormService.SaveInfoAsync(newTenantRegistrationFormModel);

            if (response is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }
    }
}