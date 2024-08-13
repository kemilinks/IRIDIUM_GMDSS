using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inetlab.SMPP;
using Inetlab.SMPP.Builders;
using Inetlab.SMPP.Common;
using Inetlab.SMPP.Logging;
using Inetlab.SMPP.PDU;
using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Mgr;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using KemilinksNotification;
using SMS = Inetlab.SMPP.SMS;

namespace IRIDIUM.GMDSS_LRIT.SmppClient
{

    public partial class SmppClientFrm : Form
    {
        private readonly MessageComposer _messageComposer;
        private readonly ILog _log;

        private readonly Inetlab.SMPP.SmppClient _client;
        private bool logToFile;
        private bool isGracefullyDisconnect;
        private string adminSmsNumber;
        

        private string _DATETIME_FORMAT = "dd-MMM-yyyy HH:mm:ss";
        
        

        private DataCommandMgr dataCommandMgr;
        private DataMgr dataMgr;
        private SystemConfigUtility sysConfigUtility;

        public SmppClientFrm()
        {
            InitializeComponent();

            LogManager.SetLoggerFactory(new TextBoxLogFactory(tbLog, LogLevel.Info));

            //HOW TO INSTALL LICENSE FILE
            //====================
            //After purchase you will receive Inetlab.SMPP.license file per E-Mail. 
            //Add this file into the root of project where you have a reference on Inetlab.SMPP.dll. Change "Build Action" of the file to "Embedded Resource". 

            //Set license before using Inetlab.SMPP classes in your code:

            // C#
            Inetlab.SMPP.LicenseManager.SetLicense(this.GetType().Assembly.GetManifestResourceStream(this.GetType(), "Inetlab.SMPP.license" ));
            //
            // VB.NET
            // Inetlab.SMPP.LicenseManager.SetLicense(Me.GetType().Assembly.GetManifestResourceStream(Me.GetType(), "Inetlab.SMPP.license"))


            _log = LogManager.GetLogger(GetType().Name);


            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                LogManager.GetLogger("AppDomain").Fatal((Exception)args.ExceptionObject, "Unhandled Exception");
            };


            _client = new Inetlab.SMPP.SmppClient();
            _client.ResponseTimeout = TimeSpan.FromSeconds(120);
            //_client.EnquireLinkInterval = TimeSpan.FromSeconds(20);

            _client.evDisconnected += new DisconnectedEventHandler(client_evDisconnected);
            _client.evDeliverSm += new DeliverSmEventHandler(client_evDeliverSm);
            _client.evEnquireLink += new EnquireLinkEventHandler(client_evEnquireLink);
            _client.evUnBind += new UnBindEventHandler(client_evUnBind);
            _client.evDataSm += new DataSmEventHandler(client_evDataSm);
            _client.evRecoverySucceeded += ClientOnRecoverySucceeded;

            _client.evServerCertificateValidation += OnCertificateValidation;


            _messageComposer = new MessageComposer();
            _messageComposer.evFullMessageReceived += OnFullMessageReceived;
            _messageComposer.evFullMessageTimeout += OnFullMessageTimeout;

            this.dataCommandMgr = new DataCommandMgr();
            this.dataMgr = new DataMgr();
            this.sysConfigUtility = new SystemConfigUtility();
            this.isGracefullyDisconnect = false;
        }

