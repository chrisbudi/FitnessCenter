using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class StrAktivitasSales
    {
        public int AktivitasSalesId { get; set; }
        public int TrMembershipId { get; set; }
        public int SalesActionId { get; set; }
        public int MemberStateId { get; set; }
        public string Note { get; set; }
        public DateTime? Date { get; set; }
        public byte[] Aktivitasstap { get; set; }

        public virtual TMemberState MemberState { get; set; }
        public virtual TSalesAction SalesAction { get; set; }
        public virtual TrMembership TrMembership { get; set; }
    }
}