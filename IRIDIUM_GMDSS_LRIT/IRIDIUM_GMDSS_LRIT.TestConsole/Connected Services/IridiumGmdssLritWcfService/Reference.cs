﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IRIDIUM_GMDSS_LRIT.TestConsole.IridiumGmdssLritWcfService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="IridiumGmdssLritWcfService.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetTerminalStatus", ReplyAction="http://tempuri.org/IService/GetTerminalStatusResponse")]
        IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse GetTerminalStatus(string msisdn, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetTerminalStatus", ReplyAction="http://tempuri.org/IService/GetTerminalStatusResponse")]
        System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> GetTerminalStatusAsync(string msisdn, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/RegisterNewTerminal", ReplyAction="http://tempuri.org/IService/RegisterNewTerminalResponse")]
        IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse RegisterNewTerminal(string msisdn, string imoNumber, string description, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/RegisterNewTerminal", ReplyAction="http://tempuri.org/IService/RegisterNewTerminalResponse")]
        System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> RegisterNewTerminalAsync(string msisdn, string imoNumber, string description, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/ChangeInterval", ReplyAction="http://tempuri.org/IService/ChangeIntervalResponse")]
        IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse ChangeInterval(string msisdn, int intervalInMinutes, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/ChangeInterval", ReplyAction="http://tempuri.org/IService/ChangeIntervalResponse")]
        System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> ChangeIntervalAsync(string msisdn, int intervalInMinutes, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/OnDemandPoll", ReplyAction="http://tempuri.org/IService/OnDemandPollResponse")]
        IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse OnDemandPoll(string msisdn, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/OnDemandPoll", ReplyAction="http://tempuri.org/IService/OnDemandPollResponse")]
        System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> OnDemandPollAsync(string msisdn, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/StopReporting", ReplyAction="http://tempuri.org/IService/StopReportingResponse")]
        IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse StopReporting(string msisdn, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/StopReporting", ReplyAction="http://tempuri.org/IService/StopReportingResponse")]
        System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> StopReportingAsync(string msisdn, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/DeregisterTerminal", ReplyAction="http://tempuri.org/IService/DeregisterTerminalResponse")]
        IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse DeregisterTerminal(string msisdn, string applicationId, string accessCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/DeregisterTerminal", ReplyAction="http://tempuri.org/IService/DeregisterTerminalResponse")]
        System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> DeregisterTerminalAsync(string msisdn, string applicationId, string accessCode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : IRIDIUM_GMDSS_LRIT.TestConsole.IridiumGmdssLritWcfService.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<IRIDIUM_GMDSS_LRIT.TestConsole.IridiumGmdssLritWcfService.IService>, IRIDIUM_GMDSS_LRIT.TestConsole.IridiumGmdssLritWcfService.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse GetTerminalStatus(string msisdn, string applicationId, string accessCode) {
            return base.Channel.GetTerminalStatus(msisdn, applicationId, accessCode);
        }
        
        public System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> GetTerminalStatusAsync(string msisdn, string applicationId, string accessCode) {
            return base.Channel.GetTerminalStatusAsync(msisdn, applicationId, accessCode);
        }
        
        public IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse RegisterNewTerminal(string msisdn, string imoNumber, string description, string applicationId, string accessCode) {
            return base.Channel.RegisterNewTerminal(msisdn, imoNumber, description, applicationId, accessCode);
        }
        
        public System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> RegisterNewTerminalAsync(string msisdn, string imoNumber, string description, string applicationId, string accessCode) {
            return base.Channel.RegisterNewTerminalAsync(msisdn, imoNumber, description, applicationId, accessCode);
        }
        
        public IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse ChangeInterval(string msisdn, int intervalInMinutes, string applicationId, string accessCode) {
            return base.Channel.ChangeInterval(msisdn, intervalInMinutes, applicationId, accessCode);
        }
        
        public System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> ChangeIntervalAsync(string msisdn, int intervalInMinutes, string applicationId, string accessCode) {
            return base.Channel.ChangeIntervalAsync(msisdn, intervalInMinutes, applicationId, accessCode);
        }
        
        public IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse OnDemandPoll(string msisdn, string applicationId, string accessCode) {
            return base.Channel.OnDemandPoll(msisdn, applicationId, accessCode);
        }
        
        public System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> OnDemandPollAsync(string msisdn, string applicationId, string accessCode) {
            return base.Channel.OnDemandPollAsync(msisdn, applicationId, accessCode);
        }
        
        public IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse StopReporting(string msisdn, string applicationId, string accessCode) {
            return base.Channel.StopReporting(msisdn, applicationId, accessCode);
        }
        
        public System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> StopReportingAsync(string msisdn, string applicationId, string accessCode) {
            return base.Channel.StopReportingAsync(msisdn, applicationId, accessCode);
        }
        
        public IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse DeregisterTerminal(string msisdn, string applicationId, string accessCode) {
            return base.Channel.DeregisterTerminal(msisdn, applicationId, accessCode);
        }
        
        public System.Threading.Tasks.Task<IRIDIUM_GMDSS_LRIT.Core.WcfService.GatewayServiceResponse> DeregisterTerminalAsync(string msisdn, string applicationId, string accessCode) {
            return base.Channel.DeregisterTerminalAsync(msisdn, applicationId, accessCode);
        }
    }
}
