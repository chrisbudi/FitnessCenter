using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrPaymentWith
    {
        public TrPaymentWith()
        {
            StrPayment = new HashSet<StrPayment>();
        }

        public int PaymentWithId { get; set; }
        public int PaymentTypeId { get; set; }
        public int? BankId { get; set; }
        public int? Edcid { get; set; }
        public string ApprCode { get; set; }
        public string TraceCode { get; set; }
        public string Pemegang { get; set; }
        public string NoKartu { get; set; }
        public DateTime? ValidUntil { get; set; }
        public decimal PayAmount { get; set; }
        public bool I { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? Mbramount { get; set; }

        public virtual TBank Bank { get; set; }
        public virtual TBank Edc { get; set; }
        public virtual TPaymentType PaymentType { get; set; }
        public virtual ICollection<StrPayment> StrPayment { get; set; }
    }
}