using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace Deployment.Controllers
{
    public partial class BackupController : BaseController
    {
        const int PageSize = 10;
        //
        // GET: /Backup/

        public ActionResult Index(int siteId = 0, int page = 1)
        {
            var site = db.Sites.SingleOrDefault(b => b.Id == siteId);
            if (site == null)
            {
                return HttpNotFound();
            }

            page = page < 0 ? 1 : page;
            var backupList = db.Backups.Where(p => p.SiteId == siteId)
                                .OrderByDescending(p => p.CreatedDate)
                                .ToList();

            var onePageOfBackups = backupList.ToPagedList(page, PageSize);

            ViewBag.Site = site;
            return View(onePageOfBackups);
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

            return RedirectToAction("Details", "Site", new { id = site.Id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}