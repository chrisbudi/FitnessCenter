using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class StrPaymentMember
    {
        public StrPaymentMember()
        {
            StrPayment = new HashSet<StrPayment>();
        }

        public int TrPaymentId { get; set; }
        public int? TrMembershipId { get; set; }
        public int Pembayaranke { get; set; }
        public bool StatusBayar { get; set; }
        public DateTime Tanggal { get; set; }
        public byte[] TimeStamp { get; set; }
        public int? MemberTypeId { get; set; }
        public string Note { get; set; }
        public int? MembershipDtlid { get; set; }

        public virtual TMemberType MemberType { get; set; }
        public virtual TrMembership MembershipDtl { get; set; }
        public virtual TrMembership TrMembership { get; set; }
        public virtual ICollection<StrPayment> StrPayment { get; set; }
    }
}