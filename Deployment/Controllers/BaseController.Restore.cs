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
            IOUtil.DeleteDirectory(siteDirectory, true);

            Zip.UnZipFiles(backupFile, siteDirectory, true);
        }
    }
}