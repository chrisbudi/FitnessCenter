using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TKelas
    {
        public TKelas()
        {
            TrPlanKelas = new HashSet<TrPlanKelas>();
        }

        public int KelasId { get; set; }
        public int? RuangKelasId { get; set; }
        public string KnamaKelas { get; set; }
        public byte[] ImageKelas { get; set; }

        public virtual TRuangKelas RuangKelas { get; set; }
        public virtual ICollection<TrPlanKelas> TrPlanKelas { get; set; }
    }
}