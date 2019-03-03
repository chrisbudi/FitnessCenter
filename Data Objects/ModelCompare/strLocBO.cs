namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("strLocBO")]
    public partial class strLocBO
    {
        public strLocBO()
        {
            trCinCouts = new HashSet<trCinCout>();
        }

        [Key]
        public int LocBoID { get; set; }

        [Required]
        [StringLength(15)]
        public string LocationID { get; set; }

        [Required]
        [StringLength(50)]
        public string BOID { get; set; }

        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }

        public virtual tUserBackOffice tUserBackOffice { get; set; }

        public virtual ICollection<trCinCout> trCinCouts { get; set; }
    }
}
