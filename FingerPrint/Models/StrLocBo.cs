using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class StrLocBo
    {
        public StrLocBo()
        {
            TrCinCout = new HashSet<TrCinCout>();
        }

        public int LocBoId { get; set; }
        public int PersonBoid { get; set; }
        public int LocationId { get; set; }

        public virtual TLocFitnessCenter Location { get; set; }
        public virtual TUserBackOffice PersonBo { get; set; }
        public virtual ICollection<TrCinCout> TrCinCout { get; set; }
    }
}