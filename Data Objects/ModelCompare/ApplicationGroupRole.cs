namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ApplicationGroupRole
    {
        [Key]
        [Column(Order = 0)]
        public string ApplicationRoleId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string ApplicationGroupId { get; set; }

        public virtual ApplicationGroup ApplicationGroup { get; set; }
    }
}
