using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrAktifitasMember
    {
        public TrAktifitasMember()
        {
            StrKlaimPt = new HashSet<StrKlaimPt>();
            TrAktifitasKelas = new HashSet<TrAktifitasKelas>();
        }

        public int AktifitasMemberId { get; set; }
        public DateTime? Ammulai { get; set; }
        public DateTime? Amselesai { get; set; }
        public string VerifikasiMember { get; set; }
        public string VerifikasiToken { get; set; }
        public int MemberId { get; set; }
        public int PersonBoid { get; set; }
        public int LocationId { get; set; }
        public int? ActionPtid { get; set; }
        public string Status { get; set; }

        public virtual TActionPt ActionPt { get; set; }
        public virtual TLocFitnessCenter Location { get; set; }
        public virtual TMember Member { get; set; }
        public virtual TUserBackOffice PersonBo { get; set; }
        public virtual ICollection<StrKlaimPt> StrKlaimPt { get; set; }
        public virtual ICollection<TrAktifitasKelas> TrAktifitasKelas { get; set; }
    }
}