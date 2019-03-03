namespace DataObjects.ModelCompare
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("trMembership")]
    public partial class trMembership
    {
        public trMembership()
        {
            strAktivitasSales = new HashSet<strAktivitasSale>();
            strPaymentMembers = new HashSet<strPaymentMember>();
            trMembership1 = new HashSet<trMembership>();
            trTTDs = new HashSet<trTTD>();
        }

        [Key]
        public int trMembersipID { get; set; }

        public int? ParentID { get; set; }

        [Required]
        [StringLength(15)]
        public string LocationID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] MSTime { get; set; }

        [StringLength(50)]
        public string MemberID { get; set; }

        [Column(TypeName = "date")]
        public DateTime MSTglMulai { get; set; }

        [Column(TypeName = "date")]
        public DateTime MSTglSelesai { get; set; }

        [Required]
        [StringLength(50)]
        public string BOID { get; set; }

        public int StatusMID { get; set; }

        public decimal? Subtotal { get; set; }

        public decimal? Disc { get; set; }

        public decimal? Total { get; set; }

        public int? GenBayar { get; set; }

        public string Note { get; set; }

        public int PersonID { get; set; }

        [StringLength(50)]
        public string AgreementID { get; set; }

        public int CountMember { get; set; }

        public int seq { get; set; }

        public decimal? Admin { get; set; }

        public decimal? Prorate { get; set; }

        public virtual ICollection<strAktivitasSale> strAktivitasSales { get; set; }

        public virtual ICollection<strPaymentMember> strPaymentMembers { get; set; }

        public virtual tLocFitnessCenter tLocFitnessCenter { get; set; }

        public virtual tMember tMember { get; set; }

        public virtual tPerson tPerson { get; set; }

        public virtual ICollection<trMembership> trMembership1 { get; set; }

        public virtual trMembership trMembership2 { get; set; }

        public virtual tStatusMember tStatusMember { get; set; }

        public virtual tUserBackOffice tUserBackOffice { get; set; }

        public virtual ICollection<trTTD> trTTDs { get; set; }
    }
}
