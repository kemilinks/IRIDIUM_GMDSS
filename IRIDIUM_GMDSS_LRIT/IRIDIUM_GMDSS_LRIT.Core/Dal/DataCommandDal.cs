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
    public class DataCommandDal
    {
        private string DEFAULT_SELECT_STATEMENT = "SELECT * FROM [DATA_COMMAND] ";
        public void InsertDataCommand(DataCommand command)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    string sql = @"INSERT INTO [DATA_COMMAND]
                                       ([SOURCE],[TYPE],[DESTINATION],[DATA],[REFERENCE_NUMBER],[DIRECTION],[STATUS],[TIMESTAMP])
                                 VALUES
                                       (@SOURCE,@TYPE,@DESTINATION,@DATA,@REFERENCE_NUMBER,@DIRECTION,@STATUS,@TIMESTAMP)";
                    using (SqlCommand Command = new SqlCommand(sql, Connection))
                    {
                        Command.Parameters.AddWithValue("@SOURCE", command.Source);
                        Command.Parameters.AddWithValue("@DESTINATION", command.Destination);
                        Command.Parameters.AddWithValue("@DATA", command.Data);
                        Command.Parameters.AddWithValue("@REFERENCE_NUMBER", command.ReferenceNumber);
                        Command.Parameters.AddWithValue("@DIRECTION", command.Direction);
                        Command.Parameters.AddWithValue("@STATUS", command.Status);
                        Command.Parameters.AddWithValue("@TIMESTAMP", command.Timestamp);
                        Command.Parameters.AddWithValue("@TYPE", command.Type);

                        Command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable To Insert  Data Command", ex.Message);
            }
        }

        public void UpdateDataCommand(DataCommand command)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    string sql = @"UPDATE [DATA_COMMAND]
                                       SET [SOURCE] = @SOURCE
                                          ,[DESTINATION] = @DESTINATION
                                          ,[DATA] = @DATA
                                          ,[REFERENCE_NUMBER] = @REFERENCE_NUMBER
                                          ,[DIRECTION] = @DIRECTION
                                          ,[STATUS] = @STATUS
                                          ,[TIMESTAMP] = @TIMESTAMP
                                          ,[TYPE] = @TYPE
                                     WHERE [ID] = @ID";
                    using (SqlCommand Command = new SqlCommand(sql, Connection))
                    {
                        Command.Parameters.AddWithValue("@SOURCE", command.Source);
                        Command.Parameters.AddWithValue("@DESTINATION", command.Destination);
                        Command.Parameters.AddWithValue("@DATA", command.Data);
                        Command.Parameters.AddWithValue("@DIRECTION", command.Direction);
                        Command.Parameters.AddWithValue("@STATUS", command.Status);
                        Command.Parameters.AddWithValue("@TIMESTAMP", command.Timestamp);
                        Command.Parameters.AddWithValue("@ID", command.Id);
                        Command.Parameters.AddWithValue("@REFERENCE_NUMBER", command.ReferenceNumber);
                        Command.Parameters.AddWithValue("@TYPE", command.Type);
                        Command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable To Update  Data Command", ex.Message);
            }
        }

        public List<DataCommand> GetDataCommands(string source)
        {
            List<DataCommand> commands = new List<DataCommand>();
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + "WHERE [SOURCE] = @SOURCE";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@SOURCE", source);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataCommand command = ConstructDataCommand(reader);
                                if (command != null)
                                    commands.Add(command);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to get Data Command by Source", ex.Message);
            }
            return commands;
        }

        public List<DataCommand> GetDataCommands(DateTime from, DateTime to)
        {
            List<DataCommand> commands = new List<DataCommand>();
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + "WHERE [TIMESTAMP] BETWEEN @FROM AND @TO";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@FROM", from);
                        cmd.Parameters.AddWithValue("@TO", to);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataCommand command = ConstructDataCommand(reader);
                                if (command != null)
                                    commands.Add(command);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to get Data Command by From and To", ex.Message);
            }
            return commands;
        }

        public List<DataCommand> GetDataCommands(bool is2Ways, string source, string destination, DateTime from, DateTime to)
        {
            List<DataCommand> commands = new List<DataCommand>();
            try
            {
                string sql = string.Empty;
                if(is2Ways)
                    sql = DEFAULT_SELECT_STATEMENT + "WHERE ([TIMESTAMP] BETWEEN @FROM AND @TO) AND (([SOURCE] = @SOURCE AND [DESTINATION] = @DESTINATION) OR (([SOURCE] = @DESTINATION AND [DESTINATION] = @SOURCE)))";
                else
                    sql = DEFAULT_SELECT_STATEMENT + "WHERE [TIMESTAMP] BETWEEN (@FROM AND @TO) AND ([SOURCE] = @SOURCE AND [DESTINATION] = @DESTINATION)";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@FROM", from);
                        cmd.Parameters.AddWithValue("@TO", to);
                        cmd.Parameters.AddWithValue("@SOURCE", source);
                        cmd.Parameters.AddWithValue("@DESTINATION", destination);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataCommand command = ConstructDataCommand(reader);
                                if (command != null)
                                    commands.Add(command);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to get Data Command by From and To", ex.Message);
            }
            return commands;
        }

        public List<DataCommand> GetDataCommands(Direction direction, CommandStatus status)
        {
            List<DataCommand> commands = new List<DataCommand>();
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + "WHERE [DIRECTION] = @DIRECTION AND [STATUS] = @STATUS";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@DIRECTION", direction);
                        cmd.Parameters.AddWithValue("@STATUS", status);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataCommand command = ConstructDataCommand(reader);
                                if (command != null)
                                    commands.Add(command);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to get Data Command by direction and status", ex.Message);
            }
            return commands;
        }

        private DataCommand ConstructDataCommand(IDataReader reader)
        {
            DataCommand command = new DataCommand();
            command.Data = Convert.ToString(reader["DATA"]);
            command.Destination = Convert.ToString(reader["DESTINATION"]);
            command.Direction = (Direction)Enum.Parse(typeof(Direction),Convert.ToString(reader["DIRECTION"]));
            command.Id = Convert.ToInt64(reader["ID"]);
            command.Timestamp = Convert.ToDateTime(reader["TIMESTAMP"]);
            command.Source = Convert.ToString(reader["SOURCE"]);
            command.Status = (CommandStatus)Enum.Parse(typeof(CommandStatus), Convert.ToString(reader["STATUS"]));
            command.ReferenceNumber = Convert.ToString(reader["REFERENCE_NUMBER"]);
            command.Type = Convert.ToString(reader["TYPE"]);
            return command;
        }
    }
}
