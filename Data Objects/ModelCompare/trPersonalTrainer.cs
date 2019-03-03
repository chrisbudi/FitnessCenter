namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("trPersonalTrainer")]
    public partial class trPersonalTrainer
    {
        public trPersonalTrainer()
        {
            strKlaimPTs = new HashSet<strKlaimPT>();
            trTTDs = new HashSet<trTTD>();
        }

        [Key]
        public int trPTID { get; set; }

        [Required]
        [StringLength(15)]
        public string LocationID { get; set; }

        [Required]
        [StringLength(50)]
        public string MemberID { get; set; }

        [Required]
        [StringLength(10)]
        public string tPaketPTID { get; set; }

        public decimal PTSubtotal { get; set; }

        public decimal PTDiskon { get; set; }

        public decimal PTTotal { get; set; }

        public int? SisaJam { get; set; }

        [Required]
        [StringLength(50)]
        public string BOID { get; set; }

        public virtual ICollection<strKlaimPT> strKlaimPTs { get; set; }

        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }

        public virtual tMember tMember { get; set; }

        public virtual tPaketPT tPaketPT { get; set; }

        public virtual tUserBackOffice tUserBackOffice { get; set; }

        public virtual ICollection<trTTD> trTTDs { get; set; }
    }
}
