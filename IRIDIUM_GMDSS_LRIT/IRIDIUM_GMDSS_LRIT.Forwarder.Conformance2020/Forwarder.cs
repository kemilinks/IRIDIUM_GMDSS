using IRIDIUM_GMDSS_LRIT.Core.Forwarder.Interface;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Forwarder.Conformance2020
{
    public class Forwarder : IPositionForwarder
    {
        public const string DateTimeFormat = "dd-MMM-yy h:mm tt";
        public const char VALUE_SPLIT_CHARACTER = ',';
        public const char KEYVALUE_SPLIT_CHARACTER = ':';
        public const char KEYVALUE_VALUE_SPLIT_CHARACTER = '|';
        public const string TAG = "Tag";

        private string _EndPoint;
        public string EndPoint
        {
            get
            {
                return this._EndPoint;
            }
            set
            {
                this._EndPoint = value;
            }
        }

        private string _Status;
        public string ForwardResult
        {
            get { return this._Status; }
        }

        public bool DoForward(Int64 reportId, string msisdn, string latitudeHemisphere, int latitudeDegree, int latitudeMinute, double latitudeMinuteDecimal, string longitudeHemisphere, int longitudeDegree, int longitudeMinute, double longitudeMinuteDecimal, int year, int month, int day, int hour, int minute, int second, DateTime gatewayReceiveTimestamp)
        {
            try
            {
                    Conformance2020DataService.ReceiverServiceClient conformance2020Client = GetServiceInterface();
                    Conformance2020DataService.Position conformance2020Position = ConvertToServiceType(latitudeHemisphere, latitudeDegree, latitudeMinute, latitudeMinuteDecimal, longitudeHemisphere, longitudeDegree, longitudeMinute, longitudeMinuteDecimal, year, month, day, hour, minute, second, gatewayReceiveTimestamp);
                    Conformance2020DataService.Response resp = conformance2020Client.AddPosition(conformance2020Position, msisdn);


                    if (resp.Success)
                    {
                        string msg = DateTime.UtcNow.ToString(DateTimeFormat) + " : A message [ Report Id - " + reportId + " , MSISDN - " + msisdn + "] is forwared to the end application [Conformance 2020].";
                        this._Status = msg;
                        return true;

                    }
                    else
                    {
                    string msg = DateTime.UtcNow.ToString(DateTimeFormat) + " : A message [ Report Id - " + reportId + " , MSISDN - " + msisdn + "] is NOT forwared to the end application [Conformance 2020].";
                    this._Status = msg;
                        return false;
                    }
            }
            catch (Exception ex)
            {
                this._Status = "Exeption in forwarding message to end application [Conformance 2020], ex: " + ex.Message;
                return false;
            }
        }

        private Conformance2020DataService.Position ConvertToServiceType(string latitudeHemisphere, int latitudeDegree, int latitudeMinute, double latitudeMinuteDecimal, string longitudeHemisphere, int longitudeDegree, int longitudeMinute, double longitudeMinuteDecimal, int year, int month, int day, int hour, int minute, int second, DateTime gatewayReceiveTimestamp)
        {
            Conformance2020DataService.Position conformancePosition = new Conformance2020DataService.Position();

            conformancePosition.COG = 0;
            conformancePosition.CSPTimeStamp = gatewayReceiveTimestamp;

            double latitudeSecond = Math.Round(latitudeMinuteDecimal * 60, 0);
            conformancePosition.Latitude = (double)latitudeDegree + ((double)latitudeMinute / 60) + ((double)latitudeSecond / 3600);
            if (latitudeHemisphere.Equals("S"))
                conformancePosition.Latitude = -1 * conformancePosition.Latitude;

            double longitudeSecond = Math.Round(longitudeMinuteDecimal * 60,0);
            conformancePosition.Longitude = longitudeDegree + ((double)longitudeMinute / 60) + ((double)longitudeSecond / 3600);
            if (longitudeHemisphere.Equals("W"))
                conformancePosition.Longitude = -1 * conformancePosition.Longitude;

            string wgs84Laitude = string.Empty;
            string wgs84Longitude = string.Empty;
            ConvertToWGS84(conformancePosition.Latitude.ToString(), conformancePosition.Longitude.ToString(), ref wgs84Laitude, ref wgs84Longitude);
            conformancePosition.LatitudeWGS84 = wgs84Laitude;
            conformancePosition.LongitudeWGS84 = wgs84Longitude;

            conformancePosition.Region = "NA";
            conformancePosition.SOG = 0;
            conformancePosition.TerminalTimeStamp = new DateTime(year, month, day, hour, minute, second);
            return conformancePosition;
        }

        private Conformance2020DataService.ReceiverServiceClient GetServiceInterface()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Name = "Conformance2020DataService";
            binding.CloseTimeout = System.TimeSpan.Parse("00:05:00");

            binding.OpenTimeout = System.TimeSpan.Parse("00:05:00");
            binding.ReceiveTimeout = System.TimeSpan.Parse("00:10:00");
            binding.SendTimeout = System.TimeSpan.Parse("00:05:00");

            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;

            binding.MaxBufferSize = 655360;
            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 655360;

            binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = System.ServiceModel.TransferMode.Buffered;

            binding.UseDefaultWebProxy = true;
            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 8192;

            binding.ReaderQuotas.MaxArrayLength = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;

            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;

            binding.Security.Transport.Realm = "";
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;


            EndpointAddress endpoint = new EndpointAddress(_EndPoint);

            Conformance2020DataService.ReceiverServiceClient client = new Conformance2020DataService.ReceiverServiceClient(binding, endpoint);

            return client;
        }

        private void ConvertToWGS84(string sLatitude, string sLongitude, ref string sWGS84_Latitude, ref string sWGS84_Longitude)
        {

            double dblTmp = 0;
            double dblSec = 0;
            int iHour = 0;
            int iMin = 0;
            int iPos = 0;
            string sDirection = null;
            string sTmp = null;

            try
            {
                //Convert Latitude to WSG84 Latitude
                double decLatitude = Convert.ToDouble(sLatitude);
                double decLongitude = Convert.ToDouble(sLongitude);

                iPos = Strings.InStr(sLatitude, ".");

                if (iPos == 0)
                {
                    sLatitude += ".0";
                    iPos = Strings.InStr(sLatitude, ".");
                }
                sTmp = Strings.Mid(sLatitude, iPos + 1);
                iHour = Convert.ToInt32(Strings.Left(sLatitude, iPos - 1));

                dblTmp = Convert.ToDouble("0." + sTmp);
                dblTmp = dblTmp * 3600;
                dblSec = dblTmp % 60;
                iMin = (int)(dblTmp - dblSec) / 60;
                dblSec = (dblTmp - (iMin * 60));

                if (decLatitude > 0.0)
                {
                    sDirection = "N";
                }
                else
                {
                    sDirection = "S";
                    iHour = iHour * -1;
                }
                dblTmp = iMin + dblSec;
                sWGS84_Latitude = Strings.Format(iHour, "00") + "." + Strings.Format(iMin, "00") + "." + Strings.Format(dblSec, "00") + "." + sDirection;

                //Convert Latitude to WSG84 Longitude
                iPos = Strings.InStr(sLongitude, ".");
                if (iPos == 0)
                {
                    sLongitude += ".0";
                    iPos = Strings.InStr(sLongitude, ".");
                }
                sTmp = Strings.Mid(sLongitude, iPos + 1);
                iHour = Convert.ToInt32(Strings.Left(sLongitude, iPos - 1));

                dblTmp = Convert.ToDouble("0." + sTmp);
                dblTmp = dblTmp * 3600;
                dblSec = dblTmp % 60;
                iMin = (int)(dblTmp - dblSec) / 60;
                dblSec = (dblTmp - (iMin * 60));

                if (decLongitude > 0.0)
                {
                    sDirection = "E";
                }
                else
                {
                    sDirection = "W";
                    iHour = iHour * -1;
                }
                dblTmp = iMin + dblSec;
                sWGS84_Longitude = Strings.Format(iHour, "000") + "." + Strings.Format(iMin, "00") + "." + Strings.Format(dblSec, "00") + "." + sDirection;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
