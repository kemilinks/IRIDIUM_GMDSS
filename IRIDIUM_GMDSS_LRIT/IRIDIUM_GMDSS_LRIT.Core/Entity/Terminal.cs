using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Entity
{
    public class Terminal
    {
        public int Id { get; set; }
        public string MSISDN { get; set; }
        public string IMONumber { get; set; }
        public TerminalStatus Status { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public DateTime ActivationTimestamp { get; set; }
        public DateTime DeactivationTimestamp { get; set; }

    }

    public enum TerminalStatus
    { 
        Created = 0,
        Pending_Activation = 1,
        Active = 2,
        Deactive = 3,
        Pending_Deactivation = 4,
        Unknown = 5
    }
}
