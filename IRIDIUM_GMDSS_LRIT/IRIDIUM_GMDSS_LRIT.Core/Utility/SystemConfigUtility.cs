using IRIDIUM_GMDSS_LRIT.Core.Entity;
using KemiAppCommon;
using KemiAppCommon.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Utility
{
    public class SystemConfigUtility
    {
        public ISystemConfig GetSysConfig(string sysKey)
        {
            ISystemConfigRepository sysConfigRep = AppCommonRepositoryMgr.GetSystemConfigRepository();
            return sysConfigRep.GetSystemConfig(sysKey);
        }

        public bool UpdateSysConfig(ISystemConfig config)
        {
            ISystemConfigRepository sysConfigRep = AppCommonRepositoryMgr.GetSystemConfigRepository();
            return sysConfigRep.Update(config);
        }

        public string GetDependencyFileStoreLocation()
        {
            ISystemConfig sysConfig = GetSysConfig(Keys.KEY_PATH_DEPENDENCY_FILE_STORE_LOCATION);
            if (sysConfig != null)
            {
                string value = sysConfig.SysValue;
                if (!string.IsNullOrEmpty(value))
                    return value;
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Empty value of  DEPENDENCY_FILE_STORE_LOCATION in System Configuration Table. Empty string is used", string.Empty);
                    return string.Empty;
                }
            }
            else
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "DEPENDENCY_FILE_STORE_LOCATION is NULL from System Configuration Table. Empty string is used", string.Empty);
                return string.Empty;
            }
        }

        public string GetLRITShortCode()
        {
            ISystemConfig sysConfig = GetSysConfig(Keys.KEY_LRIT_SHORT_CODE);
            if (sysConfig != null)
            {
                string value = sysConfig.SysValue;
                if (!string.IsNullOrEmpty(value))
                    return value;
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Empty value of  KEY_LRIT_SHORT_CODE in System Configuration Table. Empty string is used", string.Empty);
                    return string.Empty;
                }
            }
            else
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "KEY_LRIT_SHORT_CODE is NULL from System Configuration Table. Empty string is used", string.Empty);
                return string.Empty;
            }
        }
    }
}
