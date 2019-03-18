using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class StrPersonEvent
    {
        public int PersonEventId { get; set; }
        public string Gym { get; set; }
        public int NoPeserta { get; set; }

        public virtual TrPersonEvent PersonEvent { get; set; }
    }
}