namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tBank")]
    public partial class tBank
    {
        public tBank()
        {
            trPaymentWiths = new HashSet<trPaymentWith>();
        }

        [Key]
        public int BankID { get; set; }

        [StringLength(50)]
        public string NamaBank { get; set; }

        public virtual ICollection<trPaymentWith> trPaymentWiths { get; set; }
    }
}
