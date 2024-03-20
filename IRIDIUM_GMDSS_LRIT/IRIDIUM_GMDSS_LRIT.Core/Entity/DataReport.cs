using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Entity
{
    public  class DataReport
    {
        public Int64 Id { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
        public ReportStatus Status { get; set; }
        public string Remarks { get; set; }
        public DateTime ReceivedTimestamp { get; set; }
    }

    public enum ReportStatus
    { 
        New = 0,
        Processed = 1,
        NoNeedToProcess = 2,
    }
}
