using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataObjects.Entities;

namespace ViewModel.Membership.Registrasi
{
    public class trMembershipViewModel
    {
        public trMembershipViewModel()
        {
            strAktivitasSales = new HashSet<strAktivitasSale>();
            strPaymentMembers = new HashSet<strPaymentMember>();
            trTTDs = new HashSet<trTTD>();
            CountMember = 1;
        }

        [Key]
        public int trMembershipID { get; set; }

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

        public decimal? Total { get; set; }

        public int? GenBayar { get; set; }

        public string Note { get; set; }

        public int PersonID { get; set; }

        [StringLength(50)]
        public string AgreementID { get; set; }

        public int seq { get; set; }

        public int CountMember { get; set; }

        public virtual ICollection<strAktivitasSale> strAktivitasSales { get; set; }

        public virtual ICollection<strPaymentMember> strPaymentMembers { get; set; }

        public virtual ICollection<trTTD> trTTDs { get; set; }

        public virtual tPerson Person { get; set; }

        public virtual tMember tMember { get; set; }

    }
}
