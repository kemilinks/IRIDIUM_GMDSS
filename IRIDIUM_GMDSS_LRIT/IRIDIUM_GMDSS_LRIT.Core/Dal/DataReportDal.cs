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
    public class DataReportDal
    {
        private string DEFAULT_SELECT_STATEMENT = "SELECT * FROM [DATA_REPORT] ";
        public void InsertDataReport(DataReport report)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    string sql = @"INSERT INTO [DATA_REPORT]
                                           ([SOURCE],[DATA],[STATUS],[REMARKS],[RECEIVE_TIMESTAMP])
                                     VALUES
                                           (@SOURCE,@DATA,@STATUS,@REMARKS,@RECEIVE_TIMESTAMP)";
                    using (SqlCommand Command = new SqlCommand(sql, Connection))
                    {
                        Command.Parameters.AddWithValue("@SOURCE", report.Source);
                        Command.Parameters.AddWithValue("@DATA", report.Data);
                        Command.Parameters.AddWithValue("@STATUS", report.Status);
                        Command.Parameters.AddWithValue("@REMARKS", report.Remarks);
                        Command.Parameters.AddWithValue("@RECEIVE_TIMESTAMP", report.ReceivedTimestamp);

                        Command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable To Insert  Data Report", ex.Message);
            }
        }

        public void UpdateDataReport(DataReport report)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    string sql = @"UPDATE [DATA_REPORT]
                                   SET [SOURCE] = @SOURCE
                                      ,[DATA] = @DATA
                                      ,[STATUS] = @STATUS
                                      ,[REMARKS] = @REMARKS
                                      ,[RECEIVE_TIMESTAMP] = @RECEIVE_TIMESTAMP
                                 WHERE [ID] = @ID";
                    using (SqlCommand Command = new SqlCommand(sql, Connection))
                    {
                        Command.Parameters.AddWithValue("@SOURCE", report.Source);
                        Command.Parameters.AddWithValue("@DATA", report.Data);
                        Command.Parameters.AddWithValue("@STATUS", report.Status);
                        Command.Parameters.AddWithValue("@REMARKS", report.Remarks);
                        Command.Parameters.AddWithValue("@RECEIVE_TIMESTAMP", report.ReceivedTimestamp);
                        Command.Parameters.AddWithValue("@ID", report.Id);
                        Command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable To Update  Data Report", ex.Message);
            }
        }

        public List<DataReport> GetDataReports(string source)
        {
            List<DataReport> reports = new List<DataReport>();
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
                                DataReport report = ConstructDataReport(reader);
                                if (report != null)
                                    reports.Add(report);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to get Data Report by Source", ex.Message);
            }
            return reports;
        }

        public List<DataReport> GetDataReports(DateTime from, DateTime to)
        {
            List<DataReport> reports = new List<DataReport>();
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + "WHERE [RECEIVE_TIMESTAMP] BETWEEN @FROM AND @TO";
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
                                DataReport report = ConstructDataReport(reader);
                                if (report != null)
                                    reports.Add(report);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to get Data Report by From and To", ex.Message);
            }
            return reports;
        }

        public List<DataReport> GetDataReports(string source, DateTime from, DateTime to)
        {
            List<DataReport> reports = new List<DataReport>();
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + "WHERE [SOURCE] = @SOURCE AND [RECEIVE_TIMESTAMP] BETWEEN @FROM AND @TO";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@SOURCE", source);
                        cmd.Parameters.AddWithValue("@FROM", from);
                        cmd.Parameters.AddWithValue("@TO", to);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataReport report = ConstructDataReport(reader);
                                if (report != null)
                                    reports.Add(report);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to get Data Report by From and To", ex.Message);
            }
            return reports;
        }

        public List<DataReport> GetDataReports(ReportStatus status)
        {
            List<DataReport> reports = new List<DataReport>();
            try
            {
                string sql = DEFAULT_SELECT_STATEMENT + "WHERE [STATUS] = @STATUS";
                using (SqlConnection Connection = new SqlConnection(CommonDal.GetDBConnectionString()))
                {
                    Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, Connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@STATUS", status);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataReport report = ConstructDataReport(reader);
                                if (report != null)
                                    reports.Add(report);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to get Data Report by Status", ex.Message);
            }
            return reports;
        }

        private DataReport ConstructDataReport(IDataReader reader)
        {
            DataReport report = new DataReport();
            report.Data = Convert.ToString(reader["DATA"]);
            report.Id = Convert.ToInt64(reader["ID"]);
            report.ReceivedTimestamp = Convert.ToDateTime(reader["RECEIVE_TIMESTAMP"]);
            report.Source = Convert.ToString(reader["SOURCE"]);
            report.Status = (ReportStatus)Enum.Parse(typeof(ReportStatus), Convert.ToString(reader["STATUS"]));
            report.Remarks = Convert.ToString(reader["REMARKS"]);
            return report;
        }
    }
}
