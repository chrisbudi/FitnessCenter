using System;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Activity;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.DataTables;
using Services.Helpers;
using ViewModel.Activity;

namespace FitnessCenter.Areas.Activity.Controllers
{
    public class CheckInOutController : FitController
    {
        // GET: Registrasi/CheckInOut
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string invoice)
        {
            var invString = invoice.Substring(invoice.LastIndexOf('\\') + 1);
            //url = url.Substring(url.IndexOf('/') + 1);

            var member = new DataServiceMembership().GetMemberLogin(invString);
            if (member != null)
            {
                int countAkt = new DataServiceCheckInOut().GetObjCountByMemberID(member.MemberID);
                return countAkt == 0 ? RedirectToAction("CheckIn", new { memId = member.MemberID }) : RedirectToAction("CheckOut", new { memId = member.MemberID });
            }

            ViewBag.Message = "Akun ini tidak di temukan";
            return View();
        }

        public ActionResult CheckIn(string memId)
        {
            if (memId == "")
            {
                return HttpNotFound();
            }

            var dmember = new DataServiceMember();
            var member = dmember.GetobjByMemberId(memId);
            var aktifitas = new MemberActivity()
            {
                Activity = new trAktifitasMember()
                {
                    AMMulai = DateTime.Now,
                    verifikasiMember = "member",
                    verifikasiToken = "token",
                    MemberID = member.MemberID,
                    PersonBOID = User.Person.PersonID,
                    LocationID = User.ActiveLocation,
                    Status = ((char)EnumMemberCheck.In).ToString()
                },
                Member = member
            };
            return View(aktifitas);
        }

        [HttpPost]
        public ActionResult CheckIn(MemberActivity memberActivity, string fotoSource, FormCollection form)
        {
            if (!ModelState.IsValid)
            {
                TempData["id"] = memberActivity.Member.MemberID;
                return View();
            }

            var daktifitas = new DataServiceCheckInOut();
            var dMember = new DataServiceMember();

            using (var scope = new TransactionScope())
            {
                try
                {
                    daktifitas.InsertCheckIn(memberActivity.Activity);
                    var member = dMember.LoadAllData().Single(m => m.MemberID == memberActivity.Activity.MemberID);
                    if (member.MFoto == null)
                    {
                        string base64 = fotoSource.Substring(fotoSource.IndexOf(',') + 1);
                        byte[] data = Convert.FromBase64String(base64);
                        member.MFoto = data;

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

        public ActionResult CheckOut(string memId)
        {
            var dmember = new DataServiceMember();
            var member = dmember.GetobjByMemberId(memId);
            var act = new DataServiceCheckInOut().GetobjByMemberId(memId);
            act.Status = ((char)EnumMemberCheck.Out).ToString();
            var aktifitas = new MemberActivity()
            {

                Activity = act,
                Member = member
            };

            return View(aktifitas);
        }

        [HttpPost]
        public ActionResult CheckOut(MemberActivity memberActivity, string fotoSource)
        {
            if (!ModelState.IsValid)
            {
                TempData["id"] = memberActivity.Member.MemberID;
                return View();
            }
            var daktifitas = new DataServiceCheckInOut();
            var dMember = new DataServiceMember();
            memberActivity.Activity.AMSelesai = DateTime.Now;
            using (var scope = new TransactionScope())
            {
                try
                {
                    daktifitas.UpdateCheckOut(memberActivity.Activity);

                    if (fotoSource != "")
                    {
                        var member = dMember.LoadAllData().Single(m => m.MemberID == memberActivity.Activity.MemberID);

                        string base64 = fotoSource.Substring(fotoSource.IndexOf(',') + 1);
                        byte[] data = Convert.FromBase64String(base64);

                        member.MFoto = data;

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
                         select new object[]
                {
                    Convert.ToString(count++),
                    d.MemberID,
                    d.AMMulai.HasValue? d.AMMulai.Value.ToString("dd-MM-yyyy hh:mm:ss"): "01-01-1900",
                    d.PersonBOID,
                    d.LocationID
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cio.TotalFilter,
                cio.Total), JsonRequestBehavior.AllowGet);
        }
    }
}