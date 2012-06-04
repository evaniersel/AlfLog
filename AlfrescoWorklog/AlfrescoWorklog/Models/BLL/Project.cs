using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.ComponentModel.DataAnnotations;

namespace AlfrescoWorklog.Models
{
    public partial class Project
    {
        #region Private Properties
        private List<Project> _ConflictingProjects = new List<Project>();
        #endregion

        #region Public Properties
        public IEnumerable<Project> ConflictingProjects { 
            get
            {
                return _ConflictingProjects;
            
            }
            private set { }
        }

        public bool IsConflicted
        {
            get
            {
                return ConflictingProjects.Count()>0;
            }
            private set{}
        }

        //Concatenates all the user names so that multiple users can be displayed easily with the associated with the project
        public string WindowsUsers
        {
            get
            {
                string usernames = string.Empty;
                foreach (aspnet_User u in aspnet_Users)
                {
                    usernames += u.UserName+" ";
                }

                return usernames;
            }

            private set { }
        }

        //Concatenates all the user emails into one statement
        public string UserEmailAddresses
        {
            get
            {
                string emailAddresses = string.Empty;
                foreach(aspnet_User u in aspnet_Users)
                {
                    emailAddresses += u.aspnet_Membership.Email;

                    if (u != aspnet_Users.Last())
                    {
                        emailAddresses += "; ";
                    }
                }

                return emailAddresses;
            }
        }
        #endregion

        #region Methods
        internal void TestValidity(IEnumerable<Project> projects)
        {
            foreach (Project p in projects)
            {
                if (this.ID != p.ID && this.userID == p.userID)
                {
                    if (!(this.endTime <= p.startTime  || 
                        this.startTime >= p.endTime))
                    {
                        _ConflictingProjects.Add(p);
                    }

                    //If this project in undeployed then any projects started in this user should be instantly conflicting.
                    if (this.startTime == this.endTime && (p.startTime>= this.startTime || p.startTime==p.endTime))
                    {
                        _ConflictingProjects.Add(p);
                    }
                }
            }
        }

        internal bool FinishProject(aspnet_User u)
        {
            bool finished = false;

            if (aspnet_Users.Contains(u))
            {
                this.isDeployed = true;
                this.endTime = DateTime.Now;
                finished = true;
            }
            return finished;
        }
        #endregion
    }
}