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

        public string GetSystemAdminNumber()
        {
            ISystemConfig sysConfig = GetSysConfig(Keys.KEY_SYSTEM_ADMIN_NUMBER);
            if (sysConfig != null)
            {
                string value = sysConfig.SysValue;
                if (!string.IsNullOrEmpty(value))
                    return value;
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Empty value of  KEY_SYSTEM_ADMIN_NUMBER in System Configuration Table. Empty string is used", string.Empty);
                    return string.Empty;
                }
            }
            else
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "KEY_SYSTEM_ADMIN_NUMBER is NULL from System Configuration Table. Empty string is used", string.Empty);
                return string.Empty;
            }
        }

        public void UpdateLastEnquireLinkReceivedDateTime(DateTime input)
        {
            try
            {
                if (GetSysConfig(Keys.KEY_LAST_ENQUIRE_LINK_RECEIVED_DATETIME) != null)
                {
                    ISystemConfig sysConfig = new SystemConfig();
                    sysConfig.SysKey = Keys.KEY_LAST_ENQUIRE_LINK_RECEIVED_DATETIME;
                    sysConfig.SysValue = CommonUtility.ConvertDateTimeToString(input, CommonUtility._SYSTEM_DATE_TIME_FORMAT);
                    UpdateSysConfig(sysConfig);
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Update Last Enquire Last Received DateTime", ex.Message);
            }
        }

        public DateTime GetLastEnquireLinkReceivedDateTime()
        {
            ISystemConfig sysConfig = GetSysConfig(Keys.KEY_LAST_ENQUIRE_LINK_RECEIVED_DATETIME);
            if (sysConfig != null)
            {
                string value = sysConfig.SysValue;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        return CommonUtility.ConvertStringToDateTime(value, CommonUtility._SYSTEM_DATE_TIME_FORMAT);
                    }
                    catch (Exception ex)
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get Last Enquire Last Received DateTime", ex.Message);
                        return DateTime.MaxValue;
                    }
                }
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Empty value of  KEY_LAST_ENQUIRE_LINK_RECEIVED in System Configuration Table. Empty string is used", string.Empty);
                    return DateTime.MaxValue;
                }
            }
            else
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "KEY_LAST_ENQUIRE_LINK_RECEIVED is NULL from System Configuration Table. Empty string is used", string.Empty);
                return DateTime.MaxValue;
            }
        }

        public void UpdateLastSmppServerConnectionDateTime(DateTime input)
        {
            try
            {
                if (GetSysConfig(Keys.KEY_LAST_SMPP_SERVER_LAST_CONNECTION_DATETIME) != null)
                {
                    ISystemConfig sysConfig = new SystemConfig();
                    sysConfig.SysKey = Keys.KEY_LAST_SMPP_SERVER_LAST_CONNECTION_DATETIME;
                    sysConfig.SysValue = CommonUtility.ConvertDateTimeToString(input, CommonUtility._SYSTEM_DATE_TIME_FORMAT);
                    UpdateSysConfig(sysConfig);
                }
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Update Last Smpp Server Connection DateTime", ex.Message);
            }
        }

        public DateTime GetLastSmppServerConnectionDateTime()
        {
            ISystemConfig sysConfig = GetSysConfig(Keys.KEY_LAST_SMPP_SERVER_LAST_CONNECTION_DATETIME);
            if (sysConfig != null)
            {
                string value = sysConfig.SysValue;
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        return CommonUtility.ConvertStringToDateTime(value, CommonUtility._SYSTEM_DATE_TIME_FORMAT);
                    }
                    catch (Exception ex)
                    {
                        KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to Get Last Smpp Server Connection DateTime", ex.Message);
                        return DateTime.MaxValue;
                    }
                }
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Empty value of  KEY_LAST_SMPP_SERVER_LAST_CONNECTION_DATETIME in System Configuration Table. Empty string is used", string.Empty);
                    return DateTime.MaxValue;
                }
            }
            else
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "KEY_LAST_SMPP_SERVER_LAST_CONNECTION_DATETIME is NULL from System Configuration Table. Empty string is used", string.Empty);
                return DateTime.MaxValue;
            }
        }

        public string GetWebUIAdminAccounts()
        {
            ISystemConfig sysConfig = GetSysConfig(Keys.KEY_WEB_ADMIN_USER_ACCOUNTS);
            if (sysConfig != null)
            {
                string value = sysConfig.SysValue;
                if (!string.IsNullOrEmpty(value))
                    return value;
                else
                {
                    KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Empty value of  KEY_WEB_ADMIN_USER_ACCOUNTS in System Configuration Table. Empty string is used", string.Empty);
                    return string.Empty;
                }
            }
            else
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "KEY_WEB_ADMIN_USER_ACCOUNTS is NULL from System Configuration Table. Empty string is used", string.Empty);
                return string.Empty;
            }
        }
    }
}
