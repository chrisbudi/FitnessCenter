using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrEventScore
    {
        public int TrEvScoreId { get; set; }
        public int? TrEventStepId { get; set; }
        public int? PersonEventId { get; set; }
        public int? EvScoreId { get; set; }
        public int? Score { get; set; }

        public virtual TEventScore EvScore { get; set; }
        public virtual TrPersonEvent PersonEvent { get; set; }
        public virtual TrEventStep TrEventStep { get; set; }
    }
}