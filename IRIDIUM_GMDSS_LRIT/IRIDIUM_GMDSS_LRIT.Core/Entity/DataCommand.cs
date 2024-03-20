using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Entity
{
    public class DataCommand
    {
        public Int64 Id { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Data { get; set; }
        public string ReferenceNumber { get; set; }
        public Direction Direction { get; set; }
        public CommandStatus Status { get; set; }
        public DateTime Timestamp { get; set; }

        public DataCommand()
        {
            this.Type = DataCommandType.Unknown.ToString();
        }
    }

    public enum Direction
    { 
        Mobile_Originated = 0,
        Mobile_Terminated = 1
    }

    public enum CommandStatus
    {
        New = 0,
        Sent = 1,
        Received = 2,
        FailedToSend= 3
    }

    public enum DataCommandType
    { 
        Registration = 0,
        OndemandPoll = 1,
        ChangeInterval = 2,
        StopReporting = 3,
        Deregistration = 4,
        Unknown = 5,
        TerminalConfig = 6

    }
}
