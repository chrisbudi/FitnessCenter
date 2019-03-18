using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TPaketPt
    {
        public TPaketPt()
        {
            TMemberType = new HashSet<TMemberType>();
            TrPersonalTrainer = new HashSet<TrPersonalTrainer>();
        }

        public int TPaketPtid { get; set; }
        public string Pptnama { get; set; }
        public int Pptjam { get; set; }
        public int PptpersonTotal { get; set; }
        public decimal? Pptharga { get; set; }
        public int? Pptmasa { get; set; }
        public bool? Pptstatus { get; set; }
        public string Pptdesc { get; set; }
        public bool Pptmembership { get; set; }

        public virtual ICollection<TMemberType> TMemberType { get; set; }
        public virtual ICollection<TrPersonalTrainer> TrPersonalTrainer { get; set; }
    }
}