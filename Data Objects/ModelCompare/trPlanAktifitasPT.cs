namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("trPlanAktifitasPT")]
    public partial class trPlanAktifitasPT
    {
        [Key]
        public int PlanAktifitasPTID { get; set; }

        public DateTime Waktu { get; set; }

        [Required]
        [StringLength(15)]
        public string LocationID { get; set; }

        [Required]
        [StringLength(50)]
        public string MemberID { get; set; }

        [Required]
        [StringLength(50)]
        public string BOID { get; set; }

        public int ActionPTID { get; set; }

        public string Note { get; set; }

        public virtual tActionPT tActionPT { get; set; }

        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }

        public virtual tMember tMember { get; set; }

        public virtual tUserBackOffice tUserBackOffice { get; set; }
    }
}
