using IRIDIUM_GMDSS_LRIT.Core.Entity.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Utility
{
    public class Interpreter
    {
        public static Report DecodeReportFromTerminal(string reportInHex)
        {
            string trimmedReportInHex = reportInHex.Substring(4); //trim away CC0A header of SMS 

            string binarystring = Utility.Converter.FromHexStringToBits(trimmedReportInHex);

            string header_Bits = binarystring.Substring(0, 8);
            string message_Bits = binarystring.Substring(8, 2);
            string latitude_Hemisphere_Bits = binarystring.Substring(10, 1);
            string latitude_Degrees_Bits = binarystring.Substring(11, 7);
            string latitude_Minutes_Bits = binarystring.Substring(18,6);
            string latitude_DecimalMinutes_Bits = binarystring.Substring(24, 5);
            string longitude_Hemisphere_Bits = binarystring.Substring(29,1);
            string longitude_Degrees_Bits = binarystring.Substring(30,8);
            string longitude_Minutes_Bits = binarystring.Substring(38, 6);
            string longitude_DecimalMinutes_Bits = binarystring.Substring(44, 5);
            string event_Code_Bits = binarystring.Substring(49, 7);
            string month_Bits = binarystring.Substring(56, 1);
            string day_Bits = binarystring.Substring(57, 5);
            string hour_Bits = binarystring.Substring(62, 5);
            string minute_Bits = binarystring.Substring(67, 5);
            string reserved_Bits = binarystring.Substring(72, 8);

            Report report = new Report();
            report.Header = "0X" + Utility.Converter.FromBitsToHexString(header_Bits);
            report.Message = Utility.Converter.FromBitsToInteger(message_Bits).ToString() ;

            int latitude_Hemisphere = Utility.Converter.FromBitsToInteger(latitude_Hemisphere_Bits);
            if (latitude_Hemisphere == 0)
                report.Latitude_Hemisphere = "N";
            else if (latitude_Hemisphere == 1)
                report.Latitude_Hemisphere = "S";
            else
                report.Latitude_Hemisphere = "Unknown (Error In Decoding)";
            report.Latitude_Degree = Utility.Converter.FromBitsToInteger(latitude_Degrees_Bits).ToString();
            report.Latitude_Minute = Utility.Converter.FromBitsToInteger(latitude_Minutes_Bits).ToString();
            report.Latiitude_MinuteDecimal = (Utility.Converter.FromBitsToInteger(latitude_DecimalMinutes_Bits) * 0.04).ToString();

            int longitude_Hemisphere = Utility.Converter.FromBitsToInteger(longitude_Hemisphere_Bits);
            if (longitude_Hemisphere == 0)
                report.Longitude_Hemisphere = "E";
            else if (longitude_Hemisphere == 1)
                report.Longitude_Hemisphere = "W";
            else
                report.Longitude_Hemisphere = "Unknown (Error In Decoding)";
            report.Longitude_Degree = Utility.Converter.FromBitsToInteger(longitude_Degrees_Bits).ToString();
            report.Longitude_Minute = Utility.Converter.FromBitsToInteger(longitude_Minutes_Bits).ToString();
            report.Longitude_MinuteDecimal = (Utility.Converter.FromBitsToInteger(longitude_DecimalMinutes_Bits) * 0.04).ToString();

            report.Event = EventCode.GetEventCode(Utility.Converter.FromBitsToInteger(event_Code_Bits));
            report.ThraneReportEvent = EventCode.GetThraneReportEvent(Utility.Converter.FromBitsToInteger(event_Code_Bits));

            int month = Utility.Converter.FromBitsToInteger(month_Bits);
            if (month == 0)
                report.Month = DateTime.UtcNow.Month.ToString();
            else
                report.Month = "Unknown (Error In Decoding or Contact Thrane)";
            report.Day = Utility.Converter.FromBitsToInteger(day_Bits).ToString();
            report.Hour = Utility.Converter.FromBitsToInteger(hour_Bits).ToString();
            report.Minute = Utility.Converter.FromBitsToInteger(minute_Bits).ToString();

            report.Reserved = "0X" +  Utility.Converter.FromBitsToHexString(reserved_Bits);
            
            return report;
        }

        public static Command DecodeComandFromTerminal(string commandInHex)
        {
            string trimmedCommandInHex = commandInHex.Substring(4); //trim away CC0A header of SMS 

            string binarystring = Utility.Converter.FromHexStringToBits(trimmedCommandInHex);

            string header_Bits = binarystring.Substring(0, 8);
            string message_Bits = binarystring.Substring(8,2);
            string pollPosition_Bits = binarystring.Substring(10,1);
            string setIMO_Bits = binarystring.Substring(11,1);
            string setOffset_Bits = binarystring.Substring(12,1);
            string setTimer_Bits = binarystring.Substring(13,1);
            string setReduced_Bits = binarystring.Substring(14,1);
            string ack_Bits = binarystring.Substring(15,1);
            string imo_Bits = binarystring.Substring(16,24);
            string _2bitsReserved_Bits = binarystring.Substring(40,2);
            string offset_Bits = binarystring.Substring(42,11);
            string timer_Bits = binarystring.Substring(53, 13);
            string reduced_Bits = binarystring.Substring(66,1);
            string _3bitsReserved_Bits = binarystring.Substring(67,3);
            string setResponsible_Bits = binarystring.Substring(70,1);
            string responsible_Bits = binarystring.Substring(71,1);
            string version_Bits = binarystring.Substring(72,8);


            Command command = new Command();
            command.H_Header = "0X" + Utility.Converter.FromBitsToHexString(header_Bits);
            command.M_Message = Utility.Converter.FromBitsToInteger(message_Bits).ToString();
            command.P_PollPosition = Utility.Converter.FromBitsToInteger(pollPosition_Bits).ToString();
            command.SI_SetIMO = Utility.Converter.FromBitsToInteger(setIMO_Bits).ToString();
            command.SO_SetOffset = Utility.Converter.FromBitsToInteger(setOffset_Bits).ToString();
            command.ST_SetTimer = Utility.Converter.FromBitsToInteger(setTimer_Bits).ToString();
            command.SD_SetReduced = Utility.Converter.FromBitsToInteger(setReduced_Bits).ToString();
            command.A_Ack = Utility.Converter.FromBitsToInteger(ack_Bits).ToString();
            command.I_IMONumber = Utility.Converter.FromBitsToInteger(imo_Bits).ToString();
            command.R_2BitReserved = Utility.Converter.FromBitsToInteger(_2bitsReserved_Bits).ToString("X2");
            command.O_Offset = Utility.Converter.FromBitsToInteger(offset_Bits).ToString();
            command.T_Timer = Utility.Converter.FromBitsToInteger(timer_Bits).ToString();
            command.D_Reduced = Utility.Converter.FromBitsToInteger(reduced_Bits).ToString();
            command.R_3BitReserved = Utility.Converter.FromBitsToInteger(_3bitsReserved_Bits).ToString("X3");
            command.SN_SetResponsible = Utility.Converter.FromBitsToInteger(setResponsible_Bits).ToString();
            command.N_Responsible = Utility.Converter.FromBitsToInteger(responsible_Bits).ToString();
            command.V_Version = "0X" + Utility.Converter.FromBitsToHexString(version_Bits);

            return command;



        }

        public static bool IsReport(string data)
        {
            string trimmedData = data.Substring(4); //trim away CC0A header of SMS 

            string binarystring = Utility.Converter.FromHexStringToBits(trimmedData);
            string message_Bits = binarystring.Substring(8, 2);
            int message = Utility.Converter.FromBitsToInteger(message_Bits);
            if (message == 1)
                return true;
            else
                return false;
        }
    }
}
