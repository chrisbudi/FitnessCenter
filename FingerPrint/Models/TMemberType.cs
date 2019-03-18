using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TMemberType
    {
        public TMemberType()
        {
            StrPaymentMember = new HashSet<StrPaymentMember>();
            TMember = new HashSet<TMember>();
        }

        public int MemberTypeId { get; set; }
        public string MemberType { get; set; }
        public decimal Biaya { get; set; }
        public int JmlBulan { get; set; }
        public bool IsPaket { get; set; }
        public int Share { get; set; }
        public int? ShareMin { get; set; }
        public string Periode { get; set; }
        public int? LocationId { get; set; }
        public string PrFix { get; set; }
        public decimal? Admin { get; set; }
        public decimal? Prorate { get; set; }
        public bool? Status { get; set; }
        public int? TPaketPersonalTrainerId { get; set; }

        public virtual TLocFitnessCenter Location { get; set; }
        public virtual TPaketPt TPaketPersonalTrainer { get; set; }
        public virtual ICollection<StrPaymentMember> StrPaymentMember { get; set; }
        public virtual ICollection<TMember> TMember { get; set; }
    }
}