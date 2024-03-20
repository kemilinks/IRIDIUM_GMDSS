using IRIDIUM_GMDSS_LRIT.Core.Forwarder.Interface;
using IRIDIUM_GMDSS_LRIT.Core.WcfService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.WcfService.Entity
{
    public class Application
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccessCode { get; set; }
        public string ExtraInfo { get; set; }
        public string Description { get; set; }
        public string ForwarderFullAssemblyClassName { get; set; }
        public string ForwarderEndpoint { get; set; }
        public bool Active { get; set; }
        
        public IPositionForwarder Forwarder { get; set; }

    }
}
