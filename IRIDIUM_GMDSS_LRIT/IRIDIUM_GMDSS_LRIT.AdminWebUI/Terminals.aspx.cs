using IRIDIUM_GMDSS_LRIT.AdminWebUI.Data;
using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Mgr;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IRIDIUM_GMDSS_LRIT.AdminWebUI
{
    public partial class Terminals : System.Web.UI.Page
    {
        private const string _APP_ID = "MANAGEMENT_PORTAL";
        private const string _ACCESS_CODE = "!MASTER0808*";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[UIKeys._AUTHENTICATED_USER] == null)
                Response.Redirect("Login.aspx");

            if(!IsPostBack)
                LoadTerminalList();
        }

        private void LoadTerminalList()
        {
            TerminalMgr terminalMgr = new TerminalMgr();
            List<Terminal> terminals = terminalMgr.GetTerminals();
            gvTerminals.DataSource = terminals;
            gvTerminals.DataBind();
        }

        protected void btnAddNewTerminal_Click(object sender, EventArgs e)
        {
            TerminalMgr terminalMgr = new TerminalMgr();
            string applicationIds = _APP_ID + "," + txtApplicationIDs.Text;
            if (terminalMgr.InsertNewTerminal(txtMSISDN.Text, txtIMONumber.Text, txtDescription.Text, txtRemarks.Text, applicationIds))
            {
                lblMessage.Text = "New Terminal is Added.";
                txtMSISDN.Text = string.Empty;
                txtIMONumber.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtApplicationIDs.Text = string.Empty;
                LoadTerminalList();
            }
            else
            {
                lblMessage.Text = "New Terminal is NOT Added.";
            }
        }
    }
}