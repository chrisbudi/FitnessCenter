using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using FitnessCenter.Controllers;
using IdentityModel.Config;
using IdentityModel.Model;
using Microsoft.AspNet.Identity.Owin;
using Services.DataTables;
using Services.Helpers;

namespace FitnessCenter.Areas.Registrasi.Controllers
{
    public class MembershipController : FitController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: Registrasi/Membership
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddDataPaymentDetail(string aggreementid, decimal amount, string paymentType)
        {
            var model = new ViewModelMembershipPaymentDetail
            {
                AgreementId = aggreementid,
                PaymentAmount = amount,
                PaymentType = paymentType
            };

            return PartialView("Editor/PaymentDetailEntityEditor", model);
        }

        public ActionResult GetMemberObj(int id)
        {
            var dPerson = new DataServicePerson();
            var dMember = new DataServiceMember();

            var member = dMember.GetobjByPersonId(id);
            var person = dPerson.GetobjById(member.PersonID);

            var pvm = new
            {
                person.PNama,
                PGender = person.PGender == "M" ? "Male" : "Female",
                person.PIdentitas,
                tglLahirString = person.PTglLahir.Value.ToString("dd-MM-yyyy"),
                person.PHP1,
                person.PersonID,
                person.PPinBB,
                member.MemberID
            };

            return new Jsonp
            {
                Data = pvm,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult CreateMembership(int id = 0)
        {
            if (id == 0)
                return HttpNotFound();
            var dMembership = new DataServiceMembership();
            var person = new tPerson { PersonID = id };
            var membership = dMembership.Create(person, User.ActiveLocation);
            return View(membership);
        }

        [HttpPost]
        public ActionResult CreateMembership(ViewModelMembershipCreate membershipCreate, FormCollection form)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var ac = new AccountController(UserManager);
            var dPerson = new DataServicePerson();
            var dMember = new DataServiceMember();
            var dMembership = new DataServiceMembership();
            var dPaymentMember = new DataServicePaymentMember();
            var dpaymentWith = new DataServicePaymentWith();
            var dPaymentType = new DataServicePaymentMember();

            var listAggreementId = membershipCreate.Persons.Select(t => t.AgreementId).ToList();

            var memberships =
                dMembership.LoadAllData()
                    .Where(m => listAggreementId.Contains(m.AgreementID));
            using (var ts = new TransactionScope())
            {
                foreach (var tmembership in memberships)
                {
                    var memberExt = tmembership.AgreementID +
                                    dPaymentMember.GetPaymentByMembership(tmembership.trMembersipID)
                                        .First()
                                        .tMemberType.prFix +
                                    membershipCreate.Payments.Single(m => m.AgreementID == tmembership.AgreementID)
                                        .TotalMonth.Value.ToString("00") + DateTime.Now.ToString("yyMM");

                    var applicationUser = new ApplicationUser { UserName = memberExt, EmailConfirmed = true };

                    var str = Task.Run(() => ac.Reg(applicationUser, "123456"));

                    var personID = 0;
                    if (tmembership.seq == 1)
                    {
                        var tmembership1 = tmembership;
                        var ps = dPerson.GetobjById(tmembership1.PersonID);
                        var person = membershipCreate.Persons.First();
                        ps.PNama = person.PNama;
                        ps.PPinBB = person.PPinBB;
                        ps.PTglLahir = person.PTglLahir;
                        ps.PHP1 = person.PHP1;
                        ps.PAlamat = person.PAlamat;
                        ps.PTelp = person.PTelp;
                        ps.PEmail = person.PEmail;
                        ps.Id = str.Result.Id;
                        ps.PGender = person.PGender;
                        ps.PIdentitas = person.PIdentitas;

                        personID = person.PersonID;

                        dPerson.Update(ps);
                    }

                    if (tmembership.seq == 2)
                    {
                        var tmembership1 = tmembership;

                        var ps = membershipCreate.Persons.Single(p => p.AgreementId == tmembership1.AgreementID);
                        var person = new tPerson
                        {
                            PNama = ps.PNama,
                            PAlamat = ps.PAlamat,
                            PTglLahir = ps.PTglLahir,
                            PHP1 = ps.PHP1,
                            PTelp = ps.PTelp,
                            PPinBB = ps.PPinBB,
                            PEmail = ps.PAlamat,
                            Id = str.Result.Id,
                            PGender = ps.PGender,
                            PIdentitas = ps.PIdentitas
                        };

                        dPerson.Insert(person);


                        if (tmembership.seq == 2)
                        {
                            personID = dPerson.GetLastPersonId();
                            tmembership.PersonID = personID;
                        }
                    }
                    tmembership.StatusMID = new DataServiceStatusMember().GetStatusId(EnumStatusMember.Member);
                    var member = new tMember()
                    {
                        PersonID = personID,
                        MemberID = memberExt,
                        MMulai = DateTime.Now,
                    };
                    dMember.Insert(member);
                    var strLocMember = new strLocMember
                    {
                        MemberID = member.MemberID,
                        LocationID = User.ActiveLocation
                    };

                    dMembership.MemberLocSave(strLocMember);
                    tmembership.MemberID = member.MemberID;
                    dMembership.Update(tmembership);

                    var payM = dPaymentMember.GetPaymentByMembership(tmembership.trMembersipID).First();
                    payM.Note = membershipCreate.Payments.First(m => m.AgreementID == tmembership.AgreementID).Note;
                    dPaymentMember.Update(payM);

                    foreach (var payment in membershipCreate.PaymentDetails)
                    {
                        if (payment.AgreementId == tmembership.AgreementID)
                        {
                            var payWith = new trPaymentWith
                            {
                                BankID = payment.Bank,
                                PaymentTypeID = dPaymentType.GetIdByName(payment.PaymentType),
                                NoKartu = payment.NoKartu,
                                Pemegang = payment.Pemegang,
                                ValidUntil = payment.ValidUntil,
                                payAmount = payment.PaymentAmount
                            };
                            dpaymentWith.Insert(payWith);
                            var payMember = dPaymentMember.GetPaymentByMembership(tmembership.trMembersipID).First();
                            var pay = new strPayment
                            {
                                trPaymentID = payMember.trPaymentID,
                                PaymentWithID = payWith.PaymentWithID
                            };
                            dPaymentMember.InsertstrPayment(pay);
                        }
                    }
                }
                ts.Complete();
            }
            return RedirectToAction("Index");
        }

        public ActionResult MembershipResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var cm = new DataServiceMembership().LoadData(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in cm.ListClass
                         select new[]
                {
                    Convert.ToString(count++),
                    d.tPerson.PNama,
                    d.tPerson.PGender == "M"? "Male":"Female",
                    d.tPerson.PAlamat,
                    d.tPerson.PTelp,
                    d.tPerson.PHP1,
                    d.strAktivitasSales.OrderByDescending(m => m.AktivitasSalesID)
                        .Take(1)
                        .Single()
                        .tSalesAction.ActionName,
                    d.strAktivitasSales.OrderByDescending(m => m.AktivitasSalesID)
                        .Take(1)
                        .Single()
                        .tMemberState.MemberStateName,
                    Convert.ToString(d.PersonID)
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cm.TotalFilter,
                cm.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PersonList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel,
            int memberType)
        {
            var cm = new DataServiceMembership().PersonLoadData(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in cm.ListClass
                         select new[]
                {
                    Convert.ToString(count++),
                    Convert.ToString(d.PersonID),
                    d.MemberID,
                    d.tPerson.PNama,
                    d.tPerson.PGender == "M" ? "Male" : "Female",
                    d.tPerson.PIdentitas,
                    d.tPerson.PTglLahir.Value.ToString("dd-MM-yyyy"),
                    d.tPerson.PHP1,
                    d.tPerson.PPinBB
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cm.TotalFilter,
                cm.Total), JsonRequestBehavior.AllowGet);
        }
    }
}