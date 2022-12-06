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
        private readonly TransactionService _transactionService;
        private readonly JwtSettings _jwtSettings;

        public TenantAuthController(TenantAuthService tenantAuthService, HomeownerAuthService homeownerAuthService,
            TenantRegistrationFormService tenantRegistrationFormService, JwtSettings jwtSettings,
            TransactionService transactionService)
        {
            _jwtSettings = jwtSettings;
            _homeownerAuthService = homeownerAuthService;
            _tenantAuthService = tenantAuthService;
            _tenantRegistrationFormService = tenantRegistrationFormService;
            _transactionService = transactionService;
        }

        [HttpGet("getTenant/{id:length(24)}")                                                                     ]
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

            return Ok(tenantRentRequestList);
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


        [HttpGet("getMonthlyBill/{tenantUsername}")]
        public async Task<ActionResult<int>> GetMonthlyBill(string tenantUsername)
        {
            var tenant = await _tenantAuthService.GetByUserNameAsync(tenantUsername);

            var homeowner = await _homeownerAuthService.GetByUserNameAsync(tenant.HomeownerUsername);

            var amount = 0;
            foreach (var house in homeowner.HouseList) {
                foreach (var flat in house.FlatList) { 
                    if(flat.FlatId == tenant.FlatId && flat.HouseId == tenant.HouseId)
                    {
                        amount = amount + flat.Rent;
                        foreach (var utilityBill in flat.UtilityBillList) {
                            amount = amount + utilityBill.BillAmount;
                        }
                    }
                }
            }

            return amount;
        }


        [HttpPost("saveTransaction")]
        public async Task<ActionResult<bool>> SaveTransaction([FromBody] TransactionModel newTransaction)
        {
            var transaction = await _transactionService.GetAsync(newTransaction.Id);
            
            if (transaction!=null)                          
            {
                return BadRequest();
            }

            await _transactionService.CreateAsync(newTransaction);

            return Ok(true);
        }

        [HttpGet("getTransactionForTenant/{tenantUsername}")]
        public async Task<ActionResult<List<TransactionModel>>> GetTransactionForTenantl(string tenantUsername)
        {
            var transactionList = await _transactionService.GetAsync();

            var transactionForTenant = new List<TransactionModel>();

            foreach(var transaction in transactionList)
            {
                if(transaction.TenantUserName == tenantUsername)
                {
                    transactionForTenant.Add(transaction);
                }
            }
            transactionForTenant = transactionForTenant.OrderByDescending(o => o.TransactionDate).ToList();

            return transactionForTenant;
        }


        [HttpGet("currentMonthPayment/{tenantUsername}")]
        public async Task<ActionResult<bool>> currentMonthPayment(string tenantUsername)
        {
            var currentDate = DateTime.Now;
            var transactionList = await _transactionService.GetAsync();

            if (transactionList == null)
            {
                return Ok(true);
            }

            var transactionForTenant = new List<TransactionModel>();

            foreach (var transaction in transactionList)
            {
                if (transaction.TenantUserName == tenantUsername)
                {
                    transactionForTenant.Add(transaction);
                }
            }

            if (transactionForTenant.Count==0)
            {
                return Ok(true);
            }

            transactionForTenant = transactionForTenant.OrderByDescending(o => o.TransactionDate).ToList();

            Console.WriteLine(currentDate.Month.CompareTo(transactionForTenant[0].TransactionDate.Month));

            if (currentDate.Month.CompareTo(transactionForTenant[0].TransactionDate.Month) == 1)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }

        }

        [HttpGet("getTransactionForHomeowner/{homeownerUsername}/{houseId}/{flatId}")]
        public async Task<ActionResult<List<TransactionModel>>> getTransactionForHomeowner(string homeownerUsername, string houseId, string flatId)
        {
            var transactionList = await _transactionService.GetAsync();

            var transactionForHomeowner = new List<TransactionModel>();

            foreach (var transaction in transactionList)
            {
                if (transaction.HomeownerUserName == homeownerUsername && transaction.HouseId == houseId
                    && transaction.FlatId == flatId)
                {
                    transactionForHomeowner.Add(transaction);
                }
            }
            transactionForHomeowner = transactionForHomeowner.OrderByDescending(o => o.TransactionDate).ToList();

            return transactionForHomeowner;
        }


        /*[HttpDelete("{id:length(24)}")]
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
        }*/

    }
}
