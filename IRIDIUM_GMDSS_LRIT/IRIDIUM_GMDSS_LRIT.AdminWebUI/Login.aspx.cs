using IRIDIUM_GMDSS_LRIT.AdminWebUI.Data;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IRIDIUM_GMDSS_LRIT.AdminWebUI
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            bool isValid = false;
            SystemConfigUtility systemConfigUtility = new SystemConfigUtility();
            string _accounts = systemConfigUtility.GetWebUIAdminAccounts();
            if (!string.IsNullOrEmpty(_accounts))
            {
                string[] accounts = _accounts.Split('|');
                for (int i = 0; i < accounts.Length; i++)
                {
                    string account = accounts[i];
                    string username = account.Split(':')[0];
                    string password = account.Split(':')[1];
                    if (txtUsermae.Text.Equals(username) && txtPassword.Text.Equals(password))
                    {
                        isValid = true;
                        break;
                    }
                }
                if (isValid)
                {
                    AuthenticatedUser authenticatedUser = new AuthenticatedUser()
                    {
                        Username = txtUsermae.Text
                    };
                    Session[UIKeys._AUTHENTICATED_USER] = authenticatedUser;
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    txtMessage.Text = "Invalid Credentials.";
                }
            }
            else
            {
                txtMessage.Text = "Invalid Credentials.";
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtUsermae.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }
    }
}