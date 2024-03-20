using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Entity
{
    internal class Command
    {
        internal int H_Header { get; }
        internal int M_Message { get; }
        internal bool P_PollPosition { get; set; }
        internal bool SI_SetIMO { get; set; }
        internal bool SO_SetOffset { get; set; }
        internal bool ST_SetTimer { get; set; }
        internal bool SD_SetReduced { get; set; }
        internal bool A_Ack { get; set; }
        internal int I_IMONumber { get; set; }
        internal int R_2BitReserved { get; }
        internal int O_Offset { get; set; }
        internal int T_Timer { get; set; }
        internal bool D_Reduced { get; set; }
        internal int R_3BitReserved { get; }
        internal bool SN_SetResponsible { get; set; }
        internal int N_Responsible { get; set; }
        internal int V_Version { get; }

        internal Command()
        {
            H_Header = 240; //0xF0
            M_Message = 3; // 0x3
            P_PollPosition = false;
            SI_SetIMO = false;
            SO_SetOffset = false;
            ST_SetTimer = false;
            SD_SetReduced = false;
            A_Ack = false;
            I_IMONumber = 0;
            R_2BitReserved = 0;
            O_Offset = 0;
            T_Timer = 0;
            D_Reduced = false;
            R_3BitReserved = 0;
            SN_SetResponsible = false;
            N_Responsible = 0;
            V_Version = 255; // 0xFF

        }
    }
}
