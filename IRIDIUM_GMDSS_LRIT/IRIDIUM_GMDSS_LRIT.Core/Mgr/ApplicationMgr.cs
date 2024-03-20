using IRIDIUM_GMDSS_LRIT.Core.Dal;
using IRIDIUM_GMDSS_LRIT.Core.Forwarder.Interface;
using IRIDIUM_GMDSS_LRIT.Core.Utility;
using IRIDIUM_GMDSS_LRIT.Core.WcfService.Entity;
using IRIDIUM_GMDSS_LRIT.Core.WcfService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIDIUM_GMDSS_LRIT.Core.Mgr
{
    public class ApplicationMgr
    {
        public List<Application> GetApplications(bool active)
        {
            ApplicationDal applicationDal = new ApplicationDal();
            List<Application> applications = applicationDal.GetApplications(active);
            foreach (Application application in applications)
            {
                string dependencyFilename = application.ForwarderFullAssemblyClassName.Split(new char[] { ',' }, StringSplitOptions.None)[1].Trim() + ".dll";
                string dependencyFullClass = application.ForwarderFullAssemblyClassName.Split(new char[] { ',' }, StringSplitOptions.None)[0].Trim();
                application.Forwarder = ReflectorUtility.GetDependency<IPositionForwarder>(dependencyFilename, dependencyFullClass);
                application.Forwarder.EndPoint = application.ForwarderEndpoint;
            }
            return applications;
        }

        public List<Application> GetApplications()
        {
            ApplicationDal applicationDal = new ApplicationDal();
            List<Application> applications = applicationDal.GetApplications();
            foreach (Application application in applications)
            {
                string dependencyFilename = application.ForwarderFullAssemblyClassName.Split(new char[] { ',' }, StringSplitOptions.None)[1].Trim() + ".dll";
                string dependencyFullClass = application.ForwarderFullAssemblyClassName.Split(new char[] { ',' }, StringSplitOptions.None)[0].Trim();
                application.Forwarder = ReflectorUtility.GetDependency<IPositionForwarder>(dependencyFilename, dependencyFullClass);
                application.Forwarder.EndPoint = application.ForwarderEndpoint;
            }
            return applications;
        }

        public List<Application> GetApplicationsForDisplay(bool active)
        {
            ApplicationDal applicationDal = new ApplicationDal();
            return applicationDal.GetApplications(active);
        }

        public Application GetApplication(string applicationId)
        {
            ApplicationDal applicationDal = new ApplicationDal();
            return applicationDal.GetApplication(applicationId);
        }

        public IPositionForwarder GetApplicationForwarder(string applicationId)
        {
            IPositionForwarder forwarder = null;
            List<Application> applications = GetApplications();
            foreach (Application application in applications)
            {
                if (application.Id.Equals(applicationId))
                {
                    forwarder = application.Forwarder;
                    break;
                }
            }
            return forwarder;
        }
    }
}
