using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using DataAccessService.Activity;
using DataAccessService.Master;
using DataAccessService.PT;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using IdentityModel.Config;
using IdentityModel.Model;
using Microsoft.AspNet.Identity.Owin;
using Services.Class;
using Services.DataTables;
using Services.Helpers;
using ViewModel.Activity;

namespace FitnessCenter.Areas.Activity.Controllers
{
    public class InOutMemberController : FitController
    {

        private IServiceMemberMaster _memberManager;
        private IServiceMembership _membershipManager;
        private readonly IServiceCardStatus _cardstatusManager;


        public InOutMemberController(ServiceMemberMaster memberManager
            , ServiceMembership membershipManager, ServiceCardStatus cardstatusManager)
        {
            _memberManager = memberManager;
            _membershipManager = membershipManager;
            _cardstatusManager = cardstatusManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: Registrasi/CheckInOut
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            var dStatusMember = new DataServiceStatusMember();

            int statusCalonid = dStatusMember.GetStatusId(EnumStatusMember.CalonMember);
            int statusMembershipid = dStatusMember.GetStatusId(EnumStatusMember.Membership);
            var memberstatus = search.Substring(search.LastIndexOf('\\') + 1);

            var membership = _membershipManager.Get().SingleOrDefault(m => (m.ActivationCode == memberstatus || m.tMember.MemberNO == memberstatus) && (m.StatusMID == statusCalonid || m.StatusMID == statusMembershipid));


            var member = _memberManager.Get(search.Substring(search.LastIndexOf('\\') + 1));


            if (membership != null)
            {
                return RedirectToAction("In", new { id = member.MemberID, activation = membership.ActivationCode });
            }

            if (member != null)
            {
                //                var countAkt = new DataServiceCheckInOut().GetObjCountByMemberID(member.MemberID);
                //                return countAkt == 0 ?

                RedirectToAction("In", new { id = member.MemberID });
                //                    RedirectToAction("Out", new { id = member.MemberID });
            }


            ViewBag.Message = "Akun ini tidak di temukan";
            return View();
        }

        public ActionResult Active(string id = "")
        {



            return View();

        }

        public ActionResult In(int id, string activation)
        {
            var dmember = new DataServiceMember();
            var dPersonalTrainer = new DataServicePersonalTrainer();
            var dmembership = new DataServiceMembership();
            var member = dmember.GetobjByMemberId(id);

            var membership = string.IsNullOrWhiteSpace(activation)
                ? dmembership.GetobjByMemberNo(member.MemberNO)
                : dmembership.GetobjByActiveId(activation);

            if (member == null)
                member = membership.tMember;
            //            if (member == null)
            //            {
            //                return HttpNotFound();
            //            }

            var aktifitas = new MemberActivity()
            {
                Activity = new trAktifitasMember()
                {
                    AMMulai = DateTime.Now,
                    verifikasiMember = "member",
                    verifikasiToken = "token",
                    MemberID = member?.MemberID ?? 0,
                    PersonBOID = User.Person.PersonID,
                    LocationID = User.ActiveLocation,
                    Status = EnumMemberCheck.In.GetDescription(),
                },
                Member = member,
                StatusMembership = "Aktif",
            };

            aktifitas.StatusMembership = activation != null ? "Need Activation" : "Member";

            aktifitas.Membership = membership;
            aktifitas.MemberType = dmembership.GetTypeMembership(membership.trMembershipID);


            if (member == null) return View(aktifitas);
            var personalTrainer =
                dPersonalTrainer.LoadAllData()
                    .OrderByDescending(m => m.trMembership.MSTglMulai)
                    .FirstOrDefault(m => m.trMembership.tMember.MemberID == id);


            if (personalTrainer == null)
                return View(aktifitas);

            aktifitas.PersonalTrainer = personalTrainer;

            return View(aktifitas);
        }

        //public ActionResult In()
        //{
        //    //            var dmember = new DataServiceMember();
        //    var dPersonalTrainer = new DataServicePersonalTrainer();
        //    var dmembership = new DataServiceMembership();

        //    var membership = dmembership.GetobjByActiveId(id);

        //    if (membership == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var aktifitas = new MemberActivity()
        //    {
        //        Activity = new trAktifitasMember()
        //        {
        //            AMMulai = DateTime.Now,
        //            verifikasiMember = "member",
        //            verifikasiToken = "token",
        //            PersonBOID = User.Person.PersonID,
        //            LocationID = User.ActiveLocation,
        //            Status = ((char)EnumMemberCheck.In).ToString(),
        //        },
        //        StatusMembership = "Aktif",
        //    };

        //    if (membership != null)
        //    {
        //        aktifitas.StatusMembership = "Need Activation";
        //        aktifitas.Membership = membership;
        //        aktifitas.MemberType = dmembership.GetTypeMembership(membership.trMembershipID);
        //    }

        //    var personalTrainer =
        //        dPersonalTrainer.LoadAllData()
        //            .OrderByDescending(m => m.PTContractDate)
        //            .FirstOrDefault(m => m.tMember.MemberNO == id);


        //    if (personalTrainer == null) return View(aktifitas);
        //    aktifitas.PersonalTrainer = personalTrainer;
        //    //            aktifitas.DateEndPTrainer = personalTrainer.PTContractDate.Value.AddDays(personalTrainer.tPaketPT.PPTMasa.Value);
        //    return View(aktifitas);
        //}


