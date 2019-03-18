using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TRoleEvent
    {
        public TRoleEvent()
        {
            TrPersonEvent = new HashSet<TrPersonEvent>();
        }

        public int EvRoleId { get; set; }
        public string EvRoleName { get; set; }

        public virtual ICollection<TrPersonEvent> TrPersonEvent { get; set; }
    }
}