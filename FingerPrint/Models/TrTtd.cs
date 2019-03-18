using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrTtd
    {
        public int TrTtdid { get; set; }
        public int? TrMembershipId { get; set; }
        public int? TrPtid { get; set; }
        public byte[] TrTtd1 { get; set; }
        public int MemberId { get; set; }

        public virtual TMember Member { get; set; }
        public virtual TPerson MemberNavigation { get; set; }
        public virtual TrMembership TrMembership { get; set; }
        public virtual TrPersonalTrainer TrPt { get; set; }
    }
}