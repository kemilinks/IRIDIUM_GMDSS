using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Utility
{
    public class CommonUtility
    {
        public static DateTime GetCurrentTimestmap()
        {
            return DateTime.UtcNow;
        }
    }
}
