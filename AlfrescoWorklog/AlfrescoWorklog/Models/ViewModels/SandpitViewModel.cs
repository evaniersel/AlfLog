using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlfrescoWorklog.Models
{
    public class SandpitViewModel
    {
        public List<Sandpit> AvailableSandpits { get; set; }
        public List<Sandpit> SandpitsInUse { get; set; }
    }
}