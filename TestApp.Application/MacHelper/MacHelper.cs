using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            string result="";

            for (int i = 0; i < 6; i++)
            {
                result += mac.Substring(i * 2, 2);
            }

            return result;
        }
    }
}
