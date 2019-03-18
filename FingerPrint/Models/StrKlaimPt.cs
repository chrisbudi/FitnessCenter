using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class StrKlaimPt
    {
        public int ClaimPtid { get; set; }
        public int TrPersonalTrainerId { get; set; }
        public DateTime AwalClaim { get; set; }
        public DateTime AkhirClaim { get; set; }
        public bool Void { get; set; }
        public string VerifikasiMember { get; set; }
        public string VerifikasiPt { get; set; }
        public int AktifitasMemberId { get; set; }
        public int Kepuasan { get; set; }
        public string Note { get; set; }
        public int Jam { get; set; }

        public virtual TrAktifitasMember AktifitasMember { get; set; }
        public virtual TrPersonalTrainer TrPersonalTrainer { get; set; }
    }
}