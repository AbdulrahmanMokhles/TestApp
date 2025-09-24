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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _macAddressRepo.GetMacAddressesListAsync();
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
