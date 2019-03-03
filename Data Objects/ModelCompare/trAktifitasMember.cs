namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("trAktifitasMember")]
    public partial class trAktifitasMember
    {
        public trAktifitasMember()
        {
            strKlaimPTs = new HashSet<strKlaimPT>();
            trAktifitasKelas = new HashSet<trAktifitasKela>();
        }

        [Key]
        public int AktifitasMemberID { get; set; }

        public DateTime? AMMulai { get; set; }

        public DateTime? AMSelesai { get; set; }

        [Required]
        [StringLength(50)]
        public string verifikasiMember { get; set; }

        [Required]
        [StringLength(50)]
        public string verifikasiToken { get; set; }

        [Required]
        [StringLength(50)]
        public string MemberID { get; set; }

        [Required]
        [StringLength(50)]
        public string BOID { get; set; }

        [Required]
        [StringLength(15)]
        public string LocationID { get; set; }

        [Required]
        [StringLength(3)]
        public string Status { get; set; }

        public virtual ICollection<strKlaimPT> strKlaimPTs { get; set; }

        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }

        public virtual tMember tMember { get; set; }

        public virtual ICollection<trAktifitasKela> trAktifitasKelas { get; set; }

        public virtual tUserBackOffice tUserBackOffice { get; set; }
    }
}
