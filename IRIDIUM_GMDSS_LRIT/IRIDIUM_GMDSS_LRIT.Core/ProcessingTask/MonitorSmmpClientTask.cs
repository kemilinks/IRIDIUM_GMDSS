using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using KemilinksNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.ProcessingTask
{
    public class MonitorSmmpClientTask : KemiTask
    {
        private int _DELAY_IN_MINUTES_BEFORE_CHECKING_LAST_ENQUIRED_LINK_RECEIVED = 2;
        private int _ALLOWED_OFFSET_IN_MINUTES_BEFORE_CONSIDERED_LINK_IS_DOWN = 2;
        private string _SYSTEM_ADMIN_NUMBER = string.Empty;
        private SystemConfigUtility sysConfigUtility;

        public MonitorSmmpClientTask()
        {
            this.sysConfigUtility = new SystemConfigUtility();
            this._SYSTEM_ADMIN_NUMBER = this.sysConfigUtility.GetSystemAdminNumber();
        }

        protected override void DoTask()
        {
            Process[] smmpClientProcess = Process.GetProcessesByName("IRIDIUM.GMDSS_LRIT.SmppClient");
            if (smmpClientProcess.Length != 0)
            {
                DateTime currentTimestamp = CommonUtility.GetCurrentTimestmap();
                if (currentTimestamp.Subtract(this.sysConfigUtility.GetLastSmppServerConnectionDateTime()).TotalMinutes >= _DELAY_IN_MINUTES_BEFORE_CHECKING_LAST_ENQUIRED_LINK_RECEIVED)
                {
                    DateTime lastEnquireLinkReceivedDateTime = this.sysConfigUtility.GetLastEnquireLinkReceivedDateTime();
                    if (currentTimestamp.Subtract(lastEnquireLinkReceivedDateTime).TotalMinutes >= _ALLOWED_OFFSET_IN_MINUTES_BEFORE_CONSIDERED_LINK_IS_DOWN)
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Last Enquire Link Receive DateTime is outdate. Terminate SMMP Client and Restart it Now", string.Empty);
                        this.SendSmsNotificationToAdministrator(this._SYSTEM_ADMIN_NUMBER, "Last Enquire Link Reveived DateTime is outdate. Restart Client Now.");

                        if (smmpClientProcess.Length == 1)
                            smmpClientProcess[0].Kill();

                        System.Threading.Thread.Sleep(2000);

                        ProcessStartInfo start = new ProcessStartInfo();
                        start.FileName = @"E:\Webdata\Iridium_GMDSS_LRIT\IRIDIUM.GMDSS_LRIT.SmppClient\IRIDIUM.GMDSS_LRIT.SmppClient.exe";
                        Process.Start(start);
                    }
                }
            }

        }

        protected override void EndTask()
        { 
        
        }

        private void SendSmsNotificationToAdministrator(string SMS_NUMBER, string smsContent)
        {
            try
            {
                KemilinksNotification.CommzGateSMS sms = new CommzGateSMS();
                string result = sms.SendSMS(SMS_NUMBER, smsContent, "Iridium_Gmdss_Smpp_Client");
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to send Sms to System Admin", ex.Message);

            }
        }
    }
}
