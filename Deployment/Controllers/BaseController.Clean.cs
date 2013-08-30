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
        protected void CleanSiteFiles(Site site, string[] fileList)
        {
            var siteDirectoryList = site.SecondarySiteDirectory.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var siteDirectory in siteDirectoryList)
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
                    foreach (var file in fileList)
                    {
                        var fullPath = siteDirectory + Path.DirectorySeparatorChar + file;
                        var attr = System.IO.File.GetAttributes(fullPath);

                        //detect whether its a directory or file
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            IOUtil.DeleteDirectory(fullPath, false);
                        }
                        else
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
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

        [NonAction]
        protected void CleanWholeSite(Site site)
        {
            var siteDirectoryList = site.SecondarySiteDirectory.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var siteDirectory in siteDirectoryList)
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
                    IOUtil.DeleteDirectory(siteDirectory, true);
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
}