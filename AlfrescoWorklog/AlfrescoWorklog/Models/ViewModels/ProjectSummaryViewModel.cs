using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlfrescoWorklog.Models.ViewModels
{
    public class ProjectSummaryViewModel
    {
        public List<Project> RunningProjects{ get; set; }
        public List<Project> CompletedProjects { get; set; }
        public string ErrorMessage { get; set; }
    }
}