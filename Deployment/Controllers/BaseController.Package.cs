using System;
using System.IO;
using System.Linq;
using System.Web;
using Deployment.Models;
using Deployment.Helper;
using System.Security;
using System.Security.Principal;
using System.Web.Mvc;

namespace Deployment.Controllers
{
    public partial class BaseController
    {
        [NonAction]
        protected void BackupPrimarySite(Site site, int packageId = 0, string backupName = "")
        {
            backupName = packageId == 0
                ? string.Format("{0}-Manual-Backup-Date-{1:yyyyMMdd}-Time-{1:HH-mm}", site.Name, DateTime.Now)
                : backupName;
            var domain = IOUtil.GetDomainName(site.PrimarySiteDirectory);
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

            // Backup
            try
            {
                var zipFile = site.BackupDirectory + Path.DirectorySeparatorChar + backupName + ".zip";
                Zip.ZipFiles(site.PrimarySiteDirectory, zipFile);

                // Save to database
                var backupDescription = packageId == 0
                    ? "Manual backup at :" + DateTime.Now
                    : string.Format("Backup before deploying the package {0}: {1}",
                    packageId,
                    backupName);

                var backup = new Backup
                {
                    SiteId = site.Id,
                    Name = backupName,
                    Description = backupDescription,
                    Creator = User.Identity.Name,
                    CreatedDate = DateTime.Now
                };

                db.Backups.AddObject(backup);

                db.SaveChanges();
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