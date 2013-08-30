using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Deployment.Helper;
using Deployment.Models;

namespace Deployment.Controllers
{
    public partial class BaseController
    {
        [NonAction]
        protected void SavePackageFile(Package package, Site site, HttpPostedFileBase uploadFile)
        {
            // Save to temp file
            var uploadFileName = package.Name + ".zip";
            var tempFile = Path.GetTempFileName();
            uploadFile.SaveAs(tempFile);

            var packageDirectory = site.PackageDirectory;
            var domain = IOUtil.GetDomainName(packageDirectory);

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
                // Save
                var packagePath = packageDirectory + Path.DirectorySeparatorChar + uploadFileName;
                System.IO.File.Copy(tempFile, packagePath, true);
                System.IO.File.Delete(tempFile);

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

        [NonAction]
        protected void ReleasePackage(Package package, Site site)
        {
            // Copy package to temp files
            var originalSite = db.Sites.SingleOrDefault(s => s.Id == package.SiteId);
            if (originalSite == null)
            {
                return;
            }

            var packagePath = originalSite.PackageDirectory + Path.DirectorySeparatorChar + package.Name + ".zip";
            var tempFile = CopyUncFileToTempFile(packagePath, site);

            // Backup
            BackupPrimarySite(site, package.Id, package.Name);

            // Release all sites
            ReleaseToAllSiteDirectory(tempFile, package, site);

            System.IO.File.Delete(tempFile);
        }

        [NonAction]
        protected void ReleaseToAllSiteDirectory(string packageFile, Package package, Site site)
        {
            var siteDirectoryList = site.SecondarySiteDirectory.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var siteDirectory in siteDirectoryList)
            {
                ReleaseToASiteDirectory(packageFile, siteDirectory, site);
            }

            // Save to release log
            var release = new Release
            {
                SiteId = site.Id,
                Name = package.Name,
                Description = package.Description,
                Creator = User.Identity.Name,
                ReleaseDate = DateTime.Now
            };

            db.Releases.AddObject(release);
            db.SaveChanges();
        }

        [NonAction]
        protected void ReleaseToASiteDirectory(string packageFile, string siteDirectory, Site site)
        {
            var domain = IOUtil.GetDomainName(siteDirectory);

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
                // Release
                Zip.UnZipFiles(packageFile, siteDirectory, true);
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