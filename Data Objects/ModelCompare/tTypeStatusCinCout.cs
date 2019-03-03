namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tTypeStatusCinCout")]
    public partial class tTypeStatusCinCout
    {
        public tTypeStatusCinCout()
        {
            trCinCouts = new HashSet<trCinCout>();
        }

        [Key]
        public int TypeStatusInOut { get; set; }

        [Required]
        [StringLength(50)]
        public string NameStatusInOut { get; set; }

        public virtual ICollection<trCinCout> trCinCouts { get; set; }
    }
}
