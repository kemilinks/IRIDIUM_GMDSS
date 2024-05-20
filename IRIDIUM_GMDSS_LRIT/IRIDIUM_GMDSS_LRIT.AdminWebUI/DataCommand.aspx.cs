using IRIDIUM_GMDSS_LRIT.AdminWebUI.Data;
using IRIDIUM_GMDSS_LRIT.Core.Entity.UI;
using IRIDIUM_GMDSS_LRIT.Core.Mgr;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IRIDIUM_GMDSS_LRIT.AdminWebUI
{
    public partial class DataCommand : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[UIKeys._AUTHENTICATED_USER] == null)
                Response.Redirect("Login.aspx");
                
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string source = txtSource.Text;
            string destination = txtDestination.Text;

            DateTime from = DateTime.MinValue;
            DateTime to = DateTime.MaxValue;
            DateTime now = DateTime.UtcNow;
            int lastPeriod = Convert.ToInt32(ddlPeriod.SelectedValue);
            if (lastPeriod != -1)
            {
                to = now;
                from = to.AddMinutes(-1 * lastPeriod);
            }
            else
            {
                to = now;
                from = SqlDateTime.MinValue.Value;
            }

            DataCommandMgr dataCommandMgr = new DataCommandMgr();
            List<IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand> dataCommands = dataCommandMgr.GetDataCommands(true, source, destination, from, to);
            dataCommands = dataCommands.OrderByDescending(dataReport => dataReport.Timestamp).ToList<IRIDIUM_GMDSS_LRIT.Core.Entity.DataCommand>();
            List<IRIDIUM_GMDSS_LRIT.Core.Entity.UI.Command> commands = new List<IRIDIUM_GMDSS_LRIT.Core.Entity.UI.Command>();
            dataCommands.ForEach(dataCommand => {
                Command command = Interpreter.DecodeComandFromTerminal(dataCommand.Data);
                command.Id = dataCommand.Id.ToString();
                command.Direction = dataCommand.Direction.ToString();
                command.Type = dataCommand.Type;
                command.Raw = dataCommand.Data;
                command.Timestamp = dataCommand.Timestamp;
                commands.Add(command);
            });

            gvDataCommand.DataSource = commands;
            gvDataCommand.DataBind();
        }
    }
}