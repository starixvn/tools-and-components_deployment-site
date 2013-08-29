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
    public partial class ReleaseLogController : BaseController
    {
        //
        // GET: /Backup/

        public ActionResult Index(int projectId = 0, int siteId = 0, int pageIndex = 1)
        {
            var site = db.Sites.SingleOrDefault(b => b.Id == siteId);
            var project = db.Projects.SingleOrDefault(p => p.Id == projectId);

            pageIndex=pageIndex>0?pageIndex:1;
            var startIndex = (pageIndex - 1) * 50;

            ViewBag.Site = site;
            ViewBag.Project = project;
            ViewBag.Url = Request.Url.AbsoluteUri;
            ViewBag.PageIndex = pageIndex;

            if (site != null)
            {
                var releaseLogList = db.Releases.Where(p => p.SiteId == siteId)
                                .OrderByDescending(p => p.ReleaseDate)
                                .Skip(startIndex)
                                .Take(50)
                                .ToList();
                return View(releaseLogList);
            }
            else if (project != null)
            {
                var siteIdList = db.Sites.Where(s => s.ProjectId == projectId).Select(s => s.Id).ToList();

                var releaseLogList = db.Releases.Where(p => siteIdList.Contains(p.SiteId))
                              .OrderByDescending(p => p.ReleaseDate)
                              .Skip(startIndex)
                                .Take(50)
                              .ToList();
                return View(releaseLogList);
            }
            else
            {
                var releaseLogList = db.Releases
                               .OrderByDescending(p => p.ReleaseDate)
                               .Skip(startIndex)
                                .Take(50)
                               .ToList();
                return View(releaseLogList);
            }
        }

        //
        // GET: /Backup/Details/5

        public ActionResult Details(int id = 0)
        {
            var releaseLog = db.Releases.SingleOrDefault(b => b.Id == id);
            if (releaseLog == null)
            {
                return HttpNotFound();
            }

            var site = db.Sites.SingleOrDefault(p => p.Id == releaseLog.SiteId);

            if (site == null)
            {
                return HttpNotFound();
            }

            ViewBag.Site = site;
            return View(releaseLog);
        }
    }
}