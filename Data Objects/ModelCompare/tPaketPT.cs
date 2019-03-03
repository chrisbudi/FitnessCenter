namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tPaketPT")]
    public partial class tPaketPT
    {
        public tPaketPT()
        {
            trPersonalTrainers = new HashSet<trPersonalTrainer>();
        }

        [StringLength(10)]
        public string tPaketPTID { get; set; }

        [Required]
        [StringLength(50)]
        public string PPTNama { get; set; }

        public int PPTJam { get; set; }

        public decimal? PPTHarga { get; set; }

        public int? PPTMasa { get; set; }

        public virtual ICollection<trPersonalTrainer> trPersonalTrainers { get; set; }
    }
}
