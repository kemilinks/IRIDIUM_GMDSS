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
    public class ApplicationDal
    {
        private string DEFAULT_SELECT_STATEMENT = "SELECT * FROM [APPLICATION] ";

        public int InsertApplication(Application application)
        {
            int affectedRow = 0;
            try
            {
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    string sql = @"INSERT INTO [APPLICATION]
                                            ([ID],[NAME],[ACCESS_CODE],[DESCRIPTION],[EXTRA_INFO],[FORWARDER_FULL_ASSEMBLY_CLASS_NAME],[FORWARDER_ENDPOINT],[ACTIVE])
                                VALUES(@ID,@NAME,@ACCESS_CODE,@DESCRIPTION,@FORWARDER_FULL_ASSEMBLY_CLASS_NAME,@FORWARDER_ENDPOINT,@ACTIVE)";
                    using (SqlCommand Command = new SqlCommand(sql, Connection))
                    {
                        Command.Parameters.AddWithValue("@ID", application.Id);
                        Command.Parameters.AddWithValue("@NAME", application.Name);
                        Command.Parameters.AddWithValue("@ACCESS_CODE", application.AccessCode);
                        Command.Parameters.AddWithValue("@DESCRIPTION", application.Description);
                        Command.Parameters.AddWithValue("@EXTRA_INFO", application.ExtraInfo);
                        Command.Parameters.AddWithValue("@FORWARDER_FULL_ASSEMBLY_CLASS_NAME", application.ForwarderFullAssemblyClassName);
                        Command.Parameters.AddWithValue("@FORWARDER_ENDPOINT", application.ForwarderEndpoint);
                        Command.Parameters.AddWithValue("@ACTIVE", application.Active);
                        affectedRow = Command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable To Insert Application", ex.Message);
            }
            return affectedRow;
        }

        public List<Application> GetApplications()
        {
            List<Application> applications = new List<Application>();
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT;
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Application application = ConstructApplication(reader);
                                if (application != null)
                                    applications.Add(application);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get All Applications", ex.Message);
                return new List<Application>();
            }
            return applications;
        }

        public List<Application> GetApplications(bool active)
        {
            List<Application> applications = new List<Application>();
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + " WHERE [ACTIVE] = @ACTIVE";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ACTIVE", active);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Application application = ConstructApplication(reader);
                                if (application != null)
                                    applications.Add(application);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get All Applications by Active", ex.Message);
                return new List<Application>();
            }
            return applications;
        }

        public Application GetApplication(string id)
        {
            Application application = null;
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + " WHERE [ID] = @ID";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ID", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                application = ConstructApplication(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get Application by Id", ex.Message);
            }
            return application;
        }

        private Application ConstructApplication(IDataReader reader)
        {
            Application application = new Application();
            application.AccessCode = Convert.ToString(reader["ACCESS_CODE"]);
            application.Active = Convert.ToBoolean(reader["ACTIVE"]);
            application.Description = Convert.ToString(reader["DESCRIPTION"]);
            application.ExtraInfo = Convert.ToString(reader["EXTRA_INFO"]);
            application.ForwarderEndpoint = Convert.ToString(reader["FORWARDER_ENDPOINT"]);
            application.ForwarderFullAssemblyClassName = Convert.ToString(reader["FORWARDER_FULL_ASSEMBLY_CLASS_NAME"]);
            application.Id = Convert.ToString(reader["ID"]);
            application.Name = Convert.ToString(reader["NAME"]);
            return application;
        }
    }
}
