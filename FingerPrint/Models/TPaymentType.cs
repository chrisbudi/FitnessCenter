using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TPaymentType
    {
        public TPaymentType()
        {
            TrPaymentWith = new HashSet<TrPaymentWith>();
        }

        public int PaymentTypeId { get; set; }
        public string NamaType { get; set; }

        public virtual ICollection<TrPaymentWith> TrPaymentWith { get; set; }
    }
}