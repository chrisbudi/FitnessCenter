using System;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.DataTables;
using DataAccessService.Activity;
using DataAccessService.Master;
using DataAccessService.PT;
using DataObjects.Shared;
using ViewModel.Trainer.Claim;

namespace FitnessCenter.Areas.Trainers.Controllers
{
    public class ClaimPTController : FitController
    {

        private IServiceActionParam _managerActionParam;
        public ClaimPTController(ServiceActionParam managerActionParam)
        {
            this._managerActionParam = managerActionParam;
        }
        // GET: Trainers/ClaimPT
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            var invString = search.Substring(search.LastIndexOf('\\') + 1);

            var member = new DataServicePersonalTrainer().GetobjByMemberId(invString);
            if (member?.trMembership.MemberID != null)
            {
                var ada = new DataServiceClaimPT().GetObjCountByMemberID(member.trMembership.MemberID);
                return ada == 0 ? RedirectToAction("Claim", new { id = member.trMembership.MemberID }) : RedirectToAction("ClaimEnd", new { id = member.trMembership.MemberID });
            }
            ViewBag.Message = "This account not found";
            return View();
        }

        public ActionResult CekClaim(int id)
        {
            //            var ada = new DataServiceClaimPT().GetObjCountByMemberID(id);
            return RedirectToAction("Claim", new { id = id });
        }

        public ActionResult Claim(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            var pt = new DataServiceClaimPT();
            var cpt = pt.GetobjByMemberId(id);
            var m = new DataServiceCheckInOut().GetobjByMemberId(id);
            var klaim = new ClaimModel()
            {
                Claim = new strKlaimPT()
                {
                    AwalClaim = DateTime.Now,
                    verifikasiMember = m.tMember.MemberNO + " - " + m.tMember.tPerson.PNama,
                    verifikasiPT = cpt.tUserBackOffice == null ? "" : cpt.tUserBackOffice.BOIDNO,
                    trPersonalTrainerID = cpt.trPersonalTrainerID,
                    Void = false,
                    AktifitasMemberID = m.AktifitasMemberID
                },
                EndTime = "",
                StartTime = DateTime.Now.ToString("HH:mm")
            };

            return View(klaim);
        }

        [HttpPost]
        public ActionResult Claim(string memberId, ClaimModel model)
        {


            if (!ModelState.IsValid)
            {
                return View();
            }
            var kl = new DataServiceClaimPT();
            var actmember = memberId.Split(',');
            Debug.WriteLine(memberId.Split(',').Length);

            using (TransactionScope scope = new TransactionScope())
            {


                var claim = new strKlaimPT()
                {
                    AktifitasMemberID = model.Claim.AktifitasMemberID,
                    AwalClaim = DateTime.Now,
                    Jam = 0,
                    Note = "",
                    verifikasiMember = model.Claim.verifikasiMember,
                    verifikasiPT = model.Claim.verifikasiPT,
                    trPersonalTrainerID = model.Claim.trPersonalTrainerID
                };
                kl.InsertPT(claim);


                scope.Complete();
            }


            return RedirectToAction("Index");
        }

        public ActionResult ClaimEnd(int id)
        {
            var kl = new DataServiceClaimPT().GetobjById(id);
            var claim = new ClaimModel()
            {
                Claim = kl,
                EndTime = DateTime.Now.ToString("HH:mm"),
                StartTime = kl.AwalClaim.ToString("HH:mm")
            };

            return View(claim);
        }

