using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlfrescoWorklog.Models;
using System.Data;
using System.Web.Security;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using AlfrescoWorklog.Models.ViewModels;

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
            var completed_projects = db.Projects.Where(p => p.isDeployed == true).ToList();
            var running_projects = db.Projects.Where(p => p.isDeployed == false).ToList();

            foreach (Project p in completed_projects)
            {
                p.TestValidity(db.Projects);
            }

            foreach (Project p in running_projects)
            {
                p.TestValidity(db.Projects);
            }

            running_projects.Reverse();
            completed_projects.Reverse();

            ProjectSummaryViewModel psv = new ProjectSummaryViewModel();
            psv.CompletedProjects = completed_projects;
            psv.RunningProjects = running_projects;
            psv.ErrorMessage = TempData["ErrorMessage"] as string;
            return View(psv);
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
            var user = db.aspnet_User.Where(u => u.LoweredUserName.Equals(User.Identity.Name.ToLower())).Single();
            if (project.FinishProject(user))
            {
                db.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "You are not working on this project so you can't finish it!";
            }
           
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

            if (!project.aspnet_Users.Contains(user))
            {
                project.aspnet_Users.Add(user);
                db.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "You are already on this project!";
            }

            return RedirectToAction("ProjectSummary");
        }

        //Action responsible for clearing all the projects from the datastore.
        public ActionResult DeleteProjects()
        {
            foreach (Project p in db.Projects)
            {
                db.Projects.DeleteObject(p);
            }
            db.SaveChanges();

            return RedirectToAction("ProjectSummary");
        }

        //ANYTHING UNDER HERE IS IN PROGRESS>>>> NOT IMPLEMENTED PROPERLY
        public void ProjectsToXML(List<Project> projects)
        {
            using (XmlWriter writer = XmlWriter.Create(@"C:\temp\test.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Projects");
                foreach (Project p in projects)
                {
                    writer.WriteStartElement("Project");
                    writer.WriteElementString("ProjectName", p.name.ToString());
                    writer.WriteElementString("ProjectDescription", p.description.ToString());
                    writer.WriteElementString("SandpitID", p.sandpitID.ToString());
                    writer.WriteElementString("AlfrescoUserID", p.userID.ToString());

                    writer.WriteEndElement();

                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }


        private void testing()
        {
            ProjectsToXML(db.Projects.ToList());
        }
    }
}
