namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tPosisi")]
    public partial class tPosisi
    {
        public tPosisi()
        {
            tUserBackOffices = new HashSet<tUserBackOffice>();
        }

        [Key]
        public int PosisiID { get; set; }

        [Required]
        [StringLength(50)]
        public string PNamaPosisi { get; set; }

        public virtual ICollection<tUserBackOffice> tUserBackOffices { get; set; }
    }
}
