using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Application.Dtos.MacAddressDtos;
using TestApp.Domain.Models;

namespace TestApp.Domain.Interfaces
{
    public interface IMacRepo : IGenericRepo<MacAddress>
    {
        public Task<HashSet<MacAddress>> GetMacAddressesListAsync(int pageNumber, int pageSize);

        public Task<object> AddMacAddress(AddMacAddressDto dto);


    }
}
