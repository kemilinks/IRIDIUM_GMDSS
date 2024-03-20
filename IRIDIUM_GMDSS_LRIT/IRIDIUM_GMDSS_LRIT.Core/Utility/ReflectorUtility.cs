using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Utility
{
    public class ReflectorUtility
    {
        public static T GetDependency<T>(string dependencyFilename, string dependencyFullClass)
        {
            try
            {
                SystemConfigUtility systemConfigUtility = new SystemConfigUtility();
                string DynamicConfigHandlerPath = systemConfigUtility.GetDependencyFileStoreLocation() + dependencyFilename;
                Assembly _Assembly = Assembly.LoadFile(DynamicConfigHandlerPath);
                Type _Type = _Assembly.GetType(dependencyFullClass);
                Object obj = Activator.CreateInstance(_Type);
                T dependency = (T)obj;
                return dependency;
            }
            catch (Exception ex)
            {
                KemiLogger.LogWriter.Log(KemiLogger.LogWriter.Level.SEVERE, "Unable to load Dependency from HandlerName: " + dependencyFilename + ", HandlerClass " + dependencyFullClass, ex.Message);
                return default(T);
            }
        }
    }
}
