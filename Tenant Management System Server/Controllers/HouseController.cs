using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tenant_Management_System_Server.Services;
using Microsoft.AspNetCore.Authorization;
using Tenant_Management_System_Server.Models;
using static MongoDB.Driver.WriteConcern;

namespace Tenant_Management_System_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : ControllerBase
    {
        private readonly HomeownerAuthService _homeownerAuthService;

        public HouseController(HomeownerAuthService homeownerAuthService)
        {
            _homeownerAuthService = homeownerAuthService;   
        }

        public async Task<ActionResult<HomeownerModel>> Get(string id)
        {
            var homeowner = await _homeownerAuthService.GetAsync(id);

            if (homeowner is null)
            {
                return NotFound();
            }

            return homeowner;
        }


        [HttpGet("getAllHouse/{homeownerUserName}")]
        public async Task<ActionResult<List<HouseModel>>> GetAllHouse(string homeownerUserName)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(homeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            return homeowner.HouseList;
        }


        [HttpPost("addHouse")]
        public async Task<IActionResult> AddHouse([FromBody] HouseModel newHouse)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(newHouse.HomeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            foreach (var house in homeowner.HouseList)
            {
                if (house.HouseId == newHouse.HouseId)
                {
                    return BadRequest();
                }
            }

            homeowner.HouseList.Add(newHouse);

            await _homeownerAuthService.UpdateByUserNameAsync(newHouse.HomeownerUserName, homeowner);

            return NoContent();
        }

        [HttpGet("getAllFlat/{homeownerUserName}/{houseId}")]
        public async Task<ActionResult<List<FlatModel>>> GetAllFlat(string homeownerUserName, string houseId)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(homeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            foreach(var house in homeowner.HouseList)
            {
                if(house.HouseId == houseId)
                {
                    return house.FlatList;
                }
            }

            return NotFound();
        }

        [HttpGet("getFlat/{homeownerUserName}/{houseId}/{flatId}")]
        public async Task<ActionResult<FlatModel>> GetFlatInfo(string homeownerUserName, string houseId, string flatId)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(homeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            foreach (var house in homeowner.HouseList)
            {
                if (house.HouseId == houseId)
                {
                    foreach(var flat in house.FlatList)
                    {
                        if(flat.FlatId == flatId) {
                            return flat;
                        }
                    }
                    
                }
            }

            return NotFound();
        }



        [HttpPost("addFlat")]
        public async Task<IActionResult> AddFlat([FromBody] FlatModel newFlat)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(newFlat.HomeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            var flag = false;
            foreach (var house in homeowner.HouseList)
            {
                if (house.HouseId == newFlat.HouseId)
                {
                    var flag2 = house.FlatList.Any(p => p.Id == newFlat.FlatId);
                    if (flag2) break;
                    flag = true;
                    house.FlatList.Add(newFlat);
                }
            }

            if (!flag)
            {
                return BadRequest();
            }

            await _homeownerAuthService.UpdateByUserNameAsync(newFlat.HomeownerUserName, homeowner);

            return NoContent();
        }

        [HttpPost("updateFlat/{flatId}")]
        public async Task<IActionResult> updateFlat(string flatId, [FromBody] FlatModel updatedFlat)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(updatedFlat.HomeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            var flag = false;
            foreach (var house in homeowner.HouseList)
            {
                if (house.HouseId == updatedFlat.HouseId)
                {
                    foreach (var flat in house.FlatList)
                    {
                        if(flat.FlatId == flatId)
                        {
                            int index = house.FlatList.IndexOf(flat);
                            if (index != -1)
                            {
                                house.FlatList[index] = updatedFlat;
                                flag = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (!flag)
            {
                return BadRequest();
            }

            await _homeownerAuthService.UpdateByUserNameAsync(updatedFlat.HomeownerUserName, homeowner);
           
            return Ok(updatedFlat);
        }


        [HttpPost("flat/addUtilityBill/{homeownerUserName}/{houseId}/{flatId}")]
        public async Task<IActionResult> updateFlat(string homeownerUserName, string houseId, string flatId, [FromBody] UtilityBillModel utilityBillModel)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(homeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            var flag = false;
            foreach (var house in homeowner.HouseList)
            {
                if (house.HouseId == houseId)
                {
                    foreach (var flat in house.FlatList)
                    {
                        if (flat.FlatId == flatId)
                        {
                            int index = flat.UtilityBillList.IndexOf(utilityBillModel);
                            if (index == -1)
                            {
                                flat.UtilityBillList.Add(utilityBillModel);
                                flag = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (!flag)
            {
                return BadRequest();
            }

            await _homeownerAuthService.UpdateByUserNameAsync(homeownerUserName, homeowner);

            return Ok(utilityBillModel);
        }


        [HttpGet("flat/getUtilityBillList/{homeownerUserName}/{houseId}/{flatId}")]
        public async Task<ActionResult<List<UtilityBillModel>>> GetUtilityBillInfo(string homeownerUserName, string houseId, string flatId)
        {
            var homeowner = await _homeownerAuthService.GetByUserNameAsync(homeownerUserName);

            if (homeowner is null)
            {
                return NotFound();
            }

            foreach (var house in homeowner.HouseList)
            {
                if (house.HouseId == houseId)
                {
                    foreach (var flat in house.FlatList)
                    {
                        if (flat.FlatId == flatId)
                        {
                            return flat.UtilityBillList;
                        }
                    }

                }
            }

            return NotFound();
        }
    }
}
