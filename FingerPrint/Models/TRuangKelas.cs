using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TRuangKelas
    {
        public TRuangKelas()
        {
            TKelas = new HashSet<TKelas>();
        }

        public int RuangKelasId { get; set; }
        public string NruangKelas { get; set; }
        public string BackGround { get; set; }
        public string Map { get; set; }
        public int? Kapasitas { get; set; }

        public virtual ICollection<TKelas> TKelas { get; set; }
    }
}