using System;

namespace FitnessCenter.Areas.Office.Models
{
    public class EditorMemberCard
    {
        public int PersonId { get; set; }
        public int Seq { get; set; }
        public int MemberId { get; set; }
        public string MemberNo { get; set; }
        public string MemberName { get; set; }
        public string Alamat { get; set; }
        public string Kota { get; set; }
        public DateTime TglLahir { get; set; }
        public string Gender { get; set; }
        public string MemberType { get; set; }
        public int MembershipID { get; set; }

    }
}
