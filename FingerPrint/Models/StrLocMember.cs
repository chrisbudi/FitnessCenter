using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class StrLocMember
    {
        public int LocMemberId { get; set; }
        public int MemberId { get; set; }
        public int LocationId { get; set; }

        public virtual TLocFitnessCenter Location { get; set; }
        public virtual TMember Member { get; set; }
    }
}