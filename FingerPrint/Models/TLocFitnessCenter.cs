using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TLocFitnessCenter
    {
        public TLocFitnessCenter()
        {
            StrLocBo = new HashSet<StrLocBo>();
            StrLocMember = new HashSet<StrLocMember>();
            TMemberType = new HashSet<TMemberType>();
            TrAktifitasMember = new HashSet<TrAktifitasMember>();
            TrMembership = new HashSet<TrMembership>();
            TrPlanAktifitasPt = new HashSet<TrPlanAktifitasPt>();
            TrPlanKelas = new HashSet<TrPlanKelas>();
        }

        public int LocationId { get; set; }
        public string Lalamat { get; set; }
        public string Ltlp { get; set; }
        public string Lfax { get; set; }
        public string Lauth { get; set; }

        public virtual ICollection<StrLocBo> StrLocBo { get; set; }
        public virtual ICollection<StrLocMember> StrLocMember { get; set; }
        public virtual ICollection<TMemberType> TMemberType { get; set; }
        public virtual ICollection<TrAktifitasMember> TrAktifitasMember { get; set; }
        public virtual ICollection<TrMembership> TrMembership { get; set; }
        public virtual ICollection<TrPlanAktifitasPt> TrPlanAktifitasPt { get; set; }
        public virtual ICollection<TrPlanKelas> TrPlanKelas { get; set; }
    }
}