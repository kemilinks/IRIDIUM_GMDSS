using IRIDIUM_GMDSS_LRIT.Core.Dal;
using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using IRIDIUM_GMDSS_LRIT.Core.WcfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Mgr
{
    public class DataCommandMgr
    {
        private DataCommandDal dataCommandDal;
        public DataCommandMgr()
        {
            this.dataCommandDal = new DataCommandDal();
        }

        public List<DataCommand> GetDataCommands(Direction direction, CommandStatus status)
        {
            return this.dataCommandDal.GetDataCommands(direction, status);
        }

        public List<DataCommand> GetDataCommands(bool is2Ways, string source, string destination, DateTime from, DateTime to)
        {
            return this.dataCommandDal.GetDataCommands(is2Ways, source, destination, from, to);
        }

        public void UpdateDataCommand(DataCommand command)
        {
            this.dataCommandDal.UpdateDataCommand(command);
        }

        public void InsertCommand(DataCommand command)
        {
            this.dataCommandDal.InsertDataCommand(command);
        }

        public bool IssueRegistrationCommand(string destination, string imoNumber)
        {
            SystemConfigUtility systemConfigUtility = new SystemConfigUtility();
            string commandData = string.Empty;
            commandData = CommandFactory.Get_Activation_Command_InHex(Convert.ToInt32(imoNumber));
            string lritShortCode = systemConfigUtility.GetLRITShortCode();
            if (!string.IsNullOrEmpty(lritShortCode))
            {
                IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand dataCommand = new IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand();
                dataCommand.Data = commandData;
                dataCommand.Destination = destination;
                dataCommand.Direction = Direction.Mobile_Terminated;
                dataCommand.ReferenceNumber = string.Empty;
                dataCommand.Source = lritShortCode;
                dataCommand.Status = CommandStatus.New;
                dataCommand.Timestamp = DateTime.UtcNow;
                dataCommand.Type = DataCommandType.Registration.ToString();
                InsertCommand(dataCommand);
                return true;
            }
            else
                return false;
        }

        public bool IssueDeregistrationCommand(string destination)
        {
            SystemConfigUtility systemConfigUtility = new SystemConfigUtility();
            string commandData = string.Empty;
            commandData = CommandFactory.Get_Deactivation_Command_InHex();
            string lritShortCode = systemConfigUtility.GetLRITShortCode();
            if (!string.IsNullOrEmpty(lritShortCode))
            {
                IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand dataCommand = new IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand();
                dataCommand.Data = commandData;
                dataCommand.Destination = destination;
                dataCommand.Direction = Direction.Mobile_Terminated;
                dataCommand.ReferenceNumber = string.Empty;
                dataCommand.Source = lritShortCode;
                dataCommand.Status = CommandStatus.New;
                dataCommand.Timestamp = DateTime.UtcNow;
                dataCommand.Type = DataCommandType.Deregistration.ToString();
                InsertCommand(dataCommand);
                return true;
            }
            else
                return false;
        }

        public GatewayServiceResponse IssueChangeIntervalCommand(string msisdn, int intervalInMinutes, string applicationId, string accessCode)
        {
            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Change Interval Command", "msisdn: " + msisdn + ", interval In Minutes: " + intervalInMinutes + ", applicationId: " + applicationId + ", accessCode: " + accessCode);
            AccessLevelMgr accessLevelMgr = new AccessLevelMgr();
            try
            {
                GatewayServiceResponse response = accessLevelMgr.CheckAccessLevel(msisdn, applicationId, accessCode);
                if (response.isSuccessful)
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Change Interval Command", "Pass checking access level");
                    Random rad = new Random();
                    int randomInt = rad.Next(1, 15);
                    SystemConfigUtility systemConfigUtility = new SystemConfigUtility();
                    string commandData = string.Empty;
                    commandData = CommandFactory.Get_Change_Interval_Command_In_Hex(intervalInMinutes, randomInt);
                    string lritShortCode = systemConfigUtility.GetLRITShortCode();
                    if (!string.IsNullOrEmpty(lritShortCode))
                    {
                        IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand dataCommand = new IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand();
                        dataCommand.Data = commandData;
                        dataCommand.Destination = msisdn;
                        dataCommand.Direction = Direction.Mobile_Terminated;
                        dataCommand.ReferenceNumber = string.Empty;
                        dataCommand.Source = lritShortCode;
                        dataCommand.Status = CommandStatus.New;
                        dataCommand.Timestamp = DateTime.UtcNow;
                        dataCommand.Type = DataCommandType.ChangeInterval.ToString();
                        InsertCommand(dataCommand);
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Change Interval Command", "Change Interval Command Issued");
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Success,
                            isSuccessful = true,
                            message = "From Gateway - Change Interval Command Submitted"
                        };

                    }
                    else
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Change Interval Command", "Change Interval Command Not Submitted due to Missing lritShortCode.");
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Success,
                            isSuccessful = false,
                            message = "From Gateway - Change Interval Command Not Submitted due to Missing lritShortCode."
                        };
                    }
                }
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Change Interval Command", "Failed at checking access level. GatewayResponse: " + response.gatewayResponse.ToString() + ", message: " + response.message);
                    return response;
                }


            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Exception during IssueChangeIntervalCommand.", ex.Message);
                return new GatewayServiceResponse()
                {
                    gatewayResponse = Response.Fail,
                    message = "Error occured at Gateway. Contact System Admin.",
                    isSuccessful = true
                };

            }
        }

        public GatewayServiceResponse IssueOnDemandPollCommand(string msisdn, string applicationId, string accessCode)
        {
            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue On demand Poll Command", "msisdn: " + msisdn + ", applicationId: " + applicationId + ", accessCode: " + accessCode);
            AccessLevelMgr accessLevelMgr = new AccessLevelMgr();
            try
            {
                GatewayServiceResponse response = accessLevelMgr.CheckAccessLevel(msisdn, applicationId, accessCode);
                if (response.isSuccessful)
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue On demand Poll Command", "Pass checking access level");
                    SystemConfigUtility systemConfigUtility = new SystemConfigUtility();
                    string commandData = string.Empty;
                    commandData = CommandFactory.Get_Ondemand_Poll_Command_InHex();
                    string lritShortCode = systemConfigUtility.GetLRITShortCode();
                    if (!string.IsNullOrEmpty(lritShortCode))
                    {
                        IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand dataCommand = new IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand();
                        dataCommand.Data = commandData;
                        dataCommand.Destination = msisdn;
                        dataCommand.Direction = Direction.Mobile_Terminated;
                        dataCommand.ReferenceNumber = string.Empty;
                        dataCommand.Source = lritShortCode;
                        dataCommand.Status = CommandStatus.New;
                        dataCommand.Timestamp = DateTime.UtcNow;
                        dataCommand.Type = DataCommandType.OndemandPoll.ToString();
                        InsertCommand(dataCommand);
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue On demand Poll Command", "On demand Poll Command Issued");
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Success,
                            isSuccessful = true,
                            message = "From Gateway - On Demand Poll Command Submitted"
                        };
                    }
                    else
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue On demand Poll Command", " Demand Poll Command Not Submitted due to Missing lritShortCode");
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Success,
                            isSuccessful = false,
                            message = "From Gateway - Demand Poll Command Not Submitted due to Missing lritShortCode."
                        };
                    }
                }
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue On demand Poll Command", "Failed at checking access level. GatewayResponse: " + response.gatewayResponse.ToString() + ", message: " + response.message);
                    return response;
                }


            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Exception during IssueOnDemandPollCommand.", ex.Message);
                return new GatewayServiceResponse()
                {
                    gatewayResponse = Response.Fail,
                    message = "Error occured at Gateway. Contact System Admin.",
                    isSuccessful = true
                };

            }
        }

        public GatewayServiceResponse IssueStopReportingCommand(string msisdn, string applicationId, string accessCode)
        {
            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Stop Reporting Command", "msisdn: " + msisdn + ", applicationId: " + applicationId + ", accessCode: " + accessCode);
            AccessLevelMgr accessLevelMgr = new AccessLevelMgr();
            try
            {
                GatewayServiceResponse response = accessLevelMgr.CheckAccessLevel(msisdn, applicationId, accessCode);
                if (response.isSuccessful)
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Stop Reporting Command", "Pass checking access level");
                    SystemConfigUtility systemConfigUtility = new SystemConfigUtility();
                    string commandData = string.Empty;
                    commandData = CommandFactory.Get_Stop_Command_InHex();
                    string lritShortCode = systemConfigUtility.GetLRITShortCode();
                    if (!string.IsNullOrEmpty(lritShortCode))
                    {
                        IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand dataCommand = new IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand();
                        dataCommand.Data = commandData;
                        dataCommand.Destination = msisdn;
                        dataCommand.Direction = Direction.Mobile_Terminated;
                        dataCommand.ReferenceNumber = string.Empty;
                        dataCommand.Source = lritShortCode;
                        dataCommand.Status = CommandStatus.New;
                        dataCommand.Timestamp = DateTime.UtcNow;
                        dataCommand.Type = DataCommandType.StopReporting.ToString();
                        InsertCommand(dataCommand);
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Stop Reporting Command", "Stop Reporting Command Issued");
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Success,
                            isSuccessful = true,
                            message = "From Gateway - Stop Reporting Command Submitted"
                        };
                    }
                    else
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Stop Reporting Command", "Stop Reporting Command Not Submitted due to Missing lritShortCode");
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Success,
                            isSuccessful = false,
                            message = "From Gateway - Stop Reporting Command Not Submitted due to Missing lritShortCode."
                        };
                    }
                }
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Issue Stop Reporting Command", "Failed at checking access level. GatewayResponse: " + response.gatewayResponse.ToString() + ", message: " + response.message);
                    return response;
                }


            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Exception during IssueStopReportingCommand.", ex.Message);
                return new GatewayServiceResponse()
                {
                    gatewayResponse = Response.Fail,
                    message = "Error occured at Gateway. Contact System Admin.",
                    isSuccessful = true
                };

            }
        }
    }
}
