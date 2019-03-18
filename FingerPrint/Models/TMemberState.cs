using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TMemberState
    {
        public TMemberState()
        {
            StrAktivitasSales = new HashSet<StrAktivitasSales>();
        }

        public int MemberStateId { get; set; }
        public string MemberStateName { get; set; }
        public string Note { get; set; }

        public virtual ICollection<StrAktivitasSales> StrAktivitasSales { get; set; }
    }
}