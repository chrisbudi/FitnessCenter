namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class trPlanKela
    {
        public trPlanKela()
        {
            trAktifitasKelas = new HashSet<trAktifitasKela>();
        }

        [Key]
        public int PlanKelasID { get; set; }

        [Required]
        [StringLength(50)]
        public string KelasID { get; set; }

        [Required]
        [StringLength(50)]
        public string InstrukurID { get; set; }

        public short Hari { get; set; }

        public short JamKe { get; set; }

        [Required]
        [StringLength(15)]
        public string LocationID { get; set; }

        public virtual tKela tKela { get; set; }

        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }

        public virtual ICollection<trAktifitasKela> trAktifitasKelas { get; set; }

        public virtual tUserBackOffice tUserBackOffice { get; set; }
    }
}
