using System;
using System.Collections.Generic;

namespace UareUSampleCSharp.Models
{
    public partial class TUserBackOffice
    {
        public TUserBackOffice()
        {
            StrLocBo = new HashSet<StrLocBo>();
            TrAktifitasMember = new HashSet<TrAktifitasMember>();
            TrCinCout = new HashSet<TrCinCout>();
            TrMembershipPersonBoidadmNavigation = new HashSet<TrMembership>();
            TrMembershipPersonBoidsalesNavigation = new HashSet<TrMembership>();
            TrPembayaran = new HashSet<TrPembayaran>();
            TrPersonalTrainer = new HashSet<TrPersonalTrainer>();
            TrPlanAktifitasPtPersonBo = new HashSet<TrPlanAktifitasPt>();
            TrPlanAktifitasPtPersonBoidptNavigation = new HashSet<TrPlanAktifitasPt>();
            TrPlanKelasPersonBoidadmNavigation = new HashSet<TrPlanKelas>();
            TrPlanKelasPersonBoidinstrukturNavigation = new HashSet<TrPlanKelas>();
        }

        public int PersonBoid { get; set; }
        public string Boidno { get; set; }
        public bool StatusBoid { get; set; }
        public DateTime Bmulai { get; set; }
        public byte[] Bfoto { get; set; }
        public byte[] BfotoKtp { get; set; }
        public byte[] BfotoSignature { get; set; }
        public string Brfid { get; set; }
        public int PosisiId { get; set; }
        public string Background { get; set; }

        public virtual TPerson PersonBo { get; set; }
        public virtual TPosisi Posisi { get; set; }
        public virtual ICollection<StrLocBo> StrLocBo { get; set; }
        public virtual ICollection<TrAktifitasMember> TrAktifitasMember { get; set; }
        public virtual ICollection<TrCinCout> TrCinCout { get; set; }
        public virtual ICollection<TrMembership> TrMembershipPersonBoidadmNavigation { get; set; }
        public virtual ICollection<TrMembership> TrMembershipPersonBoidsalesNavigation { get; set; }
        public virtual ICollection<TrPembayaran> TrPembayaran { get; set; }
        public virtual ICollection<TrPersonalTrainer> TrPersonalTrainer { get; set; }
        public virtual ICollection<TrPlanAktifitasPt> TrPlanAktifitasPtPersonBo { get; set; }
        public virtual ICollection<TrPlanAktifitasPt> TrPlanAktifitasPtPersonBoidptNavigation { get; set; }
        public virtual ICollection<TrPlanKelas> TrPlanKelasPersonBoidadmNavigation { get; set; }
        public virtual ICollection<TrPlanKelas> TrPlanKelasPersonBoidinstrukturNavigation { get; set; }
    }
}