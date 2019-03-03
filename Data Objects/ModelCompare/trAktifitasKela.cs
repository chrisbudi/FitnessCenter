namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class trAktifitasKela
    {
        [Key]
        public int AktifitasKelasID { get; set; }

        public int PlanKelasID { get; set; }

        public int? AktifitasMemberID { get; set; }

        public virtual trAktifitasMember trAktifitasMember { get; set; }

        public virtual trPlanKela trPlanKela { get; set; }
    }
}
