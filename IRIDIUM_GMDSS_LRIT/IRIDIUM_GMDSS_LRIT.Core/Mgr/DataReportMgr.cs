using IRIDIUM_GMDSS_LRIT.Core.Dal;
using IRIDIUM_GMDSS_LRIT.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Mgr
{
    public class DataReportMgr
    {
        DataReportDal dataReportDal;
        public DataReportMgr()
        {
            this.dataReportDal = new DataReportDal();
        }

        public void InsertDataReport(DataReport dataReport)
        {
            this.dataReportDal.InsertDataReport(dataReport);
        }

        public void UpdateDataReport(DataReport dataReport)
        {
            this.dataReportDal.UpdateDataReport(dataReport);
        }

        public List<DataReport> GetDataReports(string source, DateTime from, DateTime to)
        {
            return this.dataReportDal.GetDataReports(source, from, to);
        }

        public List<DataReport> GetDataReports(ReportStatus status)
        {
            return this.dataReportDal.GetDataReports(status);
        }

    }
}
