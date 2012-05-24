using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlfrescoWorklog.Models;
using System.Data;
using System.Web.Security;

namespace AlfrescoWorklog.Controllers
{
    public class WorklogController : Controller
    {
        //
        // GET: /Worklog/
        AlfrescoWorklogDBEntities db = new AlfrescoWorklogDBEntities();


        [Authorize]
        public ActionResult CreateProject()
        {
            ProjectViewModel pvm = new ProjectViewModel();
            pvm.Sandpits = new SelectList(db.Sandpits, "ID", "name");
            pvm.Users = new SelectList(db.AlfrescoUsers, "ID", "name");
            return View(pvm);
        }


        //Returns a view with a list of validated projects.
        [Authorize]
        public ActionResult ProjectSummary()
        {
            var projects = db.Projects.ToList();

            foreach (Project p in projects)
            {
                p.TestValidity(db.Projects);
            }

            projects.Reverse();
            return View(projects);
        }

        [Authorize]
        public ActionResult SandpitSummary()
        {
            var sandpits = db.Sandpits.ToList();
            SandpitViewModel svm = new SandpitViewModel();
            svm.AvailableSandpits = sandpits.Where(s => s.IsAvailable).ToList();
            svm.SandpitsInUse = sandpits.Where(s => !s.IsAvailable).ToList();

            return View(svm);
        }

        //Creates a new project to specification
        [HttpPost]
        public ActionResult CreateProject(ProjectViewModel pvm)
        {
            if (ModelState.IsValid)
            {
                
                Project p = new Project();
                p.name = pvm.name;
                p.description = pvm.description;
                p.sandpitID = pvm.SandpitID;
                p.userID = pvm.UserID;
                p.startTime = DateTime.Now;
                p.endTime = p.startTime;
                p.aspnet_Users.Add(db.aspnet_User.Where(u => u.LoweredUserName.Equals(User.Identity.Name.ToLower())).Single());
                p.isDeployed = false;
                db.Projects.AddObject(p);
                db.SaveChanges();
                return RedirectToAction("ProjectSummary");
            }
            return View("CreateProject");
        }

        //Finishes and deploys a running project        
        [Authorize]
        public ActionResult FinishProject(int id)
        {
            var project = db.Projects.Where(p => p.ID == id).Single();
            project.isDeployed = true;
            project.endTime = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("ProjectSummary");        
        }

        //Returns the conflict results
        [Authorize]
        public ActionResult ShowConflicts(int id)
        {
            var project = db.Projects.Where(p => p.ID == id).Single();
            project.TestValidity(db.Projects);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ShowConflicts", project);
            }
            return View(project);
        }

        //TO DO: Implement the logic to check the logged in user's projects and return the list of all these projects.
        [Authorize]
        public ActionResult UserProjects()
        {
            var user = db.aspnet_User.Where(u => u.LoweredUserName.Equals(User.Identity.Name.ToLower())).Single();
            return View(user.Projects);
        }

        
        [Authorize]
        public ActionResult AddUserToProject(int projectID)
        {
            var project = db.Projects.Where(p => p.ID == projectID).Single();
            var user = db.aspnet_User.Where(u => u.LoweredUserName.Equals(User.Identity.Name.ToLower())).Single();

            if(!project.aspnet_Users.Contains(user))
            {
                project.aspnet_Users.Add(user);
                db.SaveChanges();
            }

            return View("ProjectSummary", db.Projects);
        }
    }
}
