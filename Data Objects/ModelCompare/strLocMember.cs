namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("strLocMember")]
    public partial class strLocMember
    {
        [Key]
        public int LocMemberId { get; set; }

        [Required]
        [StringLength(15)]
        public string LocationID { get; set; }

        [Required]
        [StringLength(50)]
        public string MemberID { get; set; }

        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }

        public virtual tMember tMember { get; set; }

        public virtual tMember tMember1 { get; set; }
    }
}
