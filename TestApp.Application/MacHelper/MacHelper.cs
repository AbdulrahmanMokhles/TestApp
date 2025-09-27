using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestApp.Application.Dtos.MacAddressDtos;

namespace TestApp.Application.MacHelper
{
    public static class MacHelper
    {
        public static bool IsValidMac(string mac)
        {
            mac = mac.Replace(":", "").Replace("-", "").ToUpper();

            return Regex.IsMatch(mac, "^[0-9A-F]{12}$");
        }

        public static string FinalMac(string mac)
        {
            mac = mac.Replace(":", "").Replace("-", "").ToUpper();

            return string.Join(":",
                (Enumerable.Range(0, 6).Select(i => mac.Substring(i * 2, 2))));
        }

        public static HashSet<string> GenerateMACAddresses(AddMacAddressDto dto)
        {
            if (!IsValidMac(dto.MacAddress))
                throw new ArgumentException("Invalid MAC Address");

            string normalized = FinalMac(dto.MacAddress);
            ulong macNumber = Convert.ToUInt64(normalized.Replace(":", ""), 16);

            var macs = new HashSet<string>();

            for (int i = 0; i < dto.Count; i++)
            {
                string macHex = (macNumber + (ulong)i).ToString("X12");
                macs.Add(FinalMac(macHex));
            }

            return macs;
        }

    }
}
