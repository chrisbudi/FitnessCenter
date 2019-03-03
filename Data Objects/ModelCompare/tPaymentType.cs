namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tPaymentType")]
    public partial class tPaymentType
    {
        public tPaymentType()
        {
            trPaymentWiths = new HashSet<trPaymentWith>();
        }

        [Key]
        public int PaymentTypeID { get; set; }

        [StringLength(50)]
        public string NamaType { get; set; }

        public virtual ICollection<trPaymentWith> trPaymentWiths { get; set; }
    }
}
