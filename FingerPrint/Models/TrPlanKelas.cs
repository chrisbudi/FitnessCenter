using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrPlanKelas
    {
        public TrPlanKelas()
        {
            TrAktifitasKelas = new HashSet<TrAktifitasKelas>();
        }

        public int PlanKelasId { get; set; }
        public int KelasId { get; set; }
        public int PersonBoidinstruktur { get; set; }
        public int PersonBoidadm { get; set; }
        public int DayOfWeek { get; set; }
        public string Period { get; set; }
        public TimeSpan WaktuMulai { get; set; }
        public TimeSpan WaktuSelesai { get; set; }
        public int LocationId { get; set; }

        public virtual TKelas Kelas { get; set; }
        public virtual TLocFitnessCenter Location { get; set; }
        public virtual TUserBackOffice PersonBoidadmNavigation { get; set; }
        public virtual TUserBackOffice PersonBoidinstrukturNavigation { get; set; }
        public virtual ICollection<TrAktifitasKelas> TrAktifitasKelas { get; set; }
    }
}