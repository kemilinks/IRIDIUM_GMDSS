using IRIDIUM_GMDSS_LRIT.AdminWebUI.Data;
using IRIDIUM_GMDSS_LRIT.Core.Entity;
using IRIDIUM_GMDSS_LRIT.Core.Mgr;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[UIKeys._AUTHENTICATED_USER] == null)
                Response.Redirect("Login.aspx");

            LoadTerminalList();
        }

        private void LoadTerminalList()
        {
            TerminalMgr terminalMgr = new TerminalMgr();
            List<Terminal> terminals = terminalMgr.GetTerminals();
            gvTerminals.DataSource = terminals;
            gvTerminals.DataBind();
        }
    }
}