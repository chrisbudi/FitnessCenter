using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrPersonalTrainer
    {
        public TrPersonalTrainer()
        {
            InverseParent = new HashSet<TrPersonalTrainer>();
            StrKlaimPt = new HashSet<StrKlaimPt>();
            TrPlanAktifitasPt = new HashSet<TrPlanAktifitasPt>();
            TrTtd = new HashSet<TrTtd>();
        }

        public int TrPersonalTrainerId { get; set; }
        public int TPaketPtid { get; set; }
        public int? SisaJam { get; set; }
        public int? PersonBoidpt { get; set; }
        public int Seq { get; set; }
        public int? ParentId { get; set; }
        public int? Masa { get; set; }
        public int? TrMembershipId { get; set; }

        public virtual TrPersonalTrainer Parent { get; set; }
        public virtual TUserBackOffice PersonBoidptNavigation { get; set; }
        public virtual TPaketPt TPaketPt { get; set; }
        public virtual TrMembership TrMembership { get; set; }
        public virtual ICollection<TrPersonalTrainer> InverseParent { get; set; }
        public virtual ICollection<StrKlaimPt> StrKlaimPt { get; set; }
        public virtual ICollection<TrPlanAktifitasPt> TrPlanAktifitasPt { get; set; }
        public virtual ICollection<TrTtd> TrTtd { get; set; }
    }
}