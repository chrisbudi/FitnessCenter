using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TEvent
    {
        public TEvent()
        {
            TEventStep = new HashSet<TEventStep>();
            TrPersonEvent = new HashSet<TrPersonEvent>();
        }

        public int EvId { get; set; }
        public DateTime EvStartDate { get; set; }
        public DateTime EvEndDate { get; set; }
        public string EvName { get; set; }
        public int JumlahPesertaAwal { get; set; }
        public int JumlahStep { get; set; }

        public virtual ICollection<TEventStep> TEventStep { get; set; }
        public virtual ICollection<TrPersonEvent> TrPersonEvent { get; set; }
    }
}