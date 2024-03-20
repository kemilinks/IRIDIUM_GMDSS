using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Dal
{
    internal class CommonDal
    {
        public static string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["IRIDIUM_GMDSS_LRIT_2024_DBConnectionString"].ConnectionString;
        }
    }
}