        private void SmppClient_Load(object sender, EventArgs e)
        {
            cbSubmitMode.SelectedIndex = 1;
            cbDataCoding.SelectedIndex = 2;

            cbBindingMode.Items.Clear();
            foreach (ConnectionMode mode in Enum.GetValues(typeof(ConnectionMode)))
            {
                if (mode == ConnectionMode.None) continue;
                cbBindingMode.Items.Add(mode);
            }
            cbBindingMode.SelectedItem = ConnectionMode.Transceiver;

            //ConfigurationManager.AppSettings[""].ToString();
            tbHostname.Text = ConfigurationManager.AppSettings["Host"].ToString();
            tbPort.Text = ConfigurationManager.AppSettings["Port"].ToString();
            tbSystemId.Text = ConfigurationManager.AppSettings["SystemId"].ToString();
            tbPassword.Text = ConfigurationManager.AppSettings["Password"].ToString();
            tbSrcAdr.Text = ConfigurationManager.AppSettings["Originator"].ToString();
            tbSrcAdrTON.Text = ConfigurationManager.AppSettings["OriginatorTon"].ToString();
            tbSrcAdrNPI.Text = ConfigurationManager.AppSettings["OriginatorNpi"].ToString();
            tbDestAdr.Text = ConfigurationManager.AppSettings["Destination"].ToString();
            tbDestAdrTON.Text = ConfigurationManager.AppSettings["DestinationTon"].ToString();
            tbDestAdrNPI.Text = ConfigurationManager.AppSettings["DestinationNpi"].ToString();
            this.adminSmsNumber = ConfigurationManager.AppSettings["Admin_Sms_Number"].ToString();
            this.Text += " " + "["+ ConfigurationManager.AppSettings["Environment"].ToString() + "]";

            if (ConfigurationManager.AppSettings["LogToFile"].ToString().Equals("Yes"))
                this.logToFile = true;
            else
                this.logToFile = false;

            DateTime utcNow = DateTime.UtcNow;
        }

