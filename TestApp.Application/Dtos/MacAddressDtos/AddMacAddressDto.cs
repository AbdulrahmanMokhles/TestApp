using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Application.Dtos.MacAddressDtos
{
    public class AddMacAddressDto
    {
        [Required(ErrorMessage ="Required MacAddress")]
        public string MacAddress { get; set; }
        public int? Count { get; set; }
        public int ProductId { get; set; }
    }
}
