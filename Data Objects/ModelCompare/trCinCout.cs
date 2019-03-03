namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("trCinCout")]
    public partial class trCinCout
    {
        [Key]
        public int CinCoutID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RefCinCout { get; set; }

        [Required]
        [StringLength(50)]
        public string BOID { get; set; }

        public DateTime TimeStatus { get; set; }

        public int TypeStatusInOut { get; set; }

        public int LocBoID { get; set; }

        public virtual strLocBO strLocBO { get; set; }

        public virtual tTypeStatusCinCout tTypeStatusCinCout { get; set; }

        public virtual tUserBackOffice tUserBackOffice { get; set; }
    }
}
