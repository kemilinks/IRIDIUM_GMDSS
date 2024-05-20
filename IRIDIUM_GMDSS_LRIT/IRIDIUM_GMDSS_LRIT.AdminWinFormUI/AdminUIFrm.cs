using IRIDIUM_GMDSS_LRIT.Core.ProcessingTask;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRIDIUM_GMDSS_LRIT.AdminWinFormUI
{
    public partial class AdminWinFormUIFrm : Form
    {
        public AdminWinFormUIFrm()
        {
            InitializeComponent();
        }

        private void btnStartForwarder_Click(object sender, EventArgs e)
        {
            PositionForwaderTask positionForwarderTask = new PositionForwaderTask();
            positionForwarderTask.SleepIntervalInSeconds = 30;
            positionForwarderTask.StartTask();

            richTxtOutput.AppendText("Position Forwarder started at: " + CommonUtility.ConvertDateTimeToString(CommonUtility.GetCurrentTimestmap(), CommonUtility._SYSTEM_DATE_TIME_FORMAT) + Environment.NewLine);
            btnStartForwarder.Enabled = false;
        }

        private void btnStartSMMPClientMonitor_Click(object sender, EventArgs e)
        {
            MonitorSmmpClientTask monitorSmmpClientTask = new MonitorSmmpClientTask();
            monitorSmmpClientTask.SleepIntervalInSeconds = 30;
            monitorSmmpClientTask.StartTask();
            richTxtOutput.AppendText("SMMP Client Monitoring started at: " + CommonUtility.ConvertDateTimeToString(CommonUtility.GetCurrentTimestmap(), CommonUtility._SYSTEM_DATE_TIME_FORMAT) + Environment.NewLine);
            btnStartSMMPClientMonitor.Enabled = false;
        }
    }
}
