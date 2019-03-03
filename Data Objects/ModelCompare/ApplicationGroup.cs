namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ApplicationGroup
    {
        public ApplicationGroup()
        {
            ApplicationGroupRoles = new HashSet<ApplicationGroupRole>();
            AspFormAuthorizations = new HashSet<AspFormAuthorization>();
            ApplicationUserGroups = new HashSet<ApplicationUserGroup>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<ApplicationGroupRole> ApplicationGroupRoles { get; set; }

        public virtual ICollection<AspFormAuthorization> AspFormAuthorizations { get; set; }

        public virtual ICollection<ApplicationUserGroup> ApplicationUserGroups { get; set; }
    }
}
