namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tSalesAction")]
    public partial class tSalesAction
    {
        public tSalesAction()
        {
            strAktivitasSales = new HashSet<strAktivitasSale>();
        }

        [Key]
        public int SalesActionID { get; set; }

        [Required]
        [StringLength(50)]
        public string ActionName { get; set; }

        public string Note { get; set; }

        public virtual ICollection<strAktivitasSale> strAktivitasSales { get; set; }
    }
}
