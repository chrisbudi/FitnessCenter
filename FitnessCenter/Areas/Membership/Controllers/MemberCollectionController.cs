using System;
using System.Data.Odbc;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.DataTables;
using ViewModel.Membership.Registrasi;

namespace FitnessCenter.Areas.Membership.Controllers
{
    public class MemberCollectionController : FitController
    {
        private readonly IServicePaymentMember _paymentMemberManager;
        private readonly IServiceMembership _membershipManager;
        public MemberCollectionController(ServicePaymentMember memberManager, ServiceMembership paymentWithManager)
        {
            _paymentMemberManager = memberManager;
            _membershipManager = paymentWithManager;
        }

        // GET: Membership/MembershipCollection
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CollectMember(int id)
        {
            var paymentNo = 0;
            var paymentDate = DateTime.Now;
            var member = _paymentMemberManager.Get(id, out paymentNo, out paymentDate);

            var firstPayment = _membershipManager.FirstPayment(member.trMembershipID) / 2;


            var col = new ViewModelCollection()
            {
                PaymentMember = member,
                PaymentWith = new trPaymentWith()
                {
                    payAmount = firstPayment,
                    paidAmount = firstPayment
                },
                PaymentNo = paymentNo,
                PaymentDateFor = paymentDate
            };

            return View(col);
        }

        [HttpPost]
        public ActionResult CollectMember(ViewModelCollection member)
        {
            var dMembership = new DataServiceMembership();
            var dPaymentMember = new DataServicePaymentMember();
            var dpaymentWith = new DataServicePaymentWith();

            var trMembershipID = member.PaymentMember.trMembershipID;
            var membership = dMembership.GetobjByID(trMembershipID.Value);
            using (var ts = new TransactionScope())
            {

                var tr = new trMembership()
                {
                    Admin = 0,
                    AccountingStatus = null,
                    AgreementID = membership.AgreementID,
                    MemberID = membership.MemberID,
                    ActivationCode = membership.ActivationCode,
                    CardStatus = membership.CardStatus,
                    CountMember = membership.CountMember,
                    Disc = 0,
                    GenBayar = 0,
                    LocationID = User.ActiveLocation,
                    MSTglMulai = DateTime.Now,
                    MSTglSelesai = member.PaymentDateFor.AddMonths(1),
                    Note = "",
                    Subtotal = member.PaymentWith.payAmount,
                    Total = member.PaymentWith.payAmount,
                    Prorate = 0,
                    TotalMonth = 1,
                    PersonBOIDADM = User.Person.PersonID,
                    PersonBOIDSales = membership.tUserBackOffice_PersonBOIDSales.tPerson.PersonID,
                    StatusMID = membership.StatusMID,
                    seq = member.PaymentNo,
                };

                dMembership.Insert(tr);


                var paymentmember = dPaymentMember.GetPaymentByMembership(membership.trMembershipID).Single(m => m.pembayaranke == member.PaymentNo);
                paymentmember.statusBayar = true;
                dPaymentMember.Update(paymentmember);

                var paymentwith = member.PaymentWith;
                dpaymentWith.Insert(paymentwith);

                var pay = new strPayment
                {
                    trPaymentID = paymentmember.trPaymentID,
                    PaymentWithID = paymentwith.PaymentWithID
                };
                dPaymentMember.InsertstrPayment(pay);

                ts.Complete();

            }

            return RedirectToAction("Index");
        }


        public ActionResult MemberCollectionResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var membership = new DataServiceMembership().LoadDataCollection(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in membership.ListClass
                         select new[]
                         {
                    Convert.ToString(count++),
                    d.trMembership_trMembershipID.tMember.MemberNO,
                    d.trMembership_trMembershipID.tMember.tPerson.PNama,
                    d.trMembership_trMembershipID.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    d.trMembership_trMembershipID.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
                    $"{d.trMembership_trMembershipID.MSTglMulai.ToString("dd-MM-yyyy")} - {d.trMembership_trMembershipID.MSTglSelesai.ToString("dd-MM-yyyy")}",
                    d.tMemberType.MemberType,
                    d.pembayaranke.ToString(),
                    (d.trMembership_trMembershipID.Subtotal / 2 ?? 0).ToString("N2"),
                    d.trPaymentID.ToString(),
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, membership.TotalFilter,
                membership.Total), JsonRequestBehavior.AllowGet);
        }
    }
}
