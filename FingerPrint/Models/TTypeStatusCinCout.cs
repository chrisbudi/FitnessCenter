using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TTypeStatusCinCout
    {
        public TTypeStatusCinCout()
        {
            TrCinCout = new HashSet<TrCinCout>();
        }

        public int TypeStatusInOut { get; set; }
        public string NameStatusInOut { get; set; }

        public virtual ICollection<TrCinCout> TrCinCout { get; set; }
    }
}