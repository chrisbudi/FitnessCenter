using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class StrPayment
    {
        public int StrPaymentId { get; set; }
        public int PaymentWithId { get; set; }
        public int TrPaymentId { get; set; }

        public virtual TrPaymentWith PaymentWith { get; set; }
        public virtual StrPaymentMember TrPayment { get; set; }
    }
}