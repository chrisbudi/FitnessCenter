using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class ApplicationGroups
    {
        public ApplicationGroups()
        {
            ApplicationGroupRoles = new HashSet<ApplicationGroupRoles>();
            ApplicationUserGroups = new HashSet<ApplicationUserGroups>();
            AspFormAuthorization = new HashSet<AspFormAuthorization>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<ApplicationGroupRoles> ApplicationGroupRoles { get; set; }
        public virtual ICollection<ApplicationUserGroups> ApplicationUserGroups { get; set; }
        public virtual ICollection<AspFormAuthorization> AspFormAuthorization { get; set; }
    }
}