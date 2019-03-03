using System;
using System.ComponentModel.DataAnnotations;
using DataObjects.Entities;

namespace ViewModel.Activity
{
    public class MemberActivity
    {
        public MemberActivity()
        {
            Activity = new trAktifitasMember();
            Membership = new trMembership();
            Member = new tMember();
        }

        public trAktifitasMember Activity { get; set; }
        public trMembership Membership { get; set; }
        public tMember Member { get; set; }
        public trPersonalTrainer PersonalTrainer { get; set; }
        public tMemberType MemberType { get; set; }
        public string StatusMembership { get; set; }

        [Required(ErrorMessage = "Data identitas di perlukan")]
        public string PersonIdentitas { get; set; }
    }
}
