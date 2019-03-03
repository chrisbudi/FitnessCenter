namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tActionPT")]
    public partial class tActionPT
    {
        public tActionPT()
        {
            trPlanAktifitasPTs = new HashSet<trPlanAktifitasPT>();
        }

        [Key]
        public int ActionPTID { get; set; }

        [Required]
        [StringLength(50)]
        public string ActionPTName { get; set; }

        public string ActionPTKet { get; set; }

        public virtual ICollection<trPlanAktifitasPT> trPlanAktifitasPTs { get; set; }
    }
}
