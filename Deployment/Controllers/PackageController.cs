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

namespace Deployment.Controllers
{
    public partial class PackageController : BaseController
    {
        //
        // GET: /Package/

        public ActionResult Index(int siteId = 0)
        {

            var packageList = db.Packages.Where(p => p.SiteId == siteId)
                                         .OrderByDescending(p => p.CreatedDate)
                                         .ToList();

            var site = db.Sites.SingleOrDefault(p => p.Id == siteId);

            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.Site = site;
            return View(packageList);
        }

        //
        // GET: /Package/Details/5

        public ActionResult Details(int id = 0)
        {
            var package = db.Packages.SingleOrDefault(p => p.Id == id);
            if (package == null)
            {
                return HttpNotFound();
            }

            var site = db.Sites.SingleOrDefault(p => p.Id == package.SiteId);

            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.Site = site;
            return View(package);
        }

        //
        // GET: /Package/Create

        public ActionResult Create(int siteId = 0)
        {
            var package = new Package { SiteId = siteId };

            var site = db.Sites.SingleOrDefault(p => p.Id == package.SiteId);

            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.Site = site;
            return View(package);
        }

        //
        // POST: /Package/Create

        [HttpPost]
        public ActionResult Create(Package package, HttpPostedFileBase uploadFile)
        {
            // Validation           
            if (uploadFile.ContentLength <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please input the package zip file.");
            }
            else
            {
                var fileName = uploadFile == null || string.IsNullOrEmpty(uploadFile.FileName)
                  ? string.Empty
                  : Path.GetFileName(uploadFile.FileName);
                var fileExtension = Path.GetExtension(fileName);

                if (string.Compare(fileExtension, ".zip", StringComparison.CurrentCultureIgnoreCase) != 0)
                {
                    ModelState.AddModelError(string.Empty, "Please input the package zip file.");
                }
            }

            // Execute
            if (ModelState.IsValid)
            {
                // Get site info
                var site = db.Sites.SingleOrDefault(s => s.Id == package.SiteId);
                if (site == null)
                {
                    return HttpNotFound();
                }

                // Save package file

                SavePackageFile(package, site, uploadFile);

                // Save package Info
                package.Creator = User.Identity.Name;
                package.CreatedDate = DateTime.Now;
                package.ReleaseUser = string.Empty;
                package.ReleaseDate = new DateTime(2000, 1, 1);
                db.Packages.AddObject(package);

                db.SaveChanges();
                return RedirectToAction("Index", new { siteId = package.SiteId });
            }

            return View(package);
        }

        //
        // GET: /Package/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var package = db.Packages.SingleOrDefault(p => p.Id == id);
            if (package == null)
            {
                return HttpNotFound();
            }

            return View(package);
        }

        //
        // POST: /Package/Edit/5

        [HttpPost]
        public ActionResult Edit(Package package)
        {
            var savedPackage = db.Packages.SingleOrDefault(p => p.Id == package.Id);
            if (package == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                savedPackage.Creator = User.Identity.Name;
                savedPackage.CreatedDate = DateTime.Now;
                savedPackage.Description = package.Description;

                db.SaveChanges();

                return RedirectToAction("Index", new { siteId = savedPackage.SiteId });
            }

            return View(package);
        }


        // GET: /Package/Release/5

        public ActionResult Release(int id = 0)
        {
            var package = db.Packages.SingleOrDefault(p => p.Id == id);
            if (package == null)
            {
                return HttpNotFound();
            }

            var site = db.Sites.SingleOrDefault(p => p.Id == package.SiteId);

            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.Site = site;
            return View(package);
        }

        //
        // POST: /Package/Delete/5

        [HttpPost, ActionName("Release")]
        public ActionResult ReleaseConfirmed(int id, int siteId = 0, bool isBackup = false)
        {
            var package = db.Packages.SingleOrDefault(p => p.Id == id);
            siteId = siteId == 0 ? package.SiteId : siteId;
            var site = db.Sites.SingleOrDefault(s => s.Id == siteId);
            if (site == null)
            {
                return HttpNotFound();
            }

            // Release
            ReleasePackage(package, site, isBackup);

            // Save release information date
            package.ReleaseUser = User.Identity.Name;
            package.ReleaseDate = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Details", "Site", new { id = site.Id });
        }

        // GET: /Package/Release/5

        public ActionResult ReleaseAll(int id = 0, string siteName = "")
        {
            var package = db.Packages.SingleOrDefault(p => p.Id == id);
            if (package == null)
            {
                return HttpNotFound();
            }

            var site = db.Sites.SingleOrDefault(s => s.Id == package.SiteId);
            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.SiteName = siteName;

            ViewBag.SiteList = string.IsNullOrEmpty(siteName)
                ? db.Sites.Where(s => s.ProjectId == site.ProjectId).ToList()
                : db.Sites.Where(s => s.ProjectId == site.ProjectId && s.Name.IndexOf(siteName) >= 0).ToList();

            return View(package);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}