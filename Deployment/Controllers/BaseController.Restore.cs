using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        [NonAction]
        protected void RestoreAllSite(Backup backup, Site site)
        {
            var backupPath = site.BackupDirectory + Path.DirectorySeparatorChar + backup.Name + ".zip";
            var tempFile = CopyUncFileToTempFile(backupPath, site);

            var siteDirectoryList = site.SecondarySiteDirectory.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var siteDirectory in siteDirectoryList)
            {
                RestoreSite(tempFile, siteDirectory, site);
            }

            System.IO.File.Delete(tempFile);
        }

        [NonAction]
        protected void RestoreSite(string backupFile, string siteDirectory, Site site)
        {
            Impersonate impersonate = null;
            IntPtr tokenHandle = IntPtr.Zero;
            WindowsImpersonationContext impersonatedUser = null;
            var domain = IOUtil.GetDomainName(siteDirectory);
            var usingUnc = domain.Length > 0;

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
                IOUtil.DeleteDirectory(siteDirectory, true);
                Zip.UnZipFiles(backupFile, siteDirectory, true);
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
        }
    }
}