using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AlfrescoWorklog.Models
{
    public class ProjectViewModel
    {
        //Project properties
        [Required]
        public string name { get; set; }
        [Required, StringLength(200)]
        public string description { get; set; }
        public int UserID { get; set; }
        public int SandpitID { get; set; }

        //Select List of Sandpits
        public SelectList Sandpits { get; set; }
        //Select List of users
        public SelectList Users { get; set; }

    }
}