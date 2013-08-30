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
        protected ActionResult DownloadSiteConfigZip(Site site)
        {
            var domain = IOUtil.GetDomainName(site.PrimarySiteDirectory);
            var usingUnc = domain.Length > 0;
            var tempFile = Path.GetTempFileName();

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
                // Action
                var primaryDirectory = site.PrimarySiteDirectory;
                var files = Directory.GetFiles(primaryDirectory, "*.config");
                
                Zip.ZipFiles(primaryDirectory, files, tempFile);
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

            // Response
            var memoryStream = new MemoryStream();
            using (var file = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                memoryStream.Write(bytes, 0, (int)file.Length);
                file.Close();
            }

            System.IO.File.Delete(tempFile);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, "application/zip", "config.zip");
        }

        [NonAction]
        protected ActionResult DownloadSiteZip(Site site)
        {
            var domain = IOUtil.GetDomainName(site.PrimarySiteDirectory);
            var usingUnc = domain.Length > 0;
            var tempFile = Path.GetTempFileName();

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

                // Action
                var primaryDirectory = site.PrimarySiteDirectory;
                Zip.ZipFiles(primaryDirectory, tempFile);
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

            // Response
            var memoryStream = new MemoryStream();
            using (var file = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                memoryStream.Write(bytes, 0, (int)file.Length);
                file.Close();
            }

            System.IO.File.Delete(tempFile);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return File(memoryStream, "application/zip", "Site.zip");
        }
    }
}