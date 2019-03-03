namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tStatusMember")]
    public partial class tStatusMember
    {
        public tStatusMember()
        {
            trMemberships = new HashSet<trMembership>();
        }

        [Key]
        public int StatusMID { get; set; }

        [Required]
        [StringLength(50)]
        public string STKet { get; set; }

        public virtual ICollection<trMembership> trMemberships { get; set; }
    }
}
