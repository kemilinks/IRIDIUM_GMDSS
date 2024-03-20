using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Utility
{
    public static class Converter
    {
        public static string FromBitsToHexString(string input)
        {
            return string.Join("", Enumerable.Range(0, input.Length / 8).Select(i => Convert.ToByte(input.Substring(i * 8, 8), 2).ToString("X2")));
        }

        public static string FromHexStringToBits(string input)
        {
            return String.Join(String.Empty, input.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }

        public static int FromBitsToInteger(string input)
        {
            return Convert.ToInt32(input, 2);
        }
    }
}
