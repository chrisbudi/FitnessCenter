namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("strPayment")]
    public partial class strPayment
    {
        public int StrPaymentID { get; set; }

        public int PaymentWithID { get; set; }

        public int trPaymentID { get; set; }

        public virtual strPaymentMember strPaymentMember { get; set; }

        public virtual trPaymentWith trPaymentWith { get; set; }
    }
}
