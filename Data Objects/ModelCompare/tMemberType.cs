namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tMemberType")]
    public partial class tMemberType
    {
        public tMemberType()
        {
            strPaymentMembers = new HashSet<strPaymentMember>();
            tMembers = new HashSet<tMember>();
        }

        [Key]
        public int MemberTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string MemberType { get; set; }

        public decimal Biaya { get; set; }

        public int JmlBulan { get; set; }

        public bool IsPaket { get; set; }

        public int Share { get; set; }

        [Required]
        [StringLength(6)]
        public string Periode { get; set; }

        [Required]
        [StringLength(15)]
        public string LocationID { get; set; }

        [Required]
        [StringLength(1)]
        public string prFix { get; set; }

        public decimal? Admin { get; set; }

        public decimal? Prorate { get; set; }

        public virtual ICollection<strPaymentMember> strPaymentMembers { get; set; }

        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }

        public virtual ICollection<tMember> tMembers { get; set; }
    }
}
