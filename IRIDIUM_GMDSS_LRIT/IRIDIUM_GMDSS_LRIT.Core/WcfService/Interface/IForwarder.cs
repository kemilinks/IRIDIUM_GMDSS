using IRIDIUM_GMDSS_LRIT.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.WcfService.Interface
{
    public interface IForwarder
    {
        string EndPoint { get; set; }
        string ForwardResultStatus { get; }
        string ExtraInfo { get; set; }

        bool ForwardMessage(DataCommand data);
        bool DoSpecialOpt(params object[] list);
    }
}
