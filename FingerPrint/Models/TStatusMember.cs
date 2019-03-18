using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TStatusMember
    {
        public TStatusMember()
        {
            TStatusMemberPrice = new HashSet<TStatusMemberPrice>();
            TrMembership = new HashSet<TrMembership>();
        }

        public int StatusMid { get; set; }
        public string Stket { get; set; }

        public virtual ICollection<TStatusMemberPrice> TStatusMemberPrice { get; set; }
        public virtual ICollection<TrMembership> TrMembership { get; set; }
    }
}