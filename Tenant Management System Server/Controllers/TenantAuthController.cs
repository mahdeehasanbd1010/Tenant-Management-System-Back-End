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
        private readonly TenantRegistrationFormService _tenantRegistrationFormService;
        private readonly HomeownerAuthService _homeownerAuthService;
        private readonly JwtSettings _jwtSettings;

        public TenantAuthController(TenantAuthService tenantAuthService, HomeownerAuthService homeownerAuthService,
            TenantRegistrationFormService tenantRegistrationFormService, JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
            _homeownerAuthService = homeownerAuthService;
            _tenantAuthService = tenantAuthService;
            _tenantRegistrationFormService = tenantRegistrationFormService;
        }

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


        [HttpGet("getTenantByUsername/{tenantUsername}")]
        public async Task<ActionResult<TenantModel>> GetTenantByUsername(string tenantUsername)
        {
            var tenant = await _tenantAuthService.GetByUserNameAsync(tenantUsername);

            if (tenant is null)
            {
                return NotFound();
            }

            return tenant;
        }


        [HttpGet("getAllTenant")]
        public async Task<List<TenantModel>> GetAllTenant() =>
        await _tenantAuthService.GetAsync();


        [HttpPost("updateTenant/{tenantUserName}")]
        public async Task<ActionResult<HomeownerModel>> UpdateByUserName(string tenantUserName, [FromBody] TenantModel updatedTenant)
        {
            var tenant = await _tenantAuthService.GetByUserNameAsync(tenantUserName);

            if (tenant is null)
            {
                return NotFound();
            }

            tenant = updatedTenant;

            await _tenantAuthService.UpdateByUserNameAsync(tenantUserName, tenant);

            return Ok(updatedTenant);
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

        [HttpGet("getRentRequestList/{homeownerUsername}")]
        public async Task<ActionResult<List<TenantModel>>> GetRentRequestList(string homeownerUsername)
        {
            var tenantList = await _tenantAuthService.GetAsync();

            List<TenantModel> tenantRentRequestList = new List<TenantModel>();
            foreach(var tenant in tenantList)
            {
                if(tenant.IsTenantFormFillUp && !tenant.IsRentRequestAccept && tenant.HomeownerUsername == homeownerUsername)
                {
                    tenantRentRequestList.Add(tenant);
                }
            }

            return tenantRentRequestList;
        }


        [HttpGet("acceptRentRequest/{homeownerUsername}/{tenantUsername}/{houseId}/{flatId}")]
        public async Task<ActionResult> AcceptRentRequest(string homeownerUsername, string tenantUsername, string houseId, string flatId)
        {
            var tenantList = await _tenantAuthService.GetAsync();

            foreach (var tenant in tenantList)
            {
                if (tenant.IsTenantFormFillUp && !tenant.IsRentRequestAccept && tenant.HomeownerUsername == homeownerUsername)
                {
                    if(tenant.UserName == tenantUsername)
                    {
                        var homeowner = await _homeownerAuthService.GetByUserNameAsync(homeownerUsername);

                        foreach (var house in homeowner.HouseList)
                        {
                            foreach(var flat in house.FlatList)
                            {
                                if(flat.FlatId == flatId && !flat.IsRent && flat.IsRentRequest)
                                {
                                    tenant.IsRentRequestAccept = true;
                                    await _tenantAuthService.UpdateByUserNameAsync(tenantUsername, tenant);

                                    flat.IsRent = true;
                                    flat.TenantUserName = tenantUsername;
                                    await _homeownerAuthService.UpdateByUserNameAsync(homeownerUsername, homeowner);
                                    return Ok();
                                }
                            }
                        }
                        
                    }
                    
                }
            }

            return NotFound();
        }


        [HttpDelete("deleteRentRequest/{homeownerUsername}/{tenantUsername}/{houseId}/{flatId}")]
        public async Task<IActionResult> Delete(string homeownerUsername, string tenantUsername, string houseId, string flatId)
        {
            var tenant = await _tenantAuthService.GetByUserNameAsync(tenantUsername);
            
            if (tenant is null)
            {
                return NotFound();
            }

            if (tenant.IsTenantFormFillUp && !tenant.IsRentRequestAccept && tenant.HomeownerUsername == homeownerUsername)
            {
                if (tenant.UserName == tenantUsername)
                {
                    var homeowner = await _homeownerAuthService.GetByUserNameAsync(homeownerUsername);

                    foreach (var house in homeowner.HouseList)
                    {
                        foreach (var flat in house.FlatList)
                        {
                            if (flat.FlatId == flatId && !flat.IsRent && flat.IsRentRequest)
                            {
                                flat.IsRentRequest = false;
                                await _tenantAuthService.RemoveByUserNameAsync(tenantUsername);
                                await _tenantRegistrationFormService.RemoveByUserNameAsync(tenantUsername);

                                return Ok();
                            }
                        }
                    }
                    
                }

            }
           
            return NotFound();
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

        [HttpPut("{id:length(24)}")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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

    }
}
