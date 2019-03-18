using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TCardStatus
    {
        public TCardStatus()
        {
            TrMembership = new HashSet<TrMembership>();
        }

        public int CardStatusId { get; set; }
        public string CardStatus { get; set; }
        public string CardDesc { get; set; }
        public bool FinalStatus { get; set; }

        public virtual ICollection<TrMembership> TrMembership { get; set; }
    }
}