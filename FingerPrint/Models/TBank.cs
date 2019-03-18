using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TBank
    {
        public TBank()
        {
            TrPaymentWithBank = new HashSet<TrPaymentWith>();
            TrPaymentWithEdc = new HashSet<TrPaymentWith>();
        }

        public int BankId { get; set; }
        public string NamaBank { get; set; }
        public bool Edc { get; set; }

        public virtual ICollection<TrPaymentWith> TrPaymentWithBank { get; set; }
        public virtual ICollection<TrPaymentWith> TrPaymentWithEdc { get; set; }
    }
}