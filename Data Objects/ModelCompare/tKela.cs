namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tKela
    {
        public tKela()
        {
            trPlanKelas = new HashSet<trPlanKela>();
        }

        [Key]
        [StringLength(50)]
        public string KelasID { get; set; }

        [Required]
        [StringLength(50)]
        public string KNamaKelas { get; set; }

        public int KKapasitas { get; set; }

        public virtual ICollection<trPlanKela> trPlanKelas { get; set; }
    }
}
