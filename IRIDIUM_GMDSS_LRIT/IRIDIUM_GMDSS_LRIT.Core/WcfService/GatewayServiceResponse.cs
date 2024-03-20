using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.WcfService
{
    public class GatewayServiceResponse
    {
        public Response gatewayResponse { get; set; }
        public bool isSuccessful { get; set; }
        public string message { get; set; }
    }
}
