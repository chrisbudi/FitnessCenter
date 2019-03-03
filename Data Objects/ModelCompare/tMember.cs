namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tMember")]
    public partial class tMember
    {
        public tMember()
        {
            strLocMembers = new HashSet<strLocMember>();
            strLocMembers1 = new HashSet<strLocMember>();
            trAktifitasMembers = new HashSet<trAktifitasMember>();
            trMemberships = new HashSet<trMembership>();
            trPersonalTrainers = new HashSet<trPersonalTrainer>();
            trPlanAktifitasPTs = new HashSet<trPlanAktifitasPT>();
        }

        [Key]
        [StringLength(50)]
        public string MemberID { get; set; }

        [Column(TypeName = "date")]
        public DateTime MMulai { get; set; }

        [Column(TypeName = "image")]
        public byte[] MFoto { get; set; }

        [Column(TypeName = "image")]
        public byte[] MFotoKTP { get; set; }

        [Column(TypeName = "image")]
        public byte[] MFotoCCDebit { get; set; }

        [Column(TypeName = "image")]
        public byte[] MFotoSignature { get; set; }

        public string MRFID { get; set; }

        public int PersonID { get; set; }

        public int? MemberTypeID { get; set; }

        public int? TotalMonth { get; set; }

        public virtual ICollection<strLocMember> strLocMembers { get; set; }

        public virtual ICollection<strLocMember> strLocMembers1 { get; set; }

        public virtual tMemberType tMemberType { get; set; }

        public virtual tPerson tPerson { get; set; }

        public virtual ICollection<trAktifitasMember> trAktifitasMembers { get; set; }

        public virtual ICollection<trMembership> trMemberships { get; set; }

        public virtual ICollection<trPersonalTrainer> trPersonalTrainers { get; set; }

        public virtual ICollection<trPlanAktifitasPT> trPlanAktifitasPTs { get; set; }
    }
}
