namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tUserBackOffice")]
    public partial class tUserBackOffice
    {
        public tUserBackOffice()
        {
            strLocBOes = new HashSet<strLocBO>();
            trAktifitasMembers = new HashSet<trAktifitasMember>();
            trCinCouts = new HashSet<trCinCout>();
            trMemberships = new HashSet<trMembership>();
            trPersonalTrainers = new HashSet<trPersonalTrainer>();
            trPlanAktifitasPTs = new HashSet<trPlanAktifitasPT>();
            trPlanKelas = new HashSet<trPlanKela>();
        }

        [Key]
        [StringLength(50)]
        public string BOID { get; set; }

        public bool StatusBOID { get; set; }

        [Column(TypeName = "date")]
        public DateTime BMulai { get; set; }

        [Column(TypeName = "image")]
        public byte[] BFoto { get; set; }

        [Column(TypeName = "image")]
        public byte[] BFotoKTP { get; set; }

        [Column(TypeName = "image")]
        public byte[] BFotoSignature { get; set; }

        public string BRFID { get; set; }

        public int PosisiID { get; set; }

        public int PersonID { get; set; }

        public virtual ICollection<strLocBO> strLocBOes { get; set; }

        public virtual tPerson tPerson { get; set; }

        public virtual tPosisi tPosisi { get; set; }

        public virtual ICollection<trAktifitasMember> trAktifitasMembers { get; set; }

        public virtual ICollection<trCinCout> trCinCouts { get; set; }

        public virtual ICollection<trMembership> trMemberships { get; set; }

        public virtual ICollection<trPersonalTrainer> trPersonalTrainers { get; set; }

        public virtual ICollection<trPlanAktifitasPT> trPlanAktifitasPTs { get; set; }

        public virtual ICollection<trPlanKela> trPlanKelas { get; set; }
    }
}
