using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Entity.UI
{
    internal class EventCode
    {
        public static string GetEventCode(int value)
        {
            if (value == 88)
                return value + " - Polled Position";
            else if (value == 11)
                return value + " - Interval Position";
            else if (value == 15)
                return value + " - IMO Invalid";
            else if (value == 64)
                return value + " - Power On";
            else if (value == 65)
                return value + " - Power Off";
            else if (value == 68)
                return value + " - Antenna Disconnect";
            else if (value == 69)
                return value + " - Iridium Signal Loss";
            else if (value == 91)
                return value + " - GNSS Failure";
            else if (value == 82)
                return value + " - Reduced Enter";
            else if (value == 87)
                return value + " - Reduced Exit";
            else if (value == 84)
                return value + " - Disabled Enter";
            else if (value == 85)
                return value + " - Disabled Exit";
            else if (value == 89)
                return value + " - Responsibility Lost";
            else
                return value + " - Unknow - Thrane";
        }

        public static ThraneReportEvent GetThraneReportEvent(int value)
        {
            if (value == 88)
                return ThraneReportEvent.PolledPosition;
            else if (value == 11)
                return ThraneReportEvent.IntervalPosition;
            else if (value == 15)
                return ThraneReportEvent.IMOInvalid;
            else if (value == 64)
                return ThraneReportEvent.PowerOn;
            else if (value == 65)
                return ThraneReportEvent.PowerOff;
            else if (value == 68)
                return ThraneReportEvent.AntennaDisconnect;
            else if (value == 69)
                return ThraneReportEvent.IridiuimSignalLoss;
            else if (value == 91)
                return ThraneReportEvent.GNSSFailure;
            else if (value == 82)
                return ThraneReportEvent.ReducedEnter;
            else if (value == 87)
                return ThraneReportEvent.ReducedExit;
            else if (value == 84)
                return ThraneReportEvent.DisabledEnter;
            else if (value == 85)
                return ThraneReportEvent.DisabledExit;
            else if (value == 89)
                return ThraneReportEvent.ResponsibilityLost;
            else
                return ThraneReportEvent.Unknown;
        }
    }

    public enum ThraneReportEvent
    {
        PolledPosition,
        IntervalPosition,
        IMOInvalid,
        PowerOn,
        PowerOff,
        AntennaDisconnect,
        IridiuimSignalLoss,
        GNSSFailure,
        ReducedEnter,
        ReducedExit,
        DisabledEnter,
        DisabledExit,
        ResponsibilityLost,
        Unknown
    }
}
