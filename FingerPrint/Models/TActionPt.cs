using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TActionPt
    {
        public TActionPt()
        {
            StrActionKlaim = new HashSet<StrActionKlaim>();
            TrAktifitasMember = new HashSet<TrAktifitasMember>();
        }

        public int ActionPtid { get; set; }
        public string ActionPtname { get; set; }
        public string ActionPtket { get; set; }

        public virtual ICollection<StrActionKlaim> StrActionKlaim { get; set; }
        public virtual ICollection<TrAktifitasMember> TrAktifitasMember { get; set; }
    }
}