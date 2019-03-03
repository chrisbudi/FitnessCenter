using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Helpers;
using System.Web.Mvc;
using DataAccessService.Instruktur;
using DataAccessService.Master;
using DataAccessService.PT;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Microsoft.Ajax.Utilities;
using Ninject.Infrastructure.Language;
using Services.Class;
using Services.DataTables;
using Services.Extensions;
using Services.Helpers;

namespace FitnessCenter.Areas.Trainers.Controllers
{
    public class PlanPTController : FitController
    {
        // GET: Trainers/PlanPT
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(int id)
        {
            var plan = new DataServicePlanAktifitasPT().GetobjById(id);
            var pt = new trPlanAktifitasPT()
            {
                PlanAktifitasPTID = plan.PlanAktifitasPTID,
                WaktuMulai = plan.WaktuMulai,
                LocationID = plan.LocationID,
                MemberID = plan.MemberID,
                PersonBOIDPT = plan.tUserBackOffice_PersonBOIDPT.tPerson.PersonID,
                Note = plan.Note
            };
            return new Jsonp()
            {
                Data = pt,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult Edit(trPlanAktifitasPT plan, FormCollection form)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var pl = new DataServicePlanAktifitasPT();
            pl.UpdatePlan(plan);
            return RedirectToAction("Index");
            //return null;
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            var a = form.GetValue("WaktuSesi").AttemptedValue;
            var b = Request.Form["WaktuSesi"];
            //var b = form.GetValue("MemberCreate")
            var plan = new trPlanAktifitasPT()
            {
                //                WaktuMulai = Convert.ToDateTime(Request.Form["WaktuSesi"]),
                //                ActionPTID = Convert.ToInt32(form.GetValue("ActionPTCreate").AttemptedValue),
                //                PersonBOIDPT = form.GetValue("BOIDCreate").AttemptedValue,
                //                LocationID = User.ActiveLocation,
                //                MemberID = form.GetValue("MemberCreate").AttemptedValue,
                //                Note = form.GetValue("NoteCreate").AttemptedValue
            };
            var pt = new DataServicePlanAktifitasPT();
            pt.InsertPlan(plan);

            return null;
        }

        [HttpGet]
        public ActionResult GetEvents()
        {

            var dPlan = new DataServicePlanAktifitasPT();

            var periodNow = DateTime.Now.ToString("yyyyMM");
            var rows = (from d in dPlan.LoadAllData().ToEnumerable()
                        where d.period == DateTime.Now.ToString("yyyyMM")
                        select new
                        {
                            id = d.PlanAktifitasPTID.ToString(),
                            title = d.tUserBackOffice_PersonBOIDPT.tPerson.PNama + " - " + d.tMember.tPerson.PNama,
                            start = DateTime.Now.StartOfWeek(DayOfWeek.Sunday).AddDays(d.DayOfWeek).AddHours(d.WaktuMulai.Hours).AddMinutes(d.WaktuMulai.Minutes).ToString("yyyy-MM-ddTHH:mm:ssZ").Replace('.', ':'),
                            end = DateTime.Now.StartOfWeek(DayOfWeek.Sunday).AddDays(d.DayOfWeek).AddHours(d.WaktuSelesai.Hours).AddMinutes(d.WaktuSelesai.Minutes).ToString("yyyy-MM-ddTHH:mm:ssZ").Replace('.', ':'),
                            description = d.Note,
                            allDay = false,
                            background = d.tUserBackOffice_PersonBOIDPT.Background,
                            boid = d.tUserBackOffice_PersonBOIDPT.BOIDNO,
                            memberid = d.tMember.MemberID,
                            note = d.Note,
                            day = d.DayOfWeek,
                            period = d.period,
                            ptname = d.tUserBackOffice_PersonBOIDPT.tPerson.PNama
                        });


            return Json(rows, JsonRequestBehavior.AllowGet);

        }

        private static DateTime ConvertFromUnixTimestamp(long timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }


        public ActionResult RegisterPTResult([ModelBinder(typeof(MyBinder))] MyCustomRequest requestModel)
        {
            return null;
        }


        [HttpPost]
        public ActionResult PlanInsert(trPlanAktifitasPT plan, FormCollection form, int ptid = 0)
        {
            var dplan = new DataServicePlanAktifitasPT();
            plan.period = DateTime.Now.ToString("yyyyMM");
            plan.LocationID = User.ActiveLocation;
            plan.PersonBOID = User.Person.PersonID;
            plan.PersonalTrainerID = ptid;

            using (TransactionScope scope = new TransactionScope())
            {
                dplan.InsertPlan(plan);
                scope.Complete();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PlanUpdate(trPlanAktifitasPT plan)
        {
            var dplan = new DataServicePlanAktifitasPT();
            plan.period = DateTime.Now.ToString("yyyyMM");
            dplan.UpdatePlan(plan);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveEvent(int PlanAktifitasPTID = 0)
        {
            if (PlanAktifitasPTID == 0)
            {
                return Json(null);
            }

            var dplan = new DataServicePlanAktifitasPT();
            dplan.Delete(PlanAktifitasPTID);

            return RedirectToAction("Index", "PlanPT");
        }

        [HttpPost]
        public ActionResult GetMember(string searchTerm, int pageSize, int pageNum)
        {
            var m = new DataServicePersonalTrainer();
            Select2PagedResult memberList = new Select2PagedResult();

            int listCount = 0;
            IEnumerable<trPersonalTrainer> memberIds = m.GetData(searchTerm, pageSize, pageNum, out listCount);

            memberList.Total = listCount;
            memberList.Results = new List<Select2StringResult>();

            foreach (var memberId in memberIds)
            {
                var result = new Select2StringResult
                {
                    id = memberId.trMembership.tMember.MemberID.ToString(),
                    text = memberId.trMembership.tMember.tPerson.PNama,
                    note = new string[] { memberId.trMembership.tMember.tPerson.PAlamat, memberId.trPersonalTrainerID.ToString() }
                };
                memberList.Results.Add(result);
            }

            memberList.Total = memberIds.Count();

            return new Jsonp
            {
                Data = memberList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public JsonResult UpdateTimeEvent(int id, TimeSpan startTime, TimeSpan endTime, int dayWeek)
        {
            var status = false;
            var errorMessage = "";
            DataServicePlanAktifitasPT dPlan = new DataServicePlanAktifitasPT();
            var plan = dPlan.GetobjById(id);
            plan.WaktuMulai = startTime;
            plan.WaktuSelesai = endTime;
            plan.DayOfWeek = dayWeek;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    dPlan.UpdatePlan(plan);
                    status = true;
                    scope.Complete();
                    errorMessage = "the plan is success updated";
                }
                catch (Exception ex)
                {

                    errorMessage = ex.Message;
                }
            }


            return Json(new { status, errorMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}