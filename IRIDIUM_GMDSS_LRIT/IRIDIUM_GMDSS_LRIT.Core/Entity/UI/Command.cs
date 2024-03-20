using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Entity.UI
{
    public class Command
    {
        public string Id { get; set; }
        public string Direction { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }
        public string Raw { get; set; }
        public string H_Header { get; set; }
        public string M_Message { get; set; }
        public string P_PollPosition { get; set; }
        public string SI_SetIMO { get; set; }
        public string SO_SetOffset { get; set; }
        public string ST_SetTimer { get; set; }
        public string SD_SetReduced { get; set; }
        public string A_Ack { get; set; }
        public string I_IMONumber { get; set; }
        public string R_2BitReserved { get; set; }
        public string O_Offset { get; set; }
        public string T_Timer { get; set; }
        public string D_Reduced { get; set; }
        public string R_3BitReserved { get; set; }
        public string SN_SetResponsible { get; set; }
        public string N_Responsible { get; set; }
        public string V_Version { get; set; }
    }
}
