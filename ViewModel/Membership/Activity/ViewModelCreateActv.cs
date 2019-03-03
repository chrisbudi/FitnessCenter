using System.ComponentModel.DataAnnotations;
using DataObjects.Entities;


namespace ViewModel.Membership.Activity
{
    public class ViewModelCreateActv
    {
        public strAktivitasSale Act { get; set; }
        [Display(Name = "Member Status")]
        public string MemberStatus { get; set; }
        [Display(Name = "Member Type")]
        public int MemberType { get; set; }
        public int CountMember { get; set; }
        public decimal Payment { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Admin { get; set; }
        public decimal Prorate { get; set; }
        public decimal Discount { get; set; }
    }
}