        [HttpPost]
        public ActionResult ClaimEnd(strKlaimPT klaim, string fotoSource)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var awal = klaim.AwalClaim;
            var akhir = DateTime.Now;
            var pecah = akhir.Subtract(awal);
            var selisih = (int)pecah.TotalSeconds;
            if (selisih > 1800)
            {
                var per = new DataServicePersonalTrainer();
                var pkt = per.GetobjById(klaim.trPersonalTrainerID);
                //pkt.SisaJam = (selisih <= 5400) ? (pkt.SisaJam = (pkt.SisaJam) - 1) : (pkt.SisaJam = (pkt.SisaJam) - 2);
                pkt.SisaJam = (pkt.SisaJam) - 1;
                per.UpdatePT(pkt);
            }
            else
            {
                klaim.Void = true;
            }
            klaim.AkhirClaim = DateTime.Now;
            var kl = new DataServiceClaimPT();
            kl.UpdatePT(klaim);
            return RedirectToAction("Index");
        }

        public ActionResult ForceStop(int id)
        {
            var kl = new DataServiceClaimPT().GetobjById(id);
            return View(kl);
        }

        [HttpPost]
        public ActionResult ForceStop(strKlaimPT klaim, string fotoSource)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            klaim.AkhirClaim = klaim.AwalClaim.AddHours(1);
            klaim.Jam = klaim.AkhirClaim.Hour - klaim.AwalClaim.Hour;
            var kl = new DataServiceClaimPT();

            kl.UpdatePT(klaim);

            var per = new DataServicePersonalTrainer();
            trPersonalTrainer pkt = per.GetobjById(klaim.trPersonalTrainerID);
            pkt.SisaJam = (pkt.SisaJam) - 1;
            per.UpdatePT(pkt);
            return RedirectToAction("Index");
        }
        public ActionResult ClaimPTResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var cpt = new DataServiceClaimPT().LoadData(requestModel);

            var count = requestModel.Start + 1;

            var result = from d in cpt.ListClass
                             //where d.trAktifitasMember.AMSelesai == null
                         select new object[]
                {
                    Convert.ToString(count++),
                    d.trPersonalTrainer.trMembership.tMember.MemberNO,
                    d.trPersonalTrainer.trMembership.tMember.tPerson.PNama,
                    d.trPersonalTrainer.SisaJam,
                    d.trPersonalTrainer.tUserBackOffice.tPerson.PNama,
                    d.AwalClaim.ToString("HH:mm"),
                    d.AkhirClaim.ToString("HH:mm"),
                    d.trPersonalTrainer.strKlaimPTs.Count(m => m.AkhirClaim == DateTime.MinValue.AddYears(1899))> 0 ? "In Claim" : "Not In Claim",
                    d.trPersonalTrainer.trMembership.tMember.MemberNO
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cpt.TotalFilter,
                cpt.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PlanPTResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var cpt = new DataServicePlanAktifitasPT().LoadDataPeriod(requestModel);

            var count = requestModel.Start + 1;

            var result = from d in cpt.ListClass
                         select new object[]
                         {
                             Convert.ToString(count++),
                             d.tUserBackOffice_PersonBOIDPT.tPerson.PNama,
                             d.tMember.tPerson.PNama + " - " + d.tMember.tPerson.PAlamat,
                             DateTime.Now.StartOfWeek(DayOfWeek.Sunday).AddDays(d.DayOfWeek).AddHours(d.WaktuMulai.Hours).AddMinutes(d.WaktuMulai.Minutes).ToString("dddd"),
                             DateTime.Now.StartOfWeek(DayOfWeek.Sunday).AddDays(d.DayOfWeek).AddHours(d.WaktuMulai.Hours).AddMinutes(d.WaktuMulai.Minutes).ToString("HH:mm").Replace('.', ':'),
                             DateTime.Now.StartOfWeek(DayOfWeek.Sunday).AddDays(d.DayOfWeek).AddHours(d.WaktuMulai.Hours).AddMinutes(d.WaktuMulai.Minutes).ToString("HH:mm").Replace('.', ':'),
                             d.Note,
                             $"{d.trPersonalTrainer.trPersonalTrainerID}_{d.trPersonalTrainer.strKlaimPTs.Any(m => m.AwalClaim.Date == DateTime.Now.Date)}",

                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cpt.TotalFilter,
                cpt.Total), JsonRequestBehavior.AllowGet);
        }


        public ActionResult MemberClaimHistory([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int id)
        {
            var cpt = new DataServicePersonalTrainer().LoadDataClaim(requestModel, id, User.ActiveLocation);
            var count = requestModel.Start + 1;
            var result = from d in cpt.ListClass
                         select new object[]
                {
                    Convert.ToString(count++),
                    d.PersonalTrainer.trMembership.tMember.MemberNO,
                    d.AktifitasMember.Status,
                    d.AktifitasMember.AMMulai,
                    d.AktifitasMember.AMSelesai,
                    d.PersonalTrainer.tUserBackOffice.tPerson.PNama
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cpt.TotalFilter,
                cpt.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadActionPersonalTrainer()
        {

            var model = new DataServiceActionPT().LoadAllData();

            return PartialView("Editor/ActionType", model);
        }

        public ActionResult AddClaimProgram(int seq, string actionpt)
        {
            var model = new StrActionKlaimParam()
            {
                StrActionKlaim = new StrActionKlaim()
                {
                    tActionPT = new tActionPT()
                    {
                        ActionPTName = actionpt
                    }
                }
            };
            ViewBag.seq = seq;
            return PartialView("Editor/Program", model);
        }

        [HttpGet]
        public ActionResult LoadActionClaimParam(string query = "")
        {
            var actionparam = _managerActionParam.Get().Where(m => m.NamaParam.Contains(query));
            var param = (from p in actionparam
                         select new
                         {
                             //                             p.desc,
                             //                             date = p.date.ToString()
                         }).Distinct();

            return Json(param, JsonRequestBehavior.AllowGet);
        }
    }
}