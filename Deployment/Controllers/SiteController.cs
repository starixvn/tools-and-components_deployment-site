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

        // GET: /Home/Clean/5

        public ActionResult Clean(int id = 0)
        {
            var site = db.Sites.SingleOrDefault(s => s.Id == id);
            if (site == null)
            {
                return HttpNotFound();
            }

            return View(site);
        }

        //
        // POST: /Home/Clean/5

        [HttpPost, ActionName("Clean")]
        public ActionResult CleanConfirmed(int id, string fileList)
        {
            Site site = db.Sites.SingleOrDefault(s => s.Id == id);

            var fileListArray = fileList.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim()).ToArray();

            CleanSiteFiles(site, fileListArray);

            return RedirectToAction("Details", new { id = site.Id });
        }


        public ActionResult CleanWhole(int id)
        {
            Site site = db.Sites.SingleOrDefault(s => s.Id == id);

            CleanWholeSite(site);

            return RedirectToAction("Details", new { id = site.Id });
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

            return DownloadSiteConfigZip(site);
        }

        public ActionResult DownloadSite(int id = 0)
        {
            var site = db.Sites.SingleOrDefault(s => s.Id == id);
            if (site == null)
            {
                return HttpNotFound();
            }

            return DownloadSiteZip(site);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}