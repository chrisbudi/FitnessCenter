using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TMember
    {
        public TMember()
        {
            StrLocMember = new HashSet<StrLocMember>();
            TrAktifitasMember = new HashSet<TrAktifitasMember>();
            TrMembership = new HashSet<TrMembership>();
            TrPembayaran = new HashSet<TrPembayaran>();
            TrPlanAktifitasPt = new HashSet<TrPlanAktifitasPt>();
            TrTtd = new HashSet<TrTtd>();
        }

        public int MemberId { get; set; }
        public string MemberNo { get; set; }
        public int PersonId { get; set; }
        public string Mrfid { get; set; }
        public string MfotoSignature { get; set; }
        public string Mfoto { get; set; }
        public string MfotoKtp { get; set; }
        public string MfotoCcdebit { get; set; }
        public int? MemberTypeId { get; set; }
        public string MfotoUrl { get; set; }

        public virtual TMemberType MemberType { get; set; }
        public virtual TPerson Person { get; set; }
        public virtual ICollection<StrLocMember> StrLocMember { get; set; }
        public virtual ICollection<TrAktifitasMember> TrAktifitasMember { get; set; }
        public virtual ICollection<TrMembership> TrMembership { get; set; }
        public virtual ICollection<TrPembayaran> TrPembayaran { get; set; }
        public virtual ICollection<TrPlanAktifitasPt> TrPlanAktifitasPt { get; set; }
        public virtual ICollection<TrTtd> TrTtd { get; set; }
    }
}