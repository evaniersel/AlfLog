using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlfrescoWorklog.Models
{
    public partial class Sandpit
    {
        public bool IsAvailable
        {
            get
            {
                if (Projects.Count > 0)
                {
                    try
                    {
                        var lastProject = Projects.Where(p => !p.isDeployed).Last();
                        return false;
                    }
                    catch (Exception)
                    {
                        return true;
                    }
                }
                return true;
            }
            private set { }
        }

        public Project LatestProject
        {
            get
            {
                if (Projects.Count > 0)
                    return Projects.Where(p => !p.isDeployed).Last();

                return null;
            }
            private set { }
        }
    }
}