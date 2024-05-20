using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Utility
{
    public class CommonUtility
    {
        public  static string _SYSTEM_DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

        public static DateTime GetCurrentTimestmap()
        {
            return DateTime.UtcNow;
        }

        public static string ConvertDateTimeToString(DateTime input, string format)
        {
            return input.ToString(format);
        }

        public static DateTime ConvertStringToDateTime(string strDateTime, string format)
        {
            return DateTime.ParseExact(strDateTime, format, CultureInfo.InvariantCulture);
        }
    }
}
