using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrPersonEvent
    {
        public TrPersonEvent()
        {
            TrEventScore = new HashSet<TrEventScore>();
            TrEventStep = new HashSet<TrEventStep>();
        }

        public int PersonEventId { get; set; }
        public int PersonId { get; set; }
        public int EvId { get; set; }
        public int EvRoleId { get; set; }

        public virtual TEvent Ev { get; set; }
        public virtual TRoleEvent EvRole { get; set; }
        public virtual TPerson Person { get; set; }
        public virtual StrPersonEvent StrPersonEvent { get; set; }
        public virtual ICollection<TrEventScore> TrEventScore { get; set; }
        public virtual ICollection<TrEventStep> TrEventStep { get; set; }
    }
}