using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Deployment.Models;

namespace Deployment.Controllers
{
    public partial class BackupController : BaseController
    {
        //
        // GET: /Backup/

        public ActionResult Index(int siteId = 0)
        {
            var backupList = db.Backups.Where(p => p.SiteId == siteId)
                                .OrderByDescending(p=>p.CreatedDate)
                                .ToList();
            var site = db.Sites.SingleOrDefault(b => b.Id == siteId);
            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.Site = site;
            return View(backupList);
        }

        //
        // GET: /Backup/Details/5

        public ActionResult Details(int id = 0)
        {
            var backup = db.Backups.SingleOrDefault(b => b.Id == id);
            if (backup == null)
            {
                return HttpNotFound();
            }

            var site = db.Sites.SingleOrDefault(p => p.Id == backup.SiteId);

            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.Site = site;
            return View(backup);
        }


        // GET: /Package/Release/5

        public ActionResult Restore(int id = 0)
        {
            var backup = db.Backups.SingleOrDefault(b => b.Id == id);
            if (backup == null)
            {
                return HttpNotFound();
            }

            var site = db.Sites.SingleOrDefault(p => p.Id == backup.SiteId);

            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.Site = site;
            return View(backup);
        }

        //
        // POST: /Package/Delete/5

        [HttpPost, ActionName("Restore")]
        public ActionResult RestoreConfirmed(int id)
        {
            var backup = db.Backups.SingleOrDefault(b => b.Id == id);
            if (backup == null)
            {
                return HttpNotFound();
            }

            var site = db.Sites.SingleOrDefault(p => p.Id == backup.SiteId);

            if (site == null)
            {
                return HttpNotFound();
            }

            // Restore
            RestoreAllSite(backup, site);
            // Update last restore info
            site.LastRestore = backup.Name;
            site.LastRestoreDate = DateTime.Now;
            site.LastRestoreUser = User.Identity.Name;

            db.SaveChanges();

            return RedirectToAction("Details","Home", new { id = site.Id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}