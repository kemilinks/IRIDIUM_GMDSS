using IRIDIUM_GMDSS_LRIT.Core.Dal;
using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Mgr
{
    public class DataMgr
    {
        private const string _SMS_HEADER_LENGTH = "cc0a";
        private const int _SINGLE_DATA_FIXED_LENGTH = 24;
        private DataReportMgr dataReportMgr;
        public DataMgr()
        {
            this.dataReportMgr = new DataReportMgr();
        }

        public void ProcessIncomingData(string incomingData, string lritShortCode)
        {
            //Task.Factory.StartNew(() =>
            //{
                Process(incomingData, lritShortCode);
            //});
        }

        private void Process(string incomingData, string lritShortCode)
        {
            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Incoming Data In Data Manager", "incoming Data: " + incomingData);

            string[] parts = incomingData.Split(',');
            if (parts.Length == 4)
            {
                string sourcePart = parts[1];
                string dataPart = parts[3];
                string source = sourcePart.Split(':')[1].Trim();
                string data = dataPart.Split(':')[1].Trim();

                if (!string.IsNullOrEmpty(data))
                {
                    string[] dataParts = data.Split(new string[] { _SMS_HEADER_LENGTH }, StringSplitOptions.None);
                    if (dataParts.Length != 1)
                    {
                        for (int i = 0; i < dataParts.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(dataParts[i]))
                            {
                                string singleData = _SMS_HEADER_LENGTH + dataParts[i];
                                ProcessSingleData(source,singleData, lritShortCode);
                            }

                        }
                    }
                    else
                        ProcessSingleData(source,incomingData, lritShortCode);
                }
                else
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.WARNING, "Empty Incoming Data Value In Data Manager", "Stopped processing.");

                
            }
            else
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "ProcessIncomingData", "4-Part-Data is expected. Invalid Data: " + incomingData);

           
        }

        private void ProcessSingleData(string source, string data, string lritShortCode)
        {
            if (data.Length == _SINGLE_DATA_FIXED_LENGTH)
            {
                if (Interpreter.IsReport(data))
                    ProcessIncomingDataReport(source, data);
                else
                    ProcessIncomingDataCommand(source, data, lritShortCode);
            }
            else
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "ProcessSingleData" , "Data from: " + source + " has invalid lenght. Expected 24 char data. Invalid data: " + data);
        }

        private void ProcessIncomingDataReport(string source, string data)
        {
            TerminalMgr terminalMgr = new TerminalMgr();
            Terminal terminal = terminalMgr.GetTerminal(source);
            if (terminal != null)
            {
                if (terminal.Status == TerminalStatus.Pending_Activation)
                {
                    Entity.UI.Report decodedReport = Interpreter.DecodeReportFromTerminal(data);
                    if (decodedReport.ThraneReportEvent == Entity.UI.ThraneReportEvent.PolledPosition)
                    {
                        terminal.Status = TerminalStatus.Active;
                        terminalMgr.UpdateTerminal(terminal);

                        DataReport report = new DataReport();
                        report.Data = data.ToUpper();
                        report.ReceivedTimestamp = DateTime.UtcNow;
                        report.Source = source;
                        report.Status = ReportStatus.New;
                        report.Remarks = string.Empty;

                        this.dataReportMgr.InsertDataReport(report);
                    }
                    else
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, decodedReport.ThraneReportEvent.ToString() + " Data Report is expected from the pending activation source. But other come. Check!!!", "Source: " + source + ", data: " + data);
                }
                else if (terminal.Status == TerminalStatus.Active)
                {
                    DataReport report = new DataReport();
                    report.Data = data.ToUpper();
                    report.ReceivedTimestamp = DateTime.UtcNow;
                    report.Source = source;
                    report.Status = ReportStatus.New;
                    report.Remarks = string.Empty;

                    this.dataReportMgr.InsertDataReport(report);
                }
                else if (terminal.Status == TerminalStatus.Pending_Deactivation)
                {
                    Entity.UI.Report decodedReport = Interpreter.DecodeReportFromTerminal(data);
                    if (decodedReport.ThraneReportEvent == Entity.UI.ThraneReportEvent.ResponsibilityLost)
                    {
                        terminal.Status = TerminalStatus.Deactive;
                        terminalMgr.UpdateTerminal(terminal);

                        DataReport report = new DataReport();
                        report.Data = data.ToUpper();
                        report.ReceivedTimestamp = DateTime.UtcNow;
                        report.Source = source;
                        report.Status = ReportStatus.New;
                        report.Remarks = string.Empty;

                        this.dataReportMgr.InsertDataReport(report);

                    }
                    else
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Resposibily Lost Data Report is expected from a pending deactive source, but other comes. Check!!!", "Source: " + source + ", data: " + data);
                }
                else if (terminal.Status == TerminalStatus.Deactive)
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Data Report comes from an deactived source.", "Source: " + source + ", data: " + data);
            }
            else
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Data Report comes from an unknow source.", "Source: " + source + ", data: " + data);
        }

        private void ProcessIncomingDataCommand(string source, string data, string lritShortCode)
        {
            DataCommand command = new DataCommand();
            command.Data = data.ToUpper();
            command.Destination = lritShortCode;
            command.Direction = Direction.Mobile_Originated;
            command.Source = source;
            command.Status = CommandStatus.Received;
            command.Timestamp = DateTime.UtcNow;
            command.ReferenceNumber = string.Empty;
            command.Type = DataCommandType.TerminalConfig.ToString();

            DataCommandDal dataCommandDal = new DataCommandDal();
            dataCommandDal.InsertDataCommand(command);
        }
    }
}
