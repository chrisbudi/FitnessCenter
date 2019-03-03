using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataObjects.Entities;
using ViewModel.Membership.CalonMember;

namespace ViewModel.Membership.Registrasi
{

    public class ViewModelMembershipCreate
    {

        public trMembership Membership { get; set; }

        public IEnumerable<trMembership> Memberships { get; set; }

        public strPaymentMember PaymentMember { get; set; }

        public tPerson FPerson { get; set; }

        public IEnumerable<trPaymentWith> PaymentsWith { get; set; }

        public ViewModelMasterPayment MasterPayment { get; set; }

        public int MemberCount { get; set; }

        public int? PaketPTId { get; set; }

        [Required(ErrorMessage = "Discount Nominal Cannot null")]
        public int DiscNominal { get; set; }

        public string DiscType { get; set; }

        public decimal DiscVal { get; set; }

        public decimal TotalSumPayment { get; set; }

        [Range(0, 500, ErrorMessage = "Total payment must < 500")]
        public decimal TotalPaymentMustPay { get; set; }

        public decimal TotalPaidAmount { get; set; }

        [Range(0, 500, ErrorMessage = "Total payment must < 500")]
        public decimal totalOverPaymentDetail { get; set; }
    }
}