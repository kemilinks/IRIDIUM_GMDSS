using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IRIDIUM_GMDSS_LRIT.Core.Entity.UI.EventCode;

namespace IRIDIUM_GMDSS_LRIT.Core.Entity.UI
{
    public class Report
    {
        public string Id { get; set; }
        public DateTime ReceiveTimestamp { get; set; }
        public string Raw { get; set; }
        public string Header { get; set; }
        public string Message { get; set; }
        public string Latitude_Hemisphere { get; set; }
        public string Latitude_Degree { get; set; }
        public string Latitude_Minute { get; set; }
        public string Latiitude_MinuteDecimal { get; set; }
        public string Longitude_Hemisphere { get; set; }
        public string Longitude_Degree { get; set; }
        public string Longitude_Minute { get; set; }
        public string Longitude_MinuteDecimal { get; set; }
        public string Event { get; set; }
        public ThraneReportEvent ThraneReportEvent { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Reserved { get; set; }
    }
}
