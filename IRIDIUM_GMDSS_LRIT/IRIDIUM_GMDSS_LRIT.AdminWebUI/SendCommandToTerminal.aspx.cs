using IRIDIUM_GMDSS_LRIT.AdminWebUI.Data;
using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Mgr;
using IRIDIUM_GMDSS_LRIT.Core.WcfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IRIDIUM_GMDSS_LRIT.AdminWebUI
{
    public partial class SendCommandToTerminal : System.Web.UI.Page
    {
        private const string _APP_ID = "MANAGEMENT_PORTAL";
        private const string _ACCESS_CODE = "!MASTER0808*";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[UIKeys._AUTHENTICATED_USER] == null)
                Response.Redirect("Login.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            string source = txtSource.Text;
            string destination = txtDestination.Text;
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(destination))
            {
                lblMessage.Text = "Source and Destination could be empty.";
                return;
            }

            IridiumGmdssLritWcfService.ServiceClient serviceClient = new IridiumGmdssLritWcfService.ServiceClient();
            string imoNumber = txtIMONumber.Text;
            string commandData = string.Empty;
            if (ddlCommandType.SelectedValue.Equals("Registration"))
            {
                try
                {
                    Convert.ToInt32(txtIMONumber.Text);
                }
                catch (Exception)
                {
                    lblMessage.Text = "Invalid IMO Number";
                    return;
                }

                GatewayServiceResponse response = serviceClient.RegisterNewTerminalWithoutAdding(destination, imoNumber, string.Empty, _APP_ID, _ACCESS_CODE);
                if (response.isSuccessful)
                    lblMessage.Text = "Command Submitted.";
                else
                    lblMessage.Text = "Command Not Submitted. Check Iridium Gmdss Gateway.";
            }
            else if (ddlCommandType.SelectedValue.Equals("On-demand Poll"))
            {
                GatewayServiceResponse response = serviceClient.OnDemandPoll(destination, _APP_ID, _ACCESS_CODE);
                if (response.isSuccessful)
                    lblMessage.Text = "Command Submitted.";
                else
                    lblMessage.Text = "Command Not Submitted. Check Iridium Gmdss Gateway.";
            }
            else if (ddlCommandType.SelectedValue.Equals("Reporting Interval"))
            {
                GatewayServiceResponse response = serviceClient.ChangeInterval(destination, Convert.ToInt32(ddlInterval.SelectedValue), _APP_ID, _ACCESS_CODE);
                if (response.isSuccessful)
                    lblMessage.Text = "Command Submitted.";
                else
                    lblMessage.Text = "Command Not Submitted. Check Iridium Gmdss Gateway.";
            }
            else if (ddlCommandType.SelectedValue.Equals("Stop Reporting"))
            {
                GatewayServiceResponse response = serviceClient.StopReporting(destination, _APP_ID, _ACCESS_CODE);
                if (response.isSuccessful)
                    lblMessage.Text = "Command Submitted.";
                else
                    lblMessage.Text = "Command Not Submitted. Check Iridium Gmdss Gateway.";
            }
            else if (ddlCommandType.SelectedValue.Equals("Deregistration"))
            {
                GatewayServiceResponse response = serviceClient.DeregisterTerminal(destination, _APP_ID, _ACCESS_CODE);
                if (response.isSuccessful)
                    lblMessage.Text = "Command Submitted.";
                else
                    lblMessage.Text = "Command Not Submitted. Check Iridium Gmdss Gateway.";
            }
            else
            {
                lblMessage.Text = "Unknown Command Type";
                return;
            }

            lblMessage.Text = "Command Submitted.";
            serviceClient.Close();
        }
    }
}