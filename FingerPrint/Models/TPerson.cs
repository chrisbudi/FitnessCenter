using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TPerson
    {
        public TPerson()
        {
            TMember = new HashSet<TMember>();
            TrPersonEvent = new HashSet<TrPersonEvent>();
            TrTtd = new HashSet<TrTtd>();
        }

        public int PersonId { get; set; }
        public string Pnama { get; set; }
        public string PtempLahir { get; set; }
        public DateTime? PtglLahir { get; set; }
        public string Pgender { get; set; }
        public string Palamat { get; set; }
        public string Prt { get; set; }
        public string Prw { get; set; }
        public string Pkelurahan { get; set; }
        public string Pkecamatan { get; set; }
        public string Pkota { get; set; }
        public string Ppropinsi { get; set; }
        public string Pidentitas { get; set; }
        public string Pemail { get; set; }
        public string Ptelp { get; set; }
        public string Php1 { get; set; }
        public string Php2 { get; set; }
        public string PpinBb { get; set; }
        public string Id { get; set; }

        public virtual AspNetUsers IdNavigation { get; set; }
        public virtual TUserBackOffice TUserBackOffice { get; set; }
        public virtual ICollection<TMember> TMember { get; set; }
        public virtual ICollection<TrPersonEvent> TrPersonEvent { get; set; }
        public virtual ICollection<TrTtd> TrTtd { get; set; }
    }
}