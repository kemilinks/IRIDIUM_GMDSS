using IRIDIUM_GMDSS_LRIT.Core.Dal;
using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using IRIDIUM_GMDSS_LRIT.Core.WcfService;
using IRIDIUM_GMDSS_LRIT.Core.WcfService.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Mgr
{
    public class TerminalMgr
    {
        private string _DETAILS_INVALID_TERMINAL = "MSISDN is Not Valid";

        private ApplicationDal applicationDal;
        private TerminalDal terminalDal;
        private TerminalApplicationDal terminalApplicationDal;
        
        public TerminalMgr()
        {
            this.terminalDal = new TerminalDal();
            this.applicationDal = new ApplicationDal();
            this.terminalApplicationDal = new TerminalApplicationDal();
        }

        public Terminal GetTerminal(string msisdn)
        {
            return this.terminalDal.GetTerminal(msisdn);
        }

        public List<Terminal> GetTerminals()
        {
            return this.terminalDal.GetTerminals();
        }

        public bool InsertNewTerminal(string msisnd, string imoNumber, string description)
        {
            int terminalId = 0;
            Terminal terminal = new Terminal();
            terminal.ActivationTimestamp = CommonUtility.GetCurrentTimestmap();
            terminal.CreationTimestamp = CommonUtility.GetCurrentTimestmap();
            terminal.DeactivationTimestamp = DateTime.MaxValue;
            terminal.Description = description;
            terminal.MSISDN = msisnd;
            terminal.Remark = string.Empty;
            terminal.Status = TerminalStatus.Pending_Activation;
            terminal.IMONumber = imoNumber;

            this.terminalDal.InsertTerminal(terminal, out terminalId);
            if (terminalId != 0)
                return true;
            else
                return false;
        }

        public void UpdateTerminal(Terminal terminal)
        {
            this.terminalDal.UpdateTerminal(terminal);
        }

        public List<ApplicationAccessLevel> GetApplications(Int64 terminalId)
        {
            List<ApplicationAccessLevel> applicationsAccessLevel = new List<ApplicationAccessLevel>();
            Dictionary<string, AccessLevel> applicationIdsAccessLevel = terminalApplicationDal.GetApplicationIdsWithAccessLevel(terminalId);
            foreach (KeyValuePair<string, AccessLevel> pair in applicationIdsAccessLevel)
            {
                applicationsAccessLevel.Add(new ApplicationAccessLevel()
                {
                    AccessLevel = pair.Value,
                    Application = applicationDal.GetApplication(pair.Key)
                });
            }
            return applicationsAccessLevel;
        }

        public GatewayServiceResponse GetTerminalStatus(string msisdn, string applicationId, string accessCode)
        {
            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Get Terminal Status", "msisdn: " + msisdn + " applicationId: " + applicationId + ", accessCode: " + accessCode);
            AccessLevelMgr accessLevelMgr = new AccessLevelMgr();
            try
            {
                GatewayServiceResponse response = accessLevelMgr.CheckAccessLevel(applicationId, accessCode);
                if (response.isSuccessful)
                {
                    Terminal terminal = this.GetTerminal(msisdn);
                    if (terminal == null)
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Get Terminal Status", "Terminal is not found.");
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.TerminalNotFound,
                            message = "Terminal Not Found",
                            isSuccessful = true
                        };
                    }
                    else
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Get Terminal Status", "Terminal is found. Status: " + terminal.Status.ToString());
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Success,
                            message = terminal.Status.ToString(),
                            isSuccessful = true
                        };
                    }
                }
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Get Terminal Status", "Fail in checking access level. gatewayResponse: " + response.gatewayResponse.ToString() + ", message: " + response.message);
                    return response;
                }


            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Exception during GetTerminalStatus.", ex.Message);
                return new GatewayServiceResponse() {
                    gatewayResponse = Response.Fail,
                    message = "Error occured at Gateway. Contact System Admin.",
                    isSuccessful = true
                };

            }
        }

        public GatewayServiceResponse RegisterNewTerminal(string msisdn, string imoNumber, string description, string applicationId, string accessCode)
        {
            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Register New Terminal", "msisdn: " + msisdn + ", imoNumber" + imoNumber + ", applicationId: " + applicationId + ", accessCode: " + accessCode);
            AccessLevelMgr accessLevelMgr = new AccessLevelMgr();
            try
            {
                GatewayServiceResponse response = accessLevelMgr.CheckAccessLevel(applicationId, accessCode);
                if (response.isSuccessful)
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Register New Terminal", "Access level is Ok");
                    if (this.InsertNewTerminal(msisdn, imoNumber, description))
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Register New Terminal", "New terminal inserted.");
                        DataCommandMgr dataCommandMgr = new DataCommandMgr();
                        if (dataCommandMgr.IssueRegistrationCommand(msisdn, imoNumber))
                        {
                            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Register New Terminal", "Registration Command Issued.");
                            return new GatewayServiceResponse()
                            {
                                gatewayResponse = Response.Success,
                                isSuccessful = true,
                                message = string.Empty
                            };
                        }
                        else
                        {
                            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Register New Terminal", "Registration Command NOT Issued.");
                            return new GatewayServiceResponse()
                            {
                                gatewayResponse = Response.Fail,
                                isSuccessful = false,
                                message = "Unable to issue Activation Command. Contact System Admin."
                            };
                        }
                    }
                    else
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Register New Terminal", "New terminal NOT inserted.");
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Fail,
                            isSuccessful = false,
                            message = "Unable to insert New Terminal. Contact System Admin."
                        };
                    }
                }
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Register New Terminal", "Failed at checking access level. GatewayResponse: " + response.gatewayResponse.ToString() + ", message: "+ response.message);
                    return response;
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Exception during RegisterNewTerminal.", ex.Message);
                return new GatewayServiceResponse()
                {
                    gatewayResponse = Response.Fail,
                    message = "Error occured at Gateway. Contact System Admin.",
                    isSuccessful = true
                };

            }
        }

        public GatewayServiceResponse DeregisterTerminal(string msisdn, string applicationId, string accessCode)
        {
            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Deregister Terminal", "msisdn: " + msisdn + ", applicationId: " + applicationId + ", accessCode: " + accessCode);
            AccessLevelMgr accessLevelMgr = new AccessLevelMgr();
            try
            {
                GatewayServiceResponse response = accessLevelMgr.CheckAccessLevel(msisdn, applicationId, accessCode);
                if (response.isSuccessful)
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Deregister Terminal", "Access level is Ok");
                    Terminal terminal = this.GetTerminal(msisdn);
                    if (terminal != null)
                    {
                        DataCommandMgr dataCommandMgr = new DataCommandMgr();
                        if (dataCommandMgr.IssueDeregistrationCommand(msisdn))
                        {
                            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Deregister Terminal", "Deregistration Command Issued.");
                            terminal.Status = TerminalStatus.Pending_Deactivation;
                            terminal.DeactivationTimestamp = CommonUtility.GetCurrentTimestmap();
                            this.UpdateTerminal(terminal);

                            return new GatewayServiceResponse()
                            {
                                gatewayResponse = Response.Success,
                                isSuccessful = true,
                                message = "From Gateway - Deregister Command Submitted."
                            };
                        }
                        else
                        {
                            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Deregister Terminal", "Deregistration Command Not Issued.");
                            return new GatewayServiceResponse()
                            {
                                gatewayResponse = Response.Success,
                                isSuccessful = false,
                                message = "From Gateway - Deregister Command NOT Submitted due to not being able to issue the deregister command"
                            };
                        }
                    }
                    else
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Deregister Terminal", "Terminal not Found with Supplied MSISDN.");
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Success,
                            isSuccessful = false,
                            message = "From Gateway - Terminal not Found with Supplied MSISDN: " + msisdn
                        };
                    }
                }
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Deregister Terminal", "Failed at checking access level. Gatewayresponse: " + response.gatewayResponse.ToString() + ", message: " + response.message);
                    return response;
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Exception during DeregisterTerminal.", ex.Message);
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
