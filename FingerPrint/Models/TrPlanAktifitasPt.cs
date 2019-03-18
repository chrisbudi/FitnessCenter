using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TrPlanAktifitasPt
    {
        public TrPlanAktifitasPt()
        {
            InversePalnAktifitasPtparent = new HashSet<TrPlanAktifitasPt>();
        }

        public int PlanAktifitasPtid { get; set; }
        public int PersonalTrainerId { get; set; }
        public int? PalnAktifitasPtparentId { get; set; }
        public int DayOfWeek { get; set; }
        public string Period { get; set; }
        public TimeSpan WaktuMulai { get; set; }
        public TimeSpan WaktuSelesai { get; set; }
        public int LocationId { get; set; }
        public int MemberId { get; set; }
        public int PersonBoidpt { get; set; }
        public int PersonBoid { get; set; }
        public string Note { get; set; }

        public virtual TLocFitnessCenter Location { get; set; }
        public virtual TMember Member { get; set; }
        public virtual TrPlanAktifitasPt PalnAktifitasPtparent { get; set; }
        public virtual TUserBackOffice PersonBo { get; set; }
        public virtual TUserBackOffice PersonBoidptNavigation { get; set; }
        public virtual TrPersonalTrainer PersonalTrainer { get; set; }
        public virtual ICollection<TrPlanAktifitasPt> InversePalnAktifitasPtparent { get; set; }
    }
}