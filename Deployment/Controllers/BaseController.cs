using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deployment.Models;
using Deployment.Helper;
using System.IO;
using System.Security;
using System.Security.Principal;

namespace Deployment.Controllers
{
    public partial class BaseController : Controller
    {
        protected Entities db = new Entities();

        [NonAction]
        protected string CopyUncFileToTempFile(string uncPath, Site site)
        {
            var tempFile = Path.GetTempFileName();

            var backupDirectory = site.BackupDirectory;
            var domain = IOUtil.GetDomainName(backupDirectory);

            if (domain.Length == 0)
            {
                throw new ArgumentException("Site package directory is invalid");
            }

            // Login to domain
            var Impersonate = new Impersonate();
            var tokenHandle = Impersonate.StartUserSession(site.WindowsUsername, domain, site.WindowsPassword);

            var newId = new WindowsIdentity(tokenHandle);
            var impersonatedUser = newId.Impersonate();

            try
            {
                System.IO.File.Copy(uncPath, tempFile, true);
            }
            finally
            {
                // Logoff 
                impersonatedUser.Undo();
                Impersonate.EndUserSession(tokenHandle);
            }

            return tempFile;
        }
    }
}
