using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TEventStep
    {
        public TEventStep()
        {
            TEventScore = new HashSet<TEventScore>();
            TrEventStep = new HashSet<TrEventStep>();
        }

        public int EvStepId { get; set; }
        public int EvId { get; set; }
        public int Step { get; set; }
        public int PersonCount { get; set; }
        public string StepDetail { get; set; }

        public virtual TEvent Ev { get; set; }
        public virtual ICollection<TEventScore> TEventScore { get; set; }
        public virtual ICollection<TrEventStep> TrEventStep { get; set; }
    }
}