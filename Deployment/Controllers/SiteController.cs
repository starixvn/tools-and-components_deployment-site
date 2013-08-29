using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deployment.Models;
using System.IO;
using Deployment.Helper;
using System.Security.Principal;

namespace Deployment.Controllers
{
    public partial class SiteController : BaseController
    {

        public ActionResult Index(int projectId, string siteName = "")
        {
            var project = db.Projects.SingleOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return HttpNotFound();
            }

            var siteQuery = db.Sites.Where(s => s.ProjectId == projectId);
            siteQuery = string.IsNullOrEmpty(siteName)
                ? siteQuery
                : siteQuery.Where(p => p.Name.IndexOf(siteName) >= 0);
            siteQuery = siteQuery.OrderBy(s => s.Name);

            ViewBag.Project = project;
            ViewBag.SiteName = siteName;

            var siteList = siteQuery.ToList();
            return View(siteList);
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id = 0)
        {
            Site site = db.Sites.SingleOrDefault(s => s.Id == id);
            if (site == null)
            {
                return HttpNotFound();
            }

            return View(site);
        }

        //
        // GET: /Home/Create

        public ActionResult Create(int projectId)
        {
            var project = db.Projects.SingleOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return HttpNotFound();
            }

            var site = new Site { ProjectId = projectId };

            return View(site);
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(Site site)
        {
            var project = db.Projects.SingleOrDefault(p => p.Id == site.ProjectId);
            if (project == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                site.LastRestore = string.Empty;
                site.LastRestoreDate = new DateTime(2000, 1, 1);
                site.LastRestoreUser = string.Empty;
                db.Sites.AddObject(site);
                db.SaveChanges();
                return RedirectToAction("Index", new { projectId = site.ProjectId, siteName = "" });
            }

            return View(site);
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Site site = db.Sites.SingleOrDefault(s => s.Id == id);
            if (site == null)
            {
                return HttpNotFound();
            }

            return View(site);
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(Site site)
        {
            if (ModelState.IsValid)
            {
                var savedSite = db.Sites.SingleOrDefault(s => s.Id == site.Id);
                if (savedSite == null)
                {
                    return HttpNotFound();
                }

                savedSite.Name = site.Name;
                savedSite.Description = site.Description;
                savedSite.Url = site.Url;
                savedSite.PackageDirectory = site.PackageDirectory;
                savedSite.BackupDirectory = site.BackupDirectory;
                savedSite.PrimarySiteDirectory = site.PrimarySiteDirectory;
                savedSite.SecondarySiteDirectory = site.SecondarySiteDirectory;
                savedSite.WindowsPassword = string.IsNullOrEmpty(site.WindowsPassword)
                    ? savedSite.WindowsPassword
                    : site.WindowsPassword;
                savedSite.WindowsUsername = site.WindowsUsername;

                db.SaveChanges();
                return RedirectToAction("Index", new { projectId = savedSite.ProjectId, siteName = "" });
            }
            return View(site);
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Site site = db.Sites.SingleOrDefault(s => s.Id == id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Site site = db.Sites.SingleOrDefault(s => s.Id == id);
            db.Sites.DeleteObject(site);
            db.SaveChanges();
            return RedirectToAction("Index", new { projectId = site.ProjectId, siteName = "" });
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Backup(int id = 0)
        {
            Site site = db.Sites.SingleOrDefault(s => s.Id == id);
            if (site == null)
            {
                return HttpNotFound();
            }

            BackupPrimarySite(site);

            return RedirectToAction("Index", new { projectId = site.ProjectId, siteName = "" });
        }

        //
        // GET: /Home/BackupAll/5

        public ActionResult BackupAll(int projectId)
        {
            var project = db.Projects.SingleOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return HttpNotFound();
            }

            var siteList = db.Sites.Where(p => p.ProjectId == projectId).ToList();
            foreach (var site in siteList)
            {
                BackupPrimarySite(site);
            }

            return RedirectToAction("Index", new { projectId = project.Id, siteName = "" });
        }

        public ActionResult DownloadConfig(int id = 0)
        {
            var site = db.Sites.SingleOrDefault(s => s.Id == id);
            if (site == null)
            {
                return HttpNotFound();
            }

            var domain = IOUtil.GetDomainName(site.PrimarySiteDirectory);
            // Login to domain
            var Impersonate = new Impersonate();
            var tokenHandle = Impersonate.StartUserSession(site.WindowsUsername, domain, site.WindowsPassword);
            var newId = new WindowsIdentity(tokenHandle);
            var impersonatedUser = newId.Impersonate();

            // Action
            var primaryDirectory = site.PrimarySiteDirectory;
            var files = Directory.GetFiles(primaryDirectory, "*.config");
            var tempFile = Path.GetTempFileName();
            Zip.ZipFiles(primaryDirectory, files, tempFile);

            // Logoff 
            impersonatedUser.Undo();
            Impersonate.EndUserSession(tokenHandle);

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

        public ActionResult DownloadSite(int id = 0)
        {
            var site = db.Sites.SingleOrDefault(s => s.Id == id);
            if (site == null)
            {
                return HttpNotFound();
            }

            var domain = IOUtil.GetDomainName(site.PrimarySiteDirectory);

            // Login to domain
            var Impersonate = new Impersonate();
            var tokenHandle = Impersonate.StartUserSession(site.WindowsUsername, domain, site.WindowsPassword);
            var newId = new WindowsIdentity(tokenHandle);
            var impersonatedUser = newId.Impersonate();

            // Action
            var primaryDirectory = site.PrimarySiteDirectory;
            var tempFile = Path.GetTempFileName();
            Zip.ZipFiles(primaryDirectory, tempFile);

            // Logoff 
            impersonatedUser.Undo();
            Impersonate.EndUserSession(tokenHandle);

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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}