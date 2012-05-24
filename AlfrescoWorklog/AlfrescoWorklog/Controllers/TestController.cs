using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlfrescoWorklog.Models;

namespace AlfrescoWorklog.Controllers
{ 
    public class TestController : Controller
    {
        private AlfrescoWorklogDBEntities db = new AlfrescoWorklogDBEntities();

        //
        // GET: /Test/

        public ViewResult Index()
        {
            var projects = db.Projects.Include("Sandpit").Include("User");
            ViewBag.sandpitID = new SelectList(db.Sandpits, "ID", "name");
            ViewBag.userID = new SelectList(db.Users, "ID", "name");
            return View(projects.ToList());
        }

        //
        // GET: /Test/Details/5

        public ViewResult Details(int id)
        {
            Project project = db.Projects.Single(p => p.ID == id);
            return View(project);
        }

        //
        // GET: /Test/Create

        public ActionResult Create()
        {
            
            return View();
        } 

        //
        // POST: /Test/Create

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.startTime = DateTime.Now;
                project.endTime = project.startTime;
                project.windowsLogon = User.Identity.Name.Substring(User.Identity.Name.IndexOf(@"\")+1);
                project.isDeployed = false;
                db.Projects.AddObject(project);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.sandpitID = new SelectList(db.Sandpits, "ID", "name", project.sandpitID);
            ViewBag.userID = new SelectList(db.Users, "ID", "name", project.userID);
            return View(project);
        }
        
        //
        // GET: /Test/Edit/5
 
        public ActionResult Edit(int id)
        {
            Project project = db.Projects.Single(p => p.ID == id);
            ViewBag.sandpitID = new SelectList(db.Sandpits, "ID", "name", project.sandpitID);
            ViewBag.userID = new SelectList(db.Users, "ID", "name", project.userID);
            return View(project);
        }

        //
        // POST: /Test/Edit/5

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
            ViewBag.sandpitID = new SelectList(db.Sandpits, "ID", "name", project.sandpitID);
            ViewBag.userID = new SelectList(db.Users, "ID", "name", project.userID);
            return View(project);
        }

        //
        // GET: /Test/Delete/5
 
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.Single(p => p.ID == id);
            return View(project);
        }

        //
        // POST: /Test/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Project project = db.Projects.Single(p => p.ID == id);
            db.Projects.DeleteObject(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}