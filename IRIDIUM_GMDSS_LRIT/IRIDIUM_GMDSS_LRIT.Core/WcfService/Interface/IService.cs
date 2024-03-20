using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.WcfService.Interface
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        GatewayServiceResponse GetTerminalStatus(string msisdn, string applicationId, string accessCode);

        [OperationContract]
        GatewayServiceResponse RegisterNewTerminal(string msisdn, string imoNumber, string description, string applicationId, string accessCode);

        [OperationContract]
        GatewayServiceResponse ChangeInterval(string msisdn, int intervalInMinutes, string applicationId, string accessCode);

        [OperationContract]
        GatewayServiceResponse OnDemandPoll(string msisdn, string applicationId, string accessCode);

        [OperationContract]
        GatewayServiceResponse StopReporting(string msisdn, string applicationId, string accessCode);

        [OperationContract]
        GatewayServiceResponse DeregisterTerminal(string msisdn, string applicationId, string accessCode);


    }
}