        [HttpPost]
        public ActionResult ActiveMember(MemberActivity memberActivity, string fotoSource, string personIdentitas, FormCollection form)
        {
            //            return View();
            var dMembership = new DataServiceMembership();
            var dperson = new DataServicePerson();
            var dstatusmember = new DataServiceStatusMember();

            var ac = new AccountController(UserManager);
            var dpaymentMember = new DataServicePaymentMember();
            var membership = dMembership.GetobjByActiveId(memberActivity.Membership.ActivationCode);
            var memberType = dpaymentMember.LoadAllData().First(m => m.trMembershipID == membership.trMembershipID).tMemberType;

            var prfix = memberType.prFix + membership.TotalMonth.ToString("00");

            if (membership.MSTglMulai > DateTime.Now)
            {
                prfix = prfix + DateTime.Now.ToString("yyMM");
            }
            else
            {
                prfix = prfix + membership.MSTglMulai.ToString("yyMM");
            }

            var memberNo = membership.AgreementID + prfix;
            var daktifitas = new DataServiceCheckInOut();
            var cardId = _cardstatusManager.Get("Ready").CardStatusID;

            membership.tMember.MemberNO = memberNo;
            membership.tMember.MemberTypeID = memberType.MemberTypeID;
            membership.CardStatus = cardId;
            membership.StatusMID = dstatusmember.GetStatusId(EnumStatusMember.Membership);

            var applicationUser = new ApplicationUser { UserName = memberNo, EmailConfirmed = true };

            var activity = new trAktifitasMember()
            {
                AMMulai = DateTime.Now,
                AMSelesai = null,
                LocationID = User.ActiveLocation,
                MemberID = membership.MemberID,
                PersonBOID = User.Person.PersonID,
                Status = "In",
                verifikasiMember = "",
                verifikasiToken = ""
            };

            string base64 = fotoSource.Substring(fotoSource.IndexOf(',') + 1);
            membership.tMember.MFoto = base64;
            var member = membership.tMember;
            member.MFoto = base64;
            using (var scope = new TransactionScope())
            {
                try
                {
                    dMembership.Update(membership);
                    var user = Task.Run(() => ac.RegAsync(applicationUser, "123456"));
                    var personFinish = dperson.LoadAllData().Single(m => m.PersonID == membership.tMember.tPerson.PersonID);
                    personFinish.Id = user.Result.Id;
                    if (personIdentitas != null)
                        personFinish.PIdentitas = personIdentitas;
                    dperson.Update(personFinish);
                    daktifitas.InsertCheckIn(activity);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.ToString();
                    TempData["id"] = memberActivity.Member.MemberID;
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult In(MemberActivity act, string fotoSource, FormCollection form)
        {
            //if (!ModelState.IsValid)
            //{
            //    TempData["id"] = memberActivity.Member.MemberID;
            //    return View();
            //}

            var daktifitas = new DataServiceCheckInOut();
            var dMember = new DataServiceMember();

            var activity = new trAktifitasMember()
            {
                AMMulai = DateTime.Now,
                AMSelesai = null,
                LocationID = User.ActiveLocation,
                MemberID = act.Membership.MemberID,
                PersonBOID = User.Person.PersonID,
                Status = "In",
                verifikasiMember = "",
                verifikasiToken = ""
            };



            using (var scope = new TransactionScope())
            {
                try
                {

                    daktifitas.InsertCheckIn(activity);
                    var member = dMember.LoadAllData().Single(m => m.MemberID == act.Membership.MemberID);
                    if (member.MFoto == null)
                    {
                        string base64 = fotoSource.Substring(fotoSource.IndexOf(',') + 1);

                        member.MFoto = base64;

                        dMember.Update(member);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Out(int id)
        {
            var dmember = new DataServiceMember();
            var dPersonalTrainer = new DataServicePersonalTrainer();
            var dmembership = new DataServiceMembership();
            var member = dmember.GetobjByMemberId(id);

            var membership = dmembership.GetobjByMemberNo(member.MemberNO);
            //            if (member == null)
            //            {
            //                return HttpNotFound();
            //            }

            var aktifitas = new MemberActivity()
            {
                Activity = new trAktifitasMember()
                {
                    AMMulai = DateTime.Now,
                    verifikasiMember = "member",
                    verifikasiToken = "token",
                    MemberID = member?.MemberID ?? 0,
                    PersonBOID = User.Person.PersonID,
                    LocationID = User.ActiveLocation,
                    Status = ((char)EnumMemberCheck.In).ToString(),
                },
                Member = member,
                StatusMembership = "Aktif",
            };

            if (membership != null)
            {
                aktifitas.StatusMembership = "Need Activation";
                aktifitas.Membership = membership;
                aktifitas.MemberType = dmembership.GetTypeMembership(membership.trMembershipID);
            }

            if (member == null) return View(aktifitas);
            var personalTrainer =
                dPersonalTrainer.LoadAllData()
                    .OrderByDescending(m => m.trMembership.MSTglMulai)
                    .FirstOrDefault(m => m.trMembership.tMember.MemberID == id);


            if (personalTrainer == null) return View(aktifitas);
            aktifitas.PersonalTrainer = personalTrainer;
            //            aktifitas.DateEndPTrainer = personalTrainer.PTContractDate.Value.AddDays(personalTrainer.tPaketPT.PPTMasa.Value);
            return View(aktifitas);
        }

        [HttpPost]
        public ActionResult Out(MemberActivity memberActivity, int id, FormCollection form, string fotoSource)
        {
            var daktifitas = new DataServiceCheckInOut();
            var dMember = new DataServiceMember();

            var aktivitas = daktifitas.GetobjByMemberId(id);

            aktivitas.AMSelesai = DateTime.Now;
            aktivitas.Status = (EnumMemberCheck.Out.GetDescription());

            using (var scope = new TransactionScope())
            {
                try
                {
                    daktifitas.UpdateCheckOut(aktivitas);
                    if (fotoSource != "")
                    {
                        var member = dMember.LoadAllData().Single(m => m.MemberID == id);
                        string base64 = fotoSource.Substring(fotoSource.IndexOf(',') + 1);
                        member.MFoto = base64;
                        dMember.Update(member);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.ToString();
                    TempData["id"] = memberActivity.Member.MemberID;
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult CIOResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var cio = new DataServiceCheckInOut().LoadData(requestModel);

            var count = requestModel.Start + 1;

            var result = from d in cio.ListClass
                         where d.AMSelesai == null
                         select new string[]
                {
                    Convert.ToString(count++),
                    d.tMember.MFoto,
                    d.tMember.tPerson.PNama,
                    d.tMember.MemberNO,
                    d.AMMulai?.ToString("hh:mm:ss") ?? "00:00",
                    "",
                    d.Status,
                    d.tMember.PersonID.ToString()
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cio.TotalFilter,
                cio.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult HistoryResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int memberid, FormCollection form)
        {
            var cio = new DataServiceCheckInOut().LoadHistoryData(requestModel, memberid);

            var count = requestModel.Start + 1;

            var result = from d in cio.ListClass
                         select new object[]
                {
                    Convert.ToString(count++),
                    d.tMember.MemberNO,
                    d.AMMulai?.ToString("dd-MM-yyyy hh:mm:ss") ?? "01-01-1900",
                    d.AMSelesai?.ToString("dd-MM-yyyy hh:mm:ss") ?? "01-01-1900",
                    d.LocationID
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cio.TotalFilter,
                cio.Total), JsonRequestBehavior.AllowGet);
        }
    }
}