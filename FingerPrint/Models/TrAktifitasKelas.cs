using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrAktifitasKelas
    {
        public int AktifitasKelasId { get; set; }
        public int PlanKelasId { get; set; }
        public int? AktifitasMemberId { get; set; }

        public virtual TrAktifitasMember AktifitasMember { get; set; }
        public virtual TrPlanKelas PlanKelas { get; set; }
    }
}