using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Forwarder.Interface
{
    public interface IPositionForwarder
    {
        string EndPoint { get; set; }
        string ForwardResult { get; }

        bool DoForward(Int64 reportId, string msisdn, string Latitude_Hemisphere, int latitudeDegree, int latitudeMinute, double latitudeMinuteDecimal, string Longitude_Hemisphere, int longitudeDegree, int longitudeMinute, double longitudeMinuteDecimal, int year, int month, int day, int hour, int minute, int second, DateTime gatewayReceiveTimestamp);
    }
}
