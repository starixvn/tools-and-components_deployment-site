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
            var usingUnc = domain.Length > 0;

            Impersonate impersonate = null;
            IntPtr tokenHandle = IntPtr.Zero;
            WindowsImpersonationContext impersonatedUser = null;

            if (usingUnc)
            {
                // Login to domain
                impersonate = new Impersonate();
                tokenHandle = impersonate.StartUserSession(site.WindowsUsername, domain, site.WindowsPassword);

                var newId = new WindowsIdentity(tokenHandle);
                impersonatedUser = newId.Impersonate();
            }

            try
            {
                System.IO.File.Copy(uncPath, tempFile, true);
            }
            finally
            {
                if (usingUnc)
                {
                    // Logoff 
                    impersonatedUser.Undo();
                    impersonate.EndUserSession(tokenHandle);
                }
            }

            return tempFile;
        }
    }
}
