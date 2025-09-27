using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApp.Application.Dtos.MacAddressDtos;
using TestApp.Domain.Interfaces;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MacAddressController(IMacRepo macRepo) : ControllerBase
    {
        private readonly IMacRepo _macAddressRepo=macRepo;

        [HttpGet("data-table")]
        public async Task<IActionResult> GetAll(int pageNo , int pageSize)
        {
            var result = await _macAddressRepo.GetMacAddressesListAsync(pageNo,pageSize);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddMac(AddMacAddressDto dto)
        {
            var result = await _macAddressRepo.AddMacAddress(dto);
            return Ok(result);
        }

    }
}
