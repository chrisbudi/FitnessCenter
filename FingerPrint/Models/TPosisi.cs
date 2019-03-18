using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TPosisi
    {
        public TPosisi()
        {
            TUserBackOffice = new HashSet<TUserBackOffice>();
        }

        public int PosisiId { get; set; }
        public string PnamaPosisi { get; set; }

        public virtual ICollection<TUserBackOffice> TUserBackOffice { get; set; }
    }
}