using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataObjects.Entities;
using ViewModel.Membership.CalonMember;
using ViewModel.Membership.Registrasi;

namespace ViewModel.Membership.FreezeCancel
{
    public class MemberManagementModel
    {
        public MemberManagementModel()
        {
            AvaliableMenu = new List<string>();
        }

        public trMembership Membership { get; set; }

        public ViewModelCalonMemberMemberType MemberType { get; set; }

        public IEnumerable<trPaymentWith> PaymentsWith { get; set; }

        public ViewModelMasterPayment MasterPayment { get; set; }

        public tPerson Person { get; set; }

        public string CurrentMemberType { get; set; }

        public decimal TotalSumPayment { get; set; }

        public decimal TotalNewMemberMustPay { get; set; }

        [Range(0, 0, ErrorMessage = "Total payment must = 0")]
        public decimal TotalPaymentMustPay { get; set; }

        [Range(0, 0, ErrorMessage = "Over payment must = 0")]
        public decimal TotalOverPaymentDetail { get; set; }

        public int TotalMonthFreeze { get; set; }

        public decimal? FreezePrice { get; set; }

        public decimal? TotalFreezePrice { get; set; }

        public decimal? TransferPrice { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        [Required]
        public string Note { get; set; }

        public string DateTimeNow { get; set; }

        public string StatusAction { get; set; }

        public List<string> AvaliableMenu { get; set; }
    }
}
