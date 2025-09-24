using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Application.Dtos.MacAddressDtos;
using TestApp.Application.MacHelper;
using TestApp.Domain.Interfaces;
using TestApp.Domain.Models;
using TestApp.Infrastrcture.Data;

namespace TestApp.Infrastrcture.Repositories
{
    public class MacAddressRepo : RepoBase<MacAddress> , IMacRepo
    {
        public MacAddressRepo(Context context) : base(context)
        {
            _context = context;
        }
        private Context _context;


        public async Task<List<string>> GetMacAddressesListAsync()
        {
            return await _context.MacAddresses
                .Select(m => m.Mac)
                .ToListAsync();
        }

        public List<string> AddeddMacAddresses(AddMacAddressDto dto)
        {
            if (!MacHelper.IsValidMac(dto.MacAddress))
                throw new ArgumentException("Invalid MAC Address");

            string normalized = MacHelper.FinalMac(dto.MacAddress);
            ulong macNumber = Convert.ToUInt64(normalized.Replace(":", ""), 16);

            var macs = new List<string>();

            for (int i = 0; i < dto.Count; i++)
            {
                string macHex = (macNumber + (ulong)i).ToString("X12");
                macs.Add(MacHelper.FinalMac(macHex));
            }

            return macs;
        }

        public async Task<List<string>> AddMacAddress(AddMacAddressDto dto)
        {
            var toAddList = AddeddMacAddresses(dto);
            foreach (var mac in toAddList)
            {
                var macAddress = new MacAddress { Mac = mac , ProductId = dto.ProductId};
                await Add(macAddress);
            }
            var firstAndLast = new List<string>();

            firstAndLast.Add(toAddList.First());
            firstAndLast.Add(toAddList.Last());

            return firstAndLast;
        }
    }
}
