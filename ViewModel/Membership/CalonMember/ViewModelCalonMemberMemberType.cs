
using System.ComponentModel.DataAnnotations;

namespace ViewModel.Membership.CalonMember
{
    public class ViewModelCalonMemberMemberType
    {
        public string SalesName { get; set; }

        public string MemberStatus { get; set; }

        [Required]
        public int MemberType { get; set; }

        [Required]
        public string MemberTypeDef { get; set; }

        public int CountMember { get; set; }

        public int TotalMonth { get; set; }

        public string Prefix { get; set; }

        public decimal? Payment { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? Admin { get; set; }

        public decimal? Prorate { get; set; }

        public decimal? Discount { get; set; }

        public decimal? DiscountPct { get; set; }

        public string tglLahirString { get; set; }
    }
}
