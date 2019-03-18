using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TSalesAction
    {
        public TSalesAction()
        {
            StrAktivitasSales = new HashSet<StrAktivitasSales>();
        }

        public int SalesActionId { get; set; }
        public string ActionName { get; set; }
        public string Note { get; set; }

        public virtual ICollection<StrAktivitasSales> StrAktivitasSales { get; set; }
    }
}