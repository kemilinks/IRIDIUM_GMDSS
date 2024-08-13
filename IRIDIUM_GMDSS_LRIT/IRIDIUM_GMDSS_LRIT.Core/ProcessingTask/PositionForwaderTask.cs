using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Forwarder.Interface;
using IRIDIUM_GMDSS_LRIT.Core.Mgr;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using IRIDIUM_GMDSS_LRIT.Core.WcfService.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.ProcessingTask
{
    public class PositionForwaderTask : KemiTask
    {
        private DataReportMgr dataReportMgr;
        private TerminalMgr terminalMgr;
        private ApplicationMgr applicationMgr;
        public PositionForwaderTask()
        {
            this.dataReportMgr = new DataReportMgr();
            this.terminalMgr = new TerminalMgr();
            this.applicationMgr = new ApplicationMgr();
        }

        protected override void DoTask()
        {
            List<DataReport> reports = this.dataReportMgr.GetDataReports(ReportStatus.New);
            reports = reports.OrderBy(report => report.ReceivedTimestamp).ToList<DataReport>();
            foreach (DataReport report in reports)
            {
                Console.WriteLine("Process Data Report Id: " + report.Id);
                Terminal terminal = this.terminalMgr.GetTerminal(report.Source);
                if (terminal != null)
                {
                    List<ApplicationAccessLevel> applications = this.terminalMgr.GetApplications(terminal.Id);
                    foreach (ApplicationAccessLevel applicationWithAccessLevel in applications)
                    {
                        if (applicationWithAccessLevel.AccessLevel == AccessLevel.AllowAll)
                        {
                            IPositionForwarder forwarder = this.applicationMgr.GetApplicationForwarder(applicationWithAccessLevel.Application.Id);
                            Entity.UI.Report decodedReport = Interpreter.DecodeReportFromTerminal(report.Data);
                            if (forwarder.DoForward(report.Id, report.Source, decodedReport.Latitude_Hemisphere, Convert.ToInt32(decodedReport.Latitude_Degree), Convert.ToInt32(decodedReport.Latitude_Minute), Convert.ToDouble(decodedReport.Latiitude_MinuteDecimal)
                                                                 , decodedReport.Longitude_Hemisphere, Convert.ToInt32(decodedReport.Longitude_Degree), Convert.ToInt32(decodedReport.Longitude_Minute), Convert.ToDouble(decodedReport.Longitude_MinuteDecimal)
                                                                 , DateTime.UtcNow.Year, Convert.ToInt32(decodedReport.Month), Convert.ToInt32(decodedReport.Day)
                                                                 , Convert.ToInt32(decodedReport.Hour), Convert.ToInt32(decodedReport.Minute), 00, report.ReceivedTimestamp))
                            {
                                string logLine  = "Delivered to " + applicationWithAccessLevel.Application.Id + " - ";
                                Console.WriteLine(logLine);
                                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Forwarder", logLine);
                                report.Remarks += logLine;
                                this.dataReportMgr.UpdateDataReport(report);
                            }
                            else
                            {
                                string logLine = "Not Delivered to " + applicationWithAccessLevel.Application.Id + " due to " + forwarder.ForwardResult + " - ";
                                Console.WriteLine(logLine);
                                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Forwarder", logLine);
                                report.Remarks += logLine;
                                this.dataReportMgr.UpdateDataReport(report);
                            }
                        }
                    }
                    report.Status = ReportStatus.Processed;
                    this.dataReportMgr.UpdateDataReport(report);
                }
                else
                {
                    report.Status = ReportStatus.Processed;
                    string logLine = "Failed to process as terminal is not found with Terminal MSISDN: " + report.Source;
                    Console.WriteLine(logLine);
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Forwarder", logLine);
                    report.Remarks += logLine;
                    this.dataReportMgr.UpdateDataReport(report);
                }
            }
        }

        protected override void EndTask()
        { 
        
        }
    }
}
