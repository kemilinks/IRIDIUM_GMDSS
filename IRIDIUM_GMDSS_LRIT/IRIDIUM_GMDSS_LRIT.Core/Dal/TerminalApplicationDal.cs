using IRIDIUM_GMDSS_LRIT.Core.WcfService.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Dal
{
    public class TerminalApplicationDal
    {
        public Dictionary<string, AccessLevel> GetApplicationIdsWithAccessLevel(Int64 terminalId)
        {
            Dictionary<string, AccessLevel> applicationsAccessLevel = new Dictionary<string, AccessLevel>();
            try
            {
                string sql = "SELECT * FROM [TERMINAL_APPLICATION] WHERE [TERMINAL_ID] = @TERMINAL_ID";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TERMINAL_ID", terminalId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                applicationsAccessLevel.Add(Convert.ToString(reader["APPLICATION_ID"]), (AccessLevel)Enum.Parse(typeof(AccessLevel), Convert.ToString(reader["ACCESS_LEVEL"])));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get Application Ids with Access Level by Terminal Id", ex.Message);
            }
            return applicationsAccessLevel;
        }

        public KeyValuePair<string, AccessLevel> GetApplicationIdsWithAccessLevel(Int64 terminalId, string applicationId)
        {
            KeyValuePair<string, AccessLevel> applicationsAccessLevel = new KeyValuePair<string, AccessLevel>();
            try
            {
                string sql = "SELECT * FROM [TERMINAL_APPLICATION] WHERE [TERMINAL_ID] = @TERMINAL_ID AND [APPLICATION_ID] = @APPLICATION_ID";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@TERMINAL_ID", terminalId);
                        cmd.Parameters.AddWithValue("@APPLICATION_ID", applicationId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                applicationsAccessLevel = new KeyValuePair<string, AccessLevel>(Convert.ToString(reader["APPLICATION_ID"]), (AccessLevel)Enum.Parse(typeof(AccessLevel), Convert.ToString(reader["ACCESS_LEVEL"])));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get Application Id with Access Level by Terminal Id and Application Id", ex.Message);
            }
            return applicationsAccessLevel;
        }

        public int InsertApplicationTerminal(Int64 terminalId, string applicationId, AccessLevel accessLevel)
        {
            int affectedRow = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    string sql = @"INSERT INTO [TERMINAL_APPLICATION]
                                       ([TERMINAL_ID],[APPLICATION_ID],[ACCESS_LEVEL])
                                 VALUES
                                       (@TERMINAL_ID,@APPLICATION_ID,@ACCESS_LEVEL)";
                    using (SqlCommand Command = new SqlCommand(sql, Connection))
                    {
                        Command.Parameters.AddWithValue("@TERMINAL_ID", terminalId);
                        Command.Parameters.AddWithValue("@APPLICATION_ID", applicationId);
                        Command.Parameters.AddWithValue("@ACCESS_LEVEL", accessLevel);
                        affectedRow = Command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable To Insert Application Terminal", ex.Message);
            }
            return affectedRow;
        }

        public int DeleteApplicationTerminal(Int64 terminalId)
        {
            int affectedRow = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    string sql = @"DELETE FROM [TERMINAL_APPLICATION] WHERE [TERMINAL_ID] = @TERMINAL_ID";
                    using (SqlCommand Command = new SqlCommand(sql, Connection))
                    {
                        Command.Parameters.AddWithValue("@TERMINAL_ID", terminalId);
                        affectedRow = Command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable To Delete Applications for Terminal", ex.Message);
            }
            return affectedRow;
        }
    }
}
