using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.WcfService;
using IRIDIUM_GMDSS_LRIT.Core.WcfService.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Mgr
{
    public class AccessLevelMgr
    {
        private string _DETAILS_UNABLE_TO_FIND_APPLICATION = "Unable to find Application";
        private string _DETAILS_INACTIVE_APPLICATION = "Application is Inactive";
        private string _DETAILS_INVALID_APPLICATION_TERMINAL_ACCESS_LEVEL = "Terminal and/or Application do not have proper Access Level";
        private string _DETAILS_INVALID_TERMINAL_APPLICATION_ASSIGNMENT = "Terminal is not Assigned To Application";
        private string _DETAILS_INVALID_TERMINAL = "MSISDN is Not Valid";
        private string _DETAILS_INVALID_ACCESS_CODE = "Access Code is Invalid";
        private string _APPLICATION_ID_FOR_MANAGEMENT_PORTAL = "MANAGEMENT_PORTAL";
        private string _ACCESS_CODE_FOR_MANAGEMENT_PORTAL = "!MASTER0808*";

        public AccessLevelMgr()
        {
            
        }

        public  GatewayServiceResponse CheckAccessLevel(string msisdn, string applicationId, string accessCode)
        {
            ApplicationMgr applicationMgr = new ApplicationMgr();
            TerminalMgr terminalMgr = new TerminalMgr();
            if (applicationId.Equals(_APPLICATION_ID_FOR_MANAGEMENT_PORTAL) && accessCode.Equals(_ACCESS_CODE_FOR_MANAGEMENT_PORTAL))
            {
                return new GatewayServiceResponse()
                {
                         gatewayResponse = Response.Success,
                          isSuccessful = true,
                           message= string.Empty
                };
            }

            Application application = applicationMgr.GetApplication(applicationId);
            if (application != null)
            {
                if (application.Active)
                {
                    if (application.AccessCode.Equals(accessCode))
                    {
                        Terminal terminal = terminalMgr.GetTerminal(msisdn);
                        if (terminal != null)
                        {
                            List<ApplicationAccessLevel> applicationsAccessLevel = terminalMgr.GetApplications(terminal.Id);
                            bool isApplicationFound = false;
                            bool doesApplicationHaveProperAccessLevel = false;

                            foreach (ApplicationAccessLevel applicationAccessLevel in applicationsAccessLevel)
                            {
                                if (applicationAccessLevel.Application.Id.Equals(applicationId))
                                {
                                    isApplicationFound = true;
                                    if (applicationAccessLevel.AccessLevel == AccessLevel.SubmissionOnly || applicationAccessLevel.AccessLevel == AccessLevel.AllowAll)
                                        doesApplicationHaveProperAccessLevel = true;
                                    break;
                                }
                            }

                            if (isApplicationFound)
                            {
                                if (doesApplicationHaveProperAccessLevel)
                                {
                                    return new GatewayServiceResponse()
                                    {
                                        gatewayResponse = Response.Success,
                                        isSuccessful = true,
                                         message = string.Empty
                                    };
                                }
                                else
                                {
                                    return new GatewayServiceResponse()
                                    {
                                        gatewayResponse = Response.Fail,
                                        message = _DETAILS_INVALID_APPLICATION_TERMINAL_ACCESS_LEVEL,
                                        isSuccessful = false
                                    };
                                }
                            }
                            else
                            {
                                return new GatewayServiceResponse()
                                {
                                    gatewayResponse = Response.Fail,
                                    message = _DETAILS_INVALID_TERMINAL_APPLICATION_ASSIGNMENT,
                                    isSuccessful = false
                                };
                            }
                        }
                        else
                        {
                            return new GatewayServiceResponse()
                            {
                                gatewayResponse = Response.Fail,
                                message = _DETAILS_INVALID_TERMINAL,
                                isSuccessful = false
                            };
                        }
                    }
                    else
                    {
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Fail,
                            message = _DETAILS_INVALID_ACCESS_CODE,
                            isSuccessful = false
                        };
                    }
                }
                else
                {
                    return new GatewayServiceResponse()
                    {
                        gatewayResponse = Response.Fail,
                        message = _DETAILS_INACTIVE_APPLICATION,
                        isSuccessful = false
                    };
                }
            }
            else
            {
                return new GatewayServiceResponse()
                {
                    gatewayResponse = Response.Fail,
                    message = _DETAILS_UNABLE_TO_FIND_APPLICATION,
                    isSuccessful = false
                };
            }
        }

        public GatewayServiceResponse CheckAccessLevel(string applicationId, string accessCode)
        {
            ApplicationMgr applicationMgr = new ApplicationMgr();
            TerminalMgr terminalMgr = new TerminalMgr();
            if (applicationId.Equals(_APPLICATION_ID_FOR_MANAGEMENT_PORTAL) && accessCode.Equals(_ACCESS_CODE_FOR_MANAGEMENT_PORTAL))
            {
                return new GatewayServiceResponse()
                {
                    gatewayResponse = Response.Success,
                    isSuccessful = true,
                    message = string.Empty
                };
            }

            Application application = applicationMgr.GetApplication(applicationId);
            if (application != null)
            {
                if (application.Active)
                {
                    if (application.AccessCode.Equals(accessCode))
                    {
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Success,
                            isSuccessful = true,
                            message = string.Empty
                        };
                    }
                    else
                    {
                        return new GatewayServiceResponse()
                        {
                            gatewayResponse = Response.Fail,
                            message = _DETAILS_INVALID_ACCESS_CODE,
                            isSuccessful = false
                        };
                    }
                }
                else
                {
                    return new GatewayServiceResponse()
                    {
                        gatewayResponse = Response.Fail,
                        message = _DETAILS_INACTIVE_APPLICATION,
                        isSuccessful = false
                    };
                }
            }
            else
            {
                return new GatewayServiceResponse()
                {
                    gatewayResponse = Response.Fail,
                    message = _DETAILS_UNABLE_TO_FIND_APPLICATION,
                    isSuccessful = false
                };
            }
        }
    }
}
