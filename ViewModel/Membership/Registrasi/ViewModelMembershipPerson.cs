using System;
using DataObjects.Entities;

namespace ViewModel.Membership.Registrasi
{
    public class ViewModelMembershipPerson : tPerson
    {
        public int trMembershipID { get; set; }
        public string AgreementId { get; set; }
        public int Seq { get; set; }

        public DateTime TglMulai { get; set; }
    }
}
