using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Entity.UI;
using IRIDIUM_GMDSS_LRIT.Core.Mgr;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IRIDIUM_GMDSS_LRIT.AdminWebUI
{
    public partial class DataReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string source = txtSource.Text;
            DateTime from = DateTime.MinValue;
            DateTime to = DateTime.MaxValue;
            DateTime now = DateTime.UtcNow;
            int lastPeriod = Convert.ToInt32(ddlPeriod.SelectedValue);
            if (lastPeriod != -1)
            {
                to = now;
                from = to.AddMinutes(-1 * lastPeriod);
            }
            else
            {
                to = now;
                from = SqlDateTime.MinValue.Value;
            }

            DataReportMgr dataReportMgr = new DataReportMgr();
            List<IRIDIUM_GMDSS_LRIT.Core.Entity.DataReport> dataReports = dataReportMgr.GetDataReports(source, from, to);
            dataReports = dataReports.OrderByDescending(dataReport => dataReport.ReceivedTimestamp).ToList<IRIDIUM_GMDSS_LRIT.Core.Entity.DataReport>();
            List<Report> reports = new List<Report>();
            dataReports.ForEach(dataReport => {
                Report report = Interpreter.DecodeReportFromTerminal(dataReport.Data);
                report.Id = dataReport.Id.ToString();
                report.Raw = dataReport.Data;
                report.ReceiveTimestamp = dataReport.ReceivedTimestamp;
                reports.Add(report);
            });

            gvDataReport.DataSource = reports;
            gvDataReport.DataBind();
            
        }
    }
}