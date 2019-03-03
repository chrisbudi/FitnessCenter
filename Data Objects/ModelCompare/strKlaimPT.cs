namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("strKlaimPT")]
    public partial class strKlaimPT
    {
        [Key]
        public int ClaimPTID { get; set; }

        public int trPTID { get; set; }

        public DateTime AwalClaim { get; set; }

        public DateTime AkhirClaim { get; set; }

        public bool Void { get; set; }

        [Required]
        [StringLength(50)]
        public string verifikasiMember { get; set; }

        [Required]
        [StringLength(50)]
        public string verifikasiPT { get; set; }

        public int AktifitasMemberID { get; set; }

        public virtual trAktifitasMember trAktifitasMember { get; set; }

        public virtual trPersonalTrainer trPersonalTrainer { get; set; }
    }
}
