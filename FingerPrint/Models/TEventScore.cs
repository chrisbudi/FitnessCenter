using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TEventScore
    {
        public TEventScore()
        {
            TrEventScore = new HashSet<TrEventScore>();
        }

        public int EvScoreId { get; set; }
        public string EvScoreName { get; set; }
        public int? EvStepId { get; set; }
        public int? MinScore { get; set; }
        public int? MaxScore { get; set; }

        public virtual TEventStep EvStep { get; set; }
        public virtual ICollection<TrEventScore> TrEventScore { get; set; }
    }
}