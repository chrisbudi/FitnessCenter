using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrEventStep
    {
        public TrEventStep()
        {
            TrEventScore = new HashSet<TrEventScore>();
        }

        public int TrEventStepId { get; set; }
        public int PersonEventId { get; set; }
        public int? EvStepId { get; set; }
        public int? TotalScore { get; set; }

        public virtual TEventStep EvStep { get; set; }
        public virtual TrPersonEvent PersonEvent { get; set; }
        public virtual ICollection<TrEventScore> TrEventScore { get; set; }
    }
}