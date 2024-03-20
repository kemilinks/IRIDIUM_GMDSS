using IRIDIUM_GMDSS_LRIT.Core.Mgr;
using IRIDIUM_GMDSS_LRIT.Core.WcfService;
using IRIDIUM_GMDSS_LRIT.Core.WcfService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IRIDIUM_GMDSS_LRIT.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public GatewayServiceResponse GetTerminalStatus(string msisdn, string applicationId, string accessCode)
        {
            TerminalMgr terminalMgr = new TerminalMgr();
            return terminalMgr.GetTerminalStatus(msisdn, applicationId, accessCode);
        }

        public GatewayServiceResponse RegisterNewTerminal(string msisdn, string imoNumber, string description, string applicationId, string accessCode)
        {
            TerminalMgr terminalMgr = new TerminalMgr();
            return terminalMgr.RegisterNewTerminal(msisdn, imoNumber, description, applicationId, accessCode);
        }

        public GatewayServiceResponse ChangeInterval(string msisdn, int intervalInMinutes, string applicationId, string accessCode)
        {
            DataCommandMgr dataCommandMgr = new DataCommandMgr();
            return dataCommandMgr.IssueChangeIntervalCommand(msisdn, intervalInMinutes, applicationId, accessCode);
        }

        public GatewayServiceResponse OnDemandPoll(string msisdn, string applicationId, string accessCode)
        {
            DataCommandMgr dataCommandMgr = new DataCommandMgr();
            return dataCommandMgr.IssueOnDemandPollCommand(msisdn, applicationId, accessCode);
        }

        public GatewayServiceResponse StopReporting(string msisdn, string applicationId, string accessCode)
        {
            DataCommandMgr dataCommandMgr = new DataCommandMgr();
            return dataCommandMgr.IssueStopReportingCommand(msisdn, applicationId, accessCode);
        }

        public GatewayServiceResponse DeregisterTerminal(string msisdn, string applicationId, string accessCode)
        {
            TerminalMgr terminalMgr = new TerminalMgr();
            return terminalMgr.DeregisterTerminal(msisdn, applicationId, accessCode);
        }
    }
}