        private void OnCertificateValidation(object sender, CertificateValidationEventArgs args)
        {
            //accept all certificates
            args.Accepted = true;
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                _client.Dispose();
            }
            base.Dispose(disposing);
        }


        private async Task Connect()
        {

            try
            {
                if (_client.Status == ConnectionStatus.Closed)
                {
                    _log.Info("Connecting to " + tbHostname.Text);

                    bConnect.Enabled = false;
                    bDisconnect.Enabled = false;
                    cbReconnect.Enabled = false;


                    _client.EsmeAddress = new SmeAddress(tbAddressRange.Text, (AddressTON)Convert.ToByte(tbAddrTon.Text),
                        (AddressNPI)Convert.ToByte(tbAddrNpi.Text));

                    _client.SystemType = tbSystemType.Text;

                    _client.ConnectionRecovery = cbReconnect.Checked;
                    _client.ConnectionRecoveryDelay = TimeSpan.FromSeconds(3);

                    SmppClientConnectionOptions connectionOptions = new SmppClientConnectionOptions();
                    connectionOptions.RemoteEndPoint = new DnsEndPoint(tbHostname.Text, Convert.ToInt32(tbPort.Text),
                        AddressFamily.InterNetwork);


                    if (cbSSL.Checked)
                    {
                        connectionOptions.Ssl = new SslConnectionOptions();
                        // connectionOptions.Ssl.EnabledSslProtocols =SslProtocols.Tls12;
                        connectionOptions.Ssl.ValidateCertificate = (sender, certificate, chain, errors) => true; //allow all server certificates

                        // uncomment the line bellow to enable client certificate authentication
                        // connectionOptions.ClientCertificates.Add(new X509Certificate2("client.p12", "12345"));
                    }

                    bool bSuccess = await _client.ConnectAsync(connectionOptions);

                    if (bSuccess)
                    {
                        _log.Info("SmppClient connected");

                        await Bind();
                    }
                    else
                    {

                        bConnect.Enabled = true;
                        cbReconnect.Enabled = true;
                        bDisconnect.Enabled = false;

                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Failed to connect to {tbHostname.Text}");
            }

        }

        private async Task Bind()
        {
            _log.Info("Bind client with SystemId: {0}", tbSystemId.Text);
            bDisconnect.Enabled = true;
            ConnectionMode mode = (ConnectionMode)cbBindingMode.SelectedItem;

            BindResp resp = await _client.BindAsync(tbSystemId.Text, tbPassword.Text, mode);

#pragma warning disable IDE0010 // Add missing cases
            switch (resp.Header.Status)
#pragma warning restore IDE0010 // Add missing cases
            {
                case Inetlab.SMPP.Common.CommandStatus.ESME_ROK:
                    _log.Info("Bind succeeded: Status: {0}, SystemId: {1}", resp.Header.Status, resp.SystemId);

                    bSubmit.Enabled = true;

                    break;
                default:
                    _log.Warn("Bind failed: Status: {0}", resp.Header.Status);

                    await Disconnect();
                    break;
            }
        }

        private async Task Disconnect()
        {
            _log.Info("Disconnect from SMPP server");

            if (_client.Status == ConnectionStatus.Bound)
            {
                await UnBind();
            }

            if (_client.Status == ConnectionStatus.Open)
            {
                await _client.DisconnectAsync();
            }
        }

        private void client_evDisconnected(object sender)
        {
            _log.Info("SmppClient disconnected");
            if (!this.isGracefullyDisconnect)
                this.SendSmsNotificationToAdministrator(this.adminSmsNumber, "Smpp Client disconnected ungracefully. Please Check");
            Sync(this, () =>
            {
                bConnect.Enabled = true;
                bDisconnect.Enabled = false;
                bSubmit.Enabled = false;
                cbReconnect.Enabled = true;


            });

        }

        private void ClientOnRecoverySucceeded(object sender, BindResp data)
        {
            _log.Info("Connection has been recovered.");

            Sync(this, () =>
            {
                bConnect.Enabled = false;
                bDisconnect.Enabled = true;
                bSubmit.Enabled = true;
                cbReconnect.Enabled = false;
            });

        }

        private async Task UnBind()
        {
            _log.Info("Unbind SmppClient");
            UnBindResp resp = await _client.UnbindAsync();

#pragma warning disable IDE0010 // Add missing cases
            switch (resp.Header.Status)
#pragma warning restore IDE0010 // Add missing cases
            {
                case Inetlab.SMPP.Common.CommandStatus.ESME_ROK:
                    _log.Info("UnBind succeeded: Status: {0}", resp.Header.Status);
                    break;
                default:
                    _log.Warn("UnBind failed: Status: {0}", resp.Header.Status);
                    await _client.DisconnectAsync();
                    break;
            }

        }

        private void client_evDeliverSm(object sender, DeliverSm data)
        {
            try
            {
                //Check if we received Delivery Receipt
                if (data.MessageType == MessageTypes.SMSCDeliveryReceipt)
                {
                    //Get MessageId of delivered message
                    string messageId = data.Receipt.MessageId;
                    MessageState deliveryStatus = data.Receipt.State;

                    _log.Info("Delivery Receipt received: {0}", data.Receipt.ToString());
                }
                else
                {

                    // Receive incoming message and try to concatenate all parts
                    if (data.Concatenation != null)
                    {
                        _messageComposer.AddMessage(data);
                        string logLine = string.Format("DeliverSm part received: Sequence: {0}, SourceAddress: {1}, Concatenation ( {2} ) Coding: {3}, Text: {4}", data.Header.Sequence, data.SourceAddress, data.Concatenation, data.DataCoding, _client.EncodingMapper.GetMessageText(data));
                        _log.Info(logLine);
                        if (this.logToFile)
                            KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, logLine, string.Empty);
                    }
                    else
                    {
                        if (data.DataCoding == DataCodings.Default)
                        {
                            string logLine = string.Format("DeliverSm received : Sequence: {0}, SourceAddress: {1}, Coding: {2}, Text: {3}", data.Header.Sequence, data.SourceAddress, data.DataCoding, _client.EncodingMapper.GetMessageText(data));
                            _log.Info(logLine);
                            if (this.logToFile)
                                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, logLine, string.Empty);
                        }
                        else if (data.DataCoding == DataCodings.OctetUnspecified)
                        {
                            byte[] messageDataBbytes = _client.EncodingMapper.GetMessageBytes(_client.EncodingMapper.GetMessageText(data), data.DataCoding);
                            string messageDataHexString = ByteArrayToString(messageDataBbytes);
                            string logLine = string.Format("DeliverSm received : Sequence: {0}, SourceAddress: {1}, Coding: {2}, Text: {3}", data.Header.Sequence, data.SourceAddress, data.DataCoding, messageDataHexString);
                            _log.Info(logLine);
                            if (this.logToFile)
                                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, logLine, string.Empty);
                            
                            this.dataMgr.ProcessIncomingData(logLine, tbSrcAdr.Text);
                        }
                    }

                    // Check if an ESME acknowledgement is required
                    if (data.Acknowledgement != SMEAcknowledgement.NotRequested)
                    {
                        // You have to clarify with SMSC support what kind of information they request in ESME acknowledgement.

                        string messageText = data.GetMessageText(_client.EncodingMapper);

                        var smBuilder = SMS.ForSubmit()
                            .From(data.DestinationAddress)
                            .To(data.SourceAddress)
                            .Coding(data.DataCoding)
                            .Concatenation(ConcatenationType.UDH8bit, _client.SequenceGenerator.NextReferenceNumber())
                            .Set(m => m.MessageType = MessageTypes.SMEDeliveryAcknowledgement)
                            .Text(new Receipt
                            {
                                DoneDate = DateTime.Now,
                                State = MessageState.Delivered,
                                //  MessageId = data.Response.MessageId,
                                ErrorCode = "0",
                                SubmitDate = DateTime.Now,
                                Text = messageText.Substring(0, Math.Min(20, messageText.Length))
                            }.ToString()
                            );



                        _client.SubmitAsync(smBuilder).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                data.Response.Header.Status = Inetlab.SMPP.Common.CommandStatus.ESME_RX_T_APPN;
                _log.Error(ex, "Failed to process DeliverSm");
            }
        }

        private void client_evDataSm(object sender, DataSm data)
        {
            _log.Info("DataSm received : Sequence: {0}, SourceAddress: {1}, DestAddress: {2}, Coding: {3}, Text: {4}",
                data.Header.Sequence, data.SourceAddress, data.DestinationAddress, data.DataCoding, data.GetMessageText(_client.EncodingMapper));
        }

        private void OnFullMessageTimeout(object sender, MessageEventHandlerArgs args)
        {
            _log.Info("Incomplete message received From: {0}, Text: {1}", args.GetFirst<DeliverSm>().SourceAddress, args.Text);
        }

        private void OnFullMessageReceived(object sender, MessageEventHandlerArgs args)
        {
            _log.Info("Full message received From: {0}, To: {1}, Text: {2}", args.GetFirst<DeliverSm>().SourceAddress, args.GetFirst<DeliverSm>().DestinationAddress, args.Text);
        }

        private void client_evEnquireLink(object sender, EnquireLink data)
        {
            _log.Info("EnquireLink received");
            this.sysConfigUtility.UpdateLastEnquireLinkReceivedDateTime(CommonUtility.GetCurrentTimestmap());
        }

        private void client_evUnBind(object sender, UnBind data)
        {
            _log.Info("UnBind request received");
        }

        private async void bConnect_Click(object sender, EventArgs e)
        {
            this.sysConfigUtility.UpdateLastSmppServerConnectionDateTime(CommonUtility.GetCurrentTimestmap());
            this.isGracefullyDisconnect = false;
            await Connect();
            tmSendCommandToTerminal.Start();
        }

        private async void bDisconnect_Click(object sender, EventArgs e)
        {
            this.isGracefullyDisconnect = true;
            tmSendCommandToTerminal.Stop();
            await Disconnect();
        }

        private async void bSubmit_Click(object sender, EventArgs e)
        {
            if (_client.Status != ConnectionStatus.Bound)
            {
                MessageBox.Show("Before sending messages, please connect to SMPP server.");
                return;
            }

            bSubmit.Enabled = false;

            _client.SendSpeedLimit = GetSpeedLimit(tbSubmitSpeed.Text);

            if (cbBatch.Checked)
            {
                await SubmitBatchMessages();
            }
            else
            {
                string[] dstAddresses = tbDestAdr.Text.Split(',');

                if (dstAddresses.Length == 1)
                {
                    await SubmitSingleMessage();
                }
                else if (dstAddresses.Length > 1)
                {
                    await SubmitMultiMessage(dstAddresses);
                }
            }

            bSubmit.Enabled = true;
        }

        private LimitRate GetSpeedLimit(string text)
        {
            if (string.IsNullOrWhiteSpace(tbSubmitSpeed.Text))
            {
                return LimitRate.NoLimit;
            }

            int occurrences;
            if (!int.TryParse(tbSubmitSpeed.Text, out occurrences) || occurrences == 0)
                return LimitRate.NoLimit;

            return new LimitRate(occurrences, TimeSpan.FromSeconds(1));

        }

        private async Task SubmitSingleMessage()
        {
            DataCodings coding = GetDataCoding();

            var sourceAddress = new SmeAddress(tbSrcAdr.Text, (AddressTON)byte.Parse(tbSrcAdrTON.Text), (AddressNPI)byte.Parse(tbSrcAdrNPI.Text));

            var destinationAddress = new SmeAddress(tbDestAdr.Text, (AddressTON)byte.Parse(tbDestAdrTON.Text), (AddressNPI)byte.Parse(tbDestAdrNPI.Text));

            _log.Info("Submit message To: {0}. Text: {1}", tbDestAdr.Text, tbMessageText.Text);

            ISubmitSmBuilder builder = null;

            if (coding == DataCodings.OctetUnspecified)
            {
                builder = SMS.ForSubmit()
                        .From(sourceAddress)
                        .To(destinationAddress)
                        .Coding(coding)
                        .Data(StringToByteArray(tbMessageText.Text))
                        .ExpireIn(TimeSpan.FromDays(2));
                //.DeliveryReceipt();Iridium Complained
            }
            else
            {
                builder = SMS.ForSubmit()
                   .From(sourceAddress)
                   .To(destinationAddress)
                   .Coding(coding)
                   .Text(tbMessageText.Text)
                   .ExpireIn(TimeSpan.FromDays(2));
                //.DeliveryReceipt();Iridiuim Complained
            }

            SubmitMode mode = GetSubmitMode();
            if (mode == SubmitMode.Payload)
            {
                builder.MessageInPayload();
            }
            else if (mode == SubmitMode.ShortMessageWithSAR)
            {
                builder.Concatenation(ConcatenationType.SAR);
            }

            try
            {
                IList<SubmitSmResp> resp = await _client.SubmitAsync(builder);

                if (resp.All(x => x.Header.Status == Inetlab.SMPP.Common.CommandStatus.ESME_ROK))
                {
                    _log.Info("Submit succeeded. MessageIds: {0}", string.Join(",", resp.Select(x => x.MessageId)));
                }
                else
                {
                    _log.Warn("Submit failed. Status: {0}", string.Join(",", resp.Select(x => x.Header.Status.ToString())));
                }
            }
            catch (Exception ex)
            {
                _log.Error("Submit failed. Error: {0}", ex.Message);
            }
        }

        private async Task SubmitMultiMessage(string[] dstAddresses)
        {
            DataCodings coding = GetDataCoding();

            byte srcTon = byte.Parse(tbSrcAdrTON.Text);
            byte srcNpi = byte.Parse(tbSrcAdrNPI.Text);
            string srcAdr = tbSrcAdr.Text;

            byte dstTon = byte.Parse(tbDestAdrTON.Text);
            byte dstNpi = byte.Parse(tbDestAdrNPI.Text);

            ISubmitMultiBuilder builder = SMS.ForSubmitMulti()
                .From(srcAdr, (AddressTON)srcTon, (AddressNPI)srcNpi)
                .Coding(coding)
                .Text(tbMessageText.Text);

            if (cbDeliveryReceipt.Checked)
            {
                //Request delivery receipt
                builder.DeliveryReceipt();
            }

            foreach (var dstAddress in dstAddresses)
            {
                if (dstAddress == null || dstAddress.Trim().Length == 0) continue;

                builder.To(dstAddress.Trim(), (AddressTON)dstTon, (AddressNPI)dstNpi);
            }

            _log.Info("Submit message to several addresses: {0}. Text: {1}", string.Join(", ", dstAddresses), tbMessageText.Text);


            SubmitMode mode = GetSubmitMode();
            if (mode == SubmitMode.Payload)
            {
                builder.MessageInPayload();
            }
            else if (mode == SubmitMode.ShortMessageWithSAR)
            {
                builder.Concatenation(ConcatenationType.SAR);
            }


            try
            {
                IList<SubmitMultiResp> resp = await _client.SubmitAsync(builder);

                if (resp.All(x => x.Header.Status == Inetlab.SMPP.Common.CommandStatus.ESME_ROK))
                {
                    _log.Info("Submit succeeded. MessageIds: {0}", string.Join(",", resp.Select(x => x.MessageId)));
                }
                else
                {
                    _log.Warn("Submit failed. Status: {0}", string.Join(",", resp.Select(x => x.Header.Status.ToString())));
                }
            }
            catch (Exception ex)
            {
                _log.Error("Submit failed. Error: {0}", ex.Message);
            }

        }

        private async Task SubmitBatchMessages()
        {
            var sourceAddress = new SmeAddress(tbSrcAdr.Text, (AddressTON)byte.Parse(tbSrcAdrTON.Text), (AddressNPI)byte.Parse(tbSrcAdrNPI.Text));

            var destinationAddress = new SmeAddress(tbDestAdr.Text, (AddressTON)byte.Parse(tbDestAdrTON.Text), (AddressNPI)byte.Parse(tbDestAdrNPI.Text));


            string messageText = tbMessageText.Text;

            SubmitMode mode = GetSubmitMode();

            DataCodings coding = GetDataCoding();

            int count = int.Parse(tbRepeatTimes.Text);

            _log.Info("Submit message batch. Count: {0}. Text: {1}", count, messageText);

            // bulk sms test
            List<SubmitSm> batch = new List<SubmitSm>();
            for (int i = 0; i < count; i++)
            {
                ISubmitSmBuilder builder = SMS.ForSubmit()
                    .Text(messageText)
                    .From(sourceAddress)
                    .To(destinationAddress)
                    .Coding(coding);

                if (mode == SubmitMode.Payload)
                {
                    builder.MessageInPayload();
                }
                else if (mode == SubmitMode.ShortMessageWithSAR)
                {
                    builder.Concatenation(ConcatenationType.SAR);
                }

                if (cbDeliveryReceipt.Checked)
                {
                    builder.DeliveryReceipt();
                }

                batch.AddRange(builder.Create(_client));

            }




            try
            {
                _client.Metrics.Reset();
                Stopwatch watch = Stopwatch.StartNew();

                IEnumerable<SubmitSmResp> resp = await Task.Run(() => _client.SubmitAsync(batch));

                watch.Stop();

                if (resp.All(x => x.Header.Status == Inetlab.SMPP.Common.CommandStatus.ESME_ROK))
                {
                    _log.Info("Batch sending completed. Submitted: {0}, Elapsed: {1} ms, Performance: {2} m/s", batch.Count, watch.ElapsedMilliseconds, batch.Count * 1000f / watch.ElapsedMilliseconds);
                    _log.Info($"Response time: {_client.Metrics.Sent.Requests.ResponseTime}");
                }
                else
                {
                    var wrongStatuses = resp.Where(x => x.Header.Status != Inetlab.SMPP.Common.CommandStatus.ESME_ROK)
                        .Select(x => x.Header.Status).Distinct();

                    _log.Warn("Submit failed. Wrong Status: {0}", string.Join(", ", wrongStatuses));
                }
            }
            catch (Exception ex)
            {
                _log.Error("Submit failed. Error: {0}", ex.Message);
            }




        }

        private SubmitMode GetSubmitMode()
        {
            return (SubmitMode)Enum.Parse(typeof(SubmitMode), cbSubmitMode.Text);
        }

        private DataCodings GetDataCoding()
        {
            return (DataCodings)Enum.Parse(typeof(DataCodings), cbDataCoding.Text);
        }

        private void cbAsync_CheckedChanged(object sender, EventArgs e)
        {
            tbRepeatTimes.Enabled = cbBatch.Checked;
        }

        public delegate void SyncAction();

        public static void Sync(Control control, SyncAction action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action, new object[] { });
                return;
            }

            action();
        }

        private void cbReconnect_CheckedChanged(object sender, EventArgs e)
        {
            _client.ConnectionRecovery = cbReconnect.Checked;


        }

        private void bClearLog_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
        }

        private string GetTimesttampInStringForLogging()
        {
            DateTime utcNow = DateTime.UtcNow;
            return utcNow.ToString(_DATETIME_FORMAT, CultureInfo.InvariantCulture) + " - ";
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private void SmppClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            _client.evDisconnected -= client_evDisconnected;
            _client.Dispose();
        }

        private async void tmSendCommandToTerminal_Tick(object sender, EventArgs e)
        {
            List<DataCommand> commands = this.dataCommandMgr.GetDataCommands(Direction.Mobile_Terminated, IRIDIUM_GMDSS_LRIT.Core.Entity.CommandStatus.New);
            foreach (DataCommand command in commands)
            {
                DataCodings coding = GetDataCoding();

                var sourceAddress = new SmeAddress(command.Source, (AddressTON)byte.Parse("0"), (AddressNPI)byte.Parse("0"));

                var destinationAddress = new SmeAddress(command.Destination, (AddressTON)byte.Parse("1"), (AddressNPI)byte.Parse("1"));

                _log.Info("Submit message To: {0}. Text: {1}", command.Destination, command.Data);


                ISubmitSmBuilder builder = SMS.ForSubmit()
                    .From(sourceAddress)
                    .To(destinationAddress)
                    .Coding(coding)
                    .Data(StringToByteArray(command.Data))
                    .ExpireIn(TimeSpan.FromDays(2));
                    //.DeliveryReceipt(); Iridium Complained

                SubmitMode mode = GetSubmitMode();
                if (mode == SubmitMode.Payload)
                {
                    builder.MessageInPayload();
                }
                else if (mode == SubmitMode.ShortMessageWithSAR)
                {
                    builder.Concatenation(ConcatenationType.SAR);
                }

                try
                {
                    IList<SubmitSmResp> resp = await _client.SubmitAsync(builder);

                    if (resp.All(x => x.Header.Status == Inetlab.SMPP.Common.CommandStatus.ESME_ROK))
                    {
                        string messageId = resp.Select(x => x.MessageId).ToArray()[0];
                        _log.Info("Submit succeeded. MessageIds: {0}", string.Join(",", messageId));
                        command.Status = IRIDIUM_GMDSS_LRIT.Core.Entity.CommandStatus.Sent;
                        command.ReferenceNumber = messageId;
                        command.Timestamp = DateTime.UtcNow;
                        this.dataCommandMgr.UpdateDataCommand(command);
                    }
                    else
                    {
                        string logLine = string.Format("Submit failed. Status: {0}", string.Join(",", resp.Select(x => x.Header.Status.ToString())));
                        _log.Warn(logLine);
                        command.Status = IRIDIUM_GMDSS_LRIT.Core.Entity.CommandStatus.FailedToSend;
                        command.ReferenceNumber = logLine;
                        command.Timestamp = DateTime.UtcNow;
                        this.dataCommandMgr.UpdateDataCommand(command);
                    }
                }
                catch (Exception ex)
                {
                    string logLine = string.Format("Submit failed. Error: {0}", ex.Message);
                    _log.Error(logLine);
                    command.Status = IRIDIUM_GMDSS_LRIT.Core.Entity.CommandStatus.FailedToSend;
                    command.ReferenceNumber = logLine;
                    command.Timestamp = DateTime.UtcNow;
                    this.dataCommandMgr.UpdateDataCommand(command);
                }
                

                System.Threading.Thread.Sleep(2000);
            }
        }

        private void SendSmsNotificationToAdministrator(string SMS_NUMBER, string smsContent)
        {
            try
            {
                KemilinksNotification.CommzGateSMS sms = new CommzGateSMS();
                string result = sms.SendSMS(SMS_NUMBER, smsContent, "Iridium_Gmdss_Smpp_Client");
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.INFO, "Unable to send SMS @  " + DateTime.UtcNow.ToString("dd MMM yyyy HH:mm:ss") + " of Content: " + smsContent + ". Exception: " + ex.Message, string.Empty);
            }
        }

        private void SmppClientFrm_Shown(object sender, EventArgs e)
        {
            bConnect.PerformClick();
        }
    }
}
