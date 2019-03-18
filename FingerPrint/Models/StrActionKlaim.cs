using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class StrActionKlaim
    {
        public StrActionKlaim()
        {
            StrActionKlaimParam = new HashSet<StrActionKlaimParam>();
        }

        public int StrActionClaimId { get; set; }
        public int ClaimId { get; set; }
        public int ActionPtid { get; set; }

        public virtual TActionPt ActionPt { get; set; }
        public virtual ICollection<StrActionKlaimParam> StrActionKlaimParam { get; set; }
    }
}