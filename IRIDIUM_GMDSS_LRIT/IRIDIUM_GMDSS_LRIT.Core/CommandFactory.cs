using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Entity
{
    public class CommandFactory
    {
        private const string _SMS_HEADER = "CC0A";

        public static string Get_Activation_Command_InHex(int imoNumber) // A and P set
        {
            Command command = new Command();
            command.I_IMONumber = imoNumber;
            command.SN_SetResponsible = true;
            command.N_Responsible = 1;
            command.SI_SetIMO = true;
            command.A_Ack = true;
            command.P_PollPosition = true;
            command.O_Offset = 0;
            command.T_Timer = 0;
            command.D_Reduced = false;

            string smsContent = _SMS_HEADER + GenerateCommandInHex(command);
            return smsContent;
        }

        public static string Get_Deactivation_Command_InHex() //A and P set
        {
            Command command = new Command();
            command.SN_SetResponsible = true;
            command.N_Responsible = 0;
            command.P_PollPosition = true;
            command.A_Ack = true;

            string smsContent = _SMS_HEADER + GenerateCommandInHex(command);
            return smsContent;
        }

        public static string Get_Ondemand_Poll_Command_InHex()// P set
        {
            Command command = new Command();
            command.P_PollPosition = true;
            command.N_Responsible = 1;

            string smsContent = _SMS_HEADER + GenerateCommandInHex(command);
            return smsContent;
        }

        public static string Get_Change_Interval_Command_In_Hex(int timer, int offset) //A and P set
        {
            Command command = new Command();
            command.P_PollPosition = true;
            command.A_Ack = true;
            command.ST_SetTimer = true;
            command.T_Timer = timer;
            command.SO_SetOffset = true;
            command.O_Offset = offset;
            command.N_Responsible = 1;

            string smsContent = _SMS_HEADER + GenerateCommandInHex(command);
            return smsContent;
        }

        public static string Get_Stop_Command_InHex() //A and P set
        {
            Command command = new Command();
            command.ST_SetTimer = true;
            command.T_Timer = 0;
            command.SO_SetOffset = true;
            command.O_Offset = 0;
            command.A_Ack = true;
            command.P_PollPosition = true;
            command.SD_SetReduced = false;
            command.D_Reduced = false;
            command.N_Responsible = 1;

            string smsContent = _SMS_HEADER + GenerateCommandInHex(command);
            return smsContent;
        }

        private static string GenerateCommandInHex(Command command)
        {
            string headerInBits = Convert.ToString(command.H_Header, 2).PadLeft(8, '0'); //8 bits
            string messageInBits = Convert.ToString(command.M_Message, 2).PadLeft(2, '0'); //2 bits
            string pollPositionInBits = command.P_PollPosition ? "1" : "0"; //1 bit
            string setIMOInBits = command.SI_SetIMO ? "1" : "0"; //1 bit
            string setOffsetInBits = command.SO_SetOffset ? "1" : "0"; //1 bit
            string setTimerInBits = command.ST_SetTimer ? "1" : "0"; //1 bit
            string setReducedInBits = command.SD_SetReduced ? "1" : "0"; //1 bit
            string ackInBits = command.A_Ack ? "1" : "0"; //1 bit
            string imoNumberInBits = Convert.ToString(command.I_IMONumber, 2).PadLeft(24, '0'); //24 bits
            string _2BitReservedInBits = Convert.ToString(command.R_2BitReserved, 2).PadLeft(2, '0'); //2 bits
            string offsetInBits = Convert.ToString(command.O_Offset, 2).PadLeft(11, '0'); //11 bits
            string timerInBits = Convert.ToString(command.T_Timer, 2).PadLeft(13, '0'); //13 bits
            string reducedInBits = command.D_Reduced ? "1" : "0"; //1 bit
            string _3BitReservedInBits = Convert.ToString(command.R_3BitReserved, 2).PadLeft(3, '0'); //3 bits
            string setResponsibleInBits = command.SN_SetResponsible ? "1" : "0"; //1 bit
            string responsibleInBits = Convert.ToString(command.N_Responsible, 2).PadLeft(1, '0'); //1 bits
            string versionInBits = Convert.ToString(command.V_Version, 2).PadLeft(1, '0'); //8 bits

            string commandInBinary = headerInBits + messageInBits + pollPositionInBits + setIMOInBits + setOffsetInBits + setTimerInBits + setReducedInBits + ackInBits + imoNumberInBits + _2BitReservedInBits + offsetInBits + timerInBits + reducedInBits + _3BitReservedInBits + setResponsibleInBits + responsibleInBits + versionInBits;
            string commandInHex = Utility.Converter.FromBitsToHexString(commandInBinary);
            return commandInHex;
        }

    }
}
