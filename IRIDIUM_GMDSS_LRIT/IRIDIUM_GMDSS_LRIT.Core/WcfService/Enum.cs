using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.WcfService
{
    public enum Response
    {
        AuthenticationFail = 0,
        AccessDeny = 1,
        NotActive = 2,
        TerminalNotFound = 3,
        TerminalStatusError = 4,
        InValidData = 5,
        Success = 6,
        Fail = 7,
    }
}
