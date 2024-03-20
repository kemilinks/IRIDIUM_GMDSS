using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.WcfService.Entity
{
    public enum AccessLevel
    {
        AllowAll = 0,
        ForwardingOnly = 1,
        SubmissionOnly = 2,
        DenyAll = 4
    }
}
