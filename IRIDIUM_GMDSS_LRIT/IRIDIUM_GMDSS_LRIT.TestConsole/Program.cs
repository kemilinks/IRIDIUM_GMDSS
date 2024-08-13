using IRIDIUM_GMDSS_LRIT.Core.Dal;
using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Forwarder.Interface;
using IRIDIUM_GMDSS_LRIT.Core.Mgr;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string _SHORT_CODE = "*993003";
            string _SOURCE = "881631760049";
            int _TEST_IMO_NUMBER = 9902911;
            string _REPORT_HEX = "cc0af04df12031d38b70c000";
            string _COMMAND_HEX = "cc0af0c0971b3f00082d01e0";
            //Random r = new Random();
            ////int offset = 15;
            //int offset = r.Next(1, 15);
            //int timer = 3 * 60;

            //string activation_Command_In_Hex = CommandFactory.Get_Activation_Command_InHex(_TEST_IMO_NUMBER);
            //Console.WriteLine("activation_command_In_Hex: " + activation_Command_In_Hex);

            //string ondemandpoll_Command_In_Hex = CommandFactory.Get_Ondemand_Poll_Command_InHex();
            //Console.WriteLine("ondemandpoll_Command_In_Hex: " + ondemandpoll_Command_In_Hex);

            //string deactivation_Command_In_Hex = CommandFactory.Get_Deactivation_Command_InHex();
            //Console.WriteLine("deactivation_Command_In_Hex: " + deactivation_Command_In_Hex);

            //string stop_Command_In_Hex = CommandFactory.Get_Stop_Command_InHex();
            //Console.WriteLine("stop_Command_In_Hex: " + stop_Command_In_Hex);

            //string change_Interval_Command_In_Hex = CommandFactory.Get_Change_Interval_Command_In_Hex(timer, offset);
            //Console.WriteLine("timer: " + timer + ", offset: " + offset);
            //Console.WriteLine("change_Interval_Command_In_Hex: " + change_Interval_Command_In_Hex);

            //Core.Entity.UI.Report decodedReport = Interpreter.DecodeReportFromTerminal(_REPORT_HEX);
            //IPositionForwarder forwarder = new IRIDIUM_GMDSS_LRIT.Forwarder.Conformance2020.Forwarder();
            //forwarder.EndPoint = "http://192.168.10.175/Conformance2020.Receiver.Service/Conformance2020Service.svc/";
            //forwarder.DoForward(2, "881631760049", "N", Convert.ToInt32(decodedReport.Latitude_Degree), Convert.ToInt32(decodedReport.Latitude_Minute), Convert.ToDouble(decodedReport.Latiitude_MinuteDecimal)
            //                                                     , "W", Convert.ToInt32(decodedReport.Longitude_Degree), Convert.ToInt32(decodedReport.Longitude_Minute), Convert.ToDouble(decodedReport.Longitude_MinuteDecimal)
            //                                                     , DateTime.UtcNow.Year, Convert.ToInt32(decodedReport.Month), Convert.ToInt32(decodedReport.Day)
            //                                                     , Convert.ToInt32(decodedReport.Hour), Convert.ToInt32(decodedReport.Minute), 00, DateTime.UtcNow);
            //Interpreter.DecodeComandFromTerminal(_COMMAND_HEX);

            //DataReportDal reportDal = new DataReportDal();
            //reportDal.InsertDataReport(
            //        new DataReport()
            //        {
            //            Data = _REPORT_HEX,
            //            ReceivedTimestamp = DateTime.UtcNow,
            //            Source = _SOURCE,
            //            Status = ReportStatus.New
            //        }
            //    ); ;

            //DataCommandDal commandDal = new DataCommandDal();
            //commandDal.InsertDataCommand(new DataCommand(){ 
            //     Data = _COMMAND_HEX,
            //     Destination = _SHORT_CODE,
            //      Direction = Direction.Mobile_Originated,
            //       Source = _SOURCE,
            //        Status = CommandStatus.New,
            //         Timestamp = DateTime.UtcNow
            //});

            //DataMgr dataMgr = new DataMgr();
            //dataMgr.ProcessIncomingData("");

            //Console.ReadLine();

            //string combinedSMS = "cc0af0c0971b3f00600781e0cc0af04df12031d3d8740f00";
            ////string combinedSMS = "CC0AF0CD00000000C00F01FF";
            ////int indexOfHeader = combinedSMS.IndexOf("cc");
            //string[] parts = combinedSMS.Split(new string[] { "cc0a" }, StringSplitOptions.None);
            //if (parts.Length != 1)
            //{
            //    for (int i = 0; i < parts.Length; i++)
            //    {
            //        if (!string.IsNullOrEmpty(parts[i]))
            //            Console.WriteLine("cc0a" + parts[i]);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(combinedSMS);
            //}

            DataMgr dataMgr = new DataMgr();
            dataMgr.ProcessIncomingData("DeliverSm received : Sequence: 1295, SourceAddress: 881641707025, Coding: OctetUnspecified, Text: cc0af0c000000000000000e0cc0af047d709e68559501500", "*993003");

            Console.ReadLine();
        }
    }
}
