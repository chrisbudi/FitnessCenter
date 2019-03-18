using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrMembership
    {
        public TrMembership()
        {
            InverseParent = new HashSet<TrMembership>();
            StrAktivitasSales = new HashSet<StrAktivitasSales>();
            StrPaymentMemberMembershipDtl = new HashSet<StrPaymentMember>();
            StrPaymentMemberTrMembership = new HashSet<StrPaymentMember>();
            TrPersonalTrainer = new HashSet<TrPersonalTrainer>();
            TrTtd = new HashSet<TrTtd>();
        }

        public int TrMembershipId { get; set; }
        public int? ParentId { get; set; }
        public int LocationId { get; set; }
        public byte[] Mstime { get; set; }
        public DateTime MstglMulai { get; set; }
        public DateTime MstglSelesai { get; set; }
        public int PersonBoidadm { get; set; }
        public int PersonBoidsales { get; set; }
        public int StatusMid { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Disc { get; set; }
        public decimal? Total { get; set; }
        public int? GenBayar { get; set; }
        public string Note { get; set; }
        public int MemberId { get; set; }
        public string AgreementId { get; set; }
        public int CountMember { get; set; }
        public int Seq { get; set; }
        public decimal? Admin { get; set; }
        public decimal? Prorate { get; set; }
        public int CardStatus { get; set; }
        public int TotalMonth { get; set; }
        public string AccountingStatus { get; set; }
        public string ActivationCode { get; set; }
        public DateTime? Msinput { get; set; }
        public string DiscType { get; set; }
        public decimal? DiscVal { get; set; }

        public virtual TCardStatus CardStatusNavigation { get; set; }
        public virtual TLocFitnessCenter Location { get; set; }
        public virtual TMember Member { get; set; }
        public virtual TrMembership Parent { get; set; }
        public virtual TUserBackOffice PersonBoidadmNavigation { get; set; }
        public virtual TUserBackOffice PersonBoidsalesNavigation { get; set; }
        public virtual TStatusMember StatusM { get; set; }
        public virtual ICollection<TrMembership> InverseParent { get; set; }
        public virtual ICollection<StrAktivitasSales> StrAktivitasSales { get; set; }
        public virtual ICollection<StrPaymentMember> StrPaymentMemberMembershipDtl { get; set; }
        public virtual ICollection<StrPaymentMember> StrPaymentMemberTrMembership { get; set; }
        public virtual ICollection<TrPersonalTrainer> TrPersonalTrainer { get; set; }
        public virtual ICollection<TrTtd> TrTtd { get; set; }
    }
}