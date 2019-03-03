using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataObjects.Entities;
using ViewModel.Membership.Registrasi;

//using ViewModel.Registrasi.CalonMember;

namespace ViewModel.Trainer.RegisterPT
{

    public partial class ViewModelPTCreate
    {

        public trPersonalTrainer PersonalTrainer { get; set; }
        public trMembership Membership { get; set; }

        public IEnumerable<trPersonalTrainer> PersonalTrainers { get; set; }
        public IEnumerable<trPaymentWith> PaymentsWith { get; set; }

        public ViewModelMasterPayment MasterPayment { get; set; }
        public decimal TotalSumPayment { get; set; }

        [Range(0, 500, ErrorMessage = "Total payment must < 500")]
        public decimal TotalPaymentMustPay { get; set; }

        public decimal TotalPaidAmount { get; set; }

        [Range(0, 500, ErrorMessage = "Total payment must < 500")]
        public decimal totalOverPaymentDetail { get; set; }

        public int MemberCount { get; set; }

        public string DiscType { get; set; }

        public decimal DiscVal { get; set; }

        [Required(ErrorMessage = "Discount Nominal Cannot null")]
        public int DiscNominal { get; set; }
    }
}