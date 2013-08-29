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
    public class ProjectController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Project/

        public ActionResult Index()
        {
            var projectList = db.Projects
                               .OrderBy(p => p.Name)
                               .ToList();

            return View(projectList);
        }

        //
        // GET: /Project/Details/5

        public ActionResult Details(int id = 0)
        {
            Project project = db.Projects.SingleOrDefault(p => p.Id == id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Project/Create

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.AddObject(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        //
        // GET: /Project/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Project project = db.Projects.SingleOrDefault(p => p.Id == id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Attach(project);
                db.ObjectStateManager.ChangeObjectState(project, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

    
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}