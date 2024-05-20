using IRIDIUM_GMDSS_LRIT.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Dal
{
    public class TerminalDal
    {
        private string DEFAULT_SELECT_STATEMENT = "SELECT * FROM [TERMINAL] ";
        
        public void InsertTerminal(Terminal terminal, out int terminalId)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    string sql = @"INSERT INTO [TERMINAL]
                                           ([MSISDN],[IMO_NUMBER],[STATUS],[DESCRIPTION],[REMARK],[CREATION_TIMESTAMP],[ACTIVATION_TIMESTAMP],[DEACTIVATION_TIMESTAMP])
                                    OUTPUT INSERTED.ID 
                                    VALUES
                                           (@MSISDN,@IMO_NUMBER,@STATUS,@DESCRIPTION,@REMARK,@CREATION_TIMESTAMP,@ACTIVATION_TIMESTAMP,@DEACTIVATION_TIMESTAMP)";
                    using (SqlCommand Command = new SqlCommand(sql, Connection))
                    {
                        Command.Parameters.AddWithValue("@MSISDN", terminal.MSISDN);
                        Command.Parameters.AddWithValue("@IMO_NUMBER", terminal.IMONumber);
                        Command.Parameters.AddWithValue("@STATUS", terminal.Status);
                        Command.Parameters.AddWithValue("@DESCRIPTION", terminal.Description);
                        Command.Parameters.AddWithValue("@REMARK", terminal.Remark);
                        Command.Parameters.AddWithValue("@CREATION_TIMESTAMP", terminal.CreationTimestamp);
                        
                        if(terminal.ActivationTimestamp == DateTime.MaxValue)
                            Command.Parameters.AddWithValue("@ACTIVATION_TIMESTAMP", DBNull.Value);
                        else
                            Command.Parameters.AddWithValue("@ACTIVATION_TIMESTAMP", terminal.ActivationTimestamp);

                        if(terminal.DeactivationTimestamp == DateTime.MaxValue)
                            Command.Parameters.AddWithValue("@DEACTIVATION_TIMESTAMP", DBNull.Value);
                        else
                            Command.Parameters.AddWithValue("@DEACTIVATION_TIMESTAMP", terminal.DeactivationTimestamp);
                        terminalId = (Int32)Command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable To Insert  Terminal", ex.Message);
                terminalId = -1;
            }
        }

        public void UpdateTerminal(Terminal terminal)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    string sql = @"UPDATE [TERMINAL]
                                   SET [MSISDN] = @MSISDN
                                      ,[IMO_NUMBER] = @IMO_NUMBER
                                      ,[STATUS] = @STATUS
                                      ,[DESCRIPTION] = @DESCRIPTION
                                      ,[REMARK] = @REMARK
                                      ,[CREATION_TIMESTAMP] = @CREATION_TIMESTAMP
                                      ,[ACTIVATION_TIMESTAMP] = @ACTIVATION_TIMESTAMP
                                      ,[DEACTIVATION_TIMESTAMP] = @DEACTIVATION_TIMESTAMP
                                 WHERE [ID] = @ID";
                    using (SqlCommand Command = new SqlCommand(sql, Connection))
                    {
                        Command.Parameters.AddWithValue("@MSISDN", terminal.MSISDN);
                        Command.Parameters.AddWithValue("@IMO_NUMBER", terminal.IMONumber);
                        Command.Parameters.AddWithValue("@STATUS", terminal.Status);
                        Command.Parameters.AddWithValue("@DESCRIPTION", terminal.Description);
                        Command.Parameters.AddWithValue("@REMARK", terminal.Remark);
                        Command.Parameters.AddWithValue("@CREATION_TIMESTAMP", terminal.CreationTimestamp);

                        if (terminal.ActivationTimestamp == DateTime.MaxValue)
                            Command.Parameters.AddWithValue("@ACTIVATION_TIMESTAMP", DBNull.Value);
                        else
                            Command.Parameters.AddWithValue("@ACTIVATION_TIMESTAMP", terminal.ActivationTimestamp);

                        if (terminal.DeactivationTimestamp == DateTime.MaxValue)
                            Command.Parameters.AddWithValue("@DEACTIVATION_TIMESTAMP", DBNull.Value);
                        else
                            Command.Parameters.AddWithValue("@DEACTIVATION_TIMESTAMP", terminal.DeactivationTimestamp);

                        Command.Parameters.AddWithValue("@ID", terminal.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable To Update  Terminal", ex.Message);
            }
        }

        public Terminal GetTerminal(int terminalId)
        {
            Terminal terminal = null;
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + "WHERE [ID] = @ID";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ID", terminalId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                terminal = ConstructTerminal(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get Terminal by Terminal Id", ex.Message);
            }
            return terminal;
        }

        public Terminal GetTerminal(string msisdn)
        {
            Terminal terminal = null;
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + "WHERE [MSISDN] = @MSISDN";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@MSISDN", msisdn);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                terminal = ConstructTerminal(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get Terminal by msisdn", ex.Message);
            }
            return terminal;
        }

        public List<Terminal> GetTerminals()
        {
            List<Terminal> terminals = new List<Terminal>();
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT;
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Terminal terminal = ConstructTerminal(reader);
                                if (terminal != null)
                                    terminals.Add(terminal);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get All Terminal", ex.Message);
            }
            return terminals;
        }

        private Terminal ConstructTerminal(IDataReader reader)
        {
            Terminal terminal = new Terminal();
            if (reader["ACTIVATION_TIMESTAMP"] != DBNull.Value)
                terminal.ActivationTimestamp = Convert.ToDateTime(reader["ACTIVATION_TIMESTAMP"]);
            else
                terminal.ActivationTimestamp = DateTime.MaxValue;

            terminal.CreationTimestamp = Convert.ToDateTime(reader["CREATION_TIMESTAMP"]);

            if (reader["DEACTIVATION_TIMESTAMP"] != DBNull.Value)
                terminal.DeactivationTimestamp = Convert.ToDateTime(reader["DEACTIVATION_TIMESTAMP"]);
            else
                terminal.DeactivationTimestamp = DateTime.MaxValue;

            terminal.Description = Convert.ToString(reader["DESCRIPTION"]);

            terminal.Id = Convert.ToInt32(reader["ID"]);

            terminal.MSISDN = Convert.ToString(reader["MSISDN"]);

            terminal.Remark = Convert.ToString(reader["REMARK"]);

            terminal.Status = (TerminalStatus)Enum.Parse(typeof(TerminalStatus), Convert.ToString(reader["STATUS"]));

            terminal.IMONumber = Convert.ToString(reader["IMO_NUMBER"]);

            return terminal;
        }
    }
}
