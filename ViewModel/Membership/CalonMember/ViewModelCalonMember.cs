using DataObjects.Entities;

namespace ViewModel.Membership.CalonMember
{
    public class ViewModelCalonMember
    {
        public tPerson Person { get; set; }
        public trMembership Membership { get; set; }
        public ViewModelCalonMemberMemberType MemberType { get; set; }
    }
}
