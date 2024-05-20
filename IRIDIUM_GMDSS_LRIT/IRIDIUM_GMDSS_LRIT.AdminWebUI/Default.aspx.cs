using IRIDIUM_GMDSS_LRIT.AdminWebUI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IRIDIUM_GMDSS_LRIT.AdminWebUI
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[UIKeys._AUTHENTICATED_USER] == null)
                Response.Redirect("Login.aspx");
        }
    }
}