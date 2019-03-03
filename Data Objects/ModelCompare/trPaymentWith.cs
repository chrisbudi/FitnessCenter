namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("trPaymentWith")]
    public partial class trPaymentWith
    {
        public trPaymentWith()
        {
            strPayments = new HashSet<strPayment>();
        }

        [Key]
        public int PaymentWithID { get; set; }

        public int PaymentTypeID { get; set; }

        public int? BankID { get; set; }

        [StringLength(50)]
        public string Pemegang { get; set; }

        [StringLength(50)]
        public string NoKartu { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidUntil { get; set; }

        public decimal payAmount { get; set; }

        public virtual ICollection<strPayment> strPayments { get; set; }

        public virtual tBank tBank { get; set; }

        public virtual tPaymentType tPaymentType { get; set; }
    }
}
