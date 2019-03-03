using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Instruktur;
using DataAccessService.PT;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Ninject.Infrastructure.Language;
using Services.Class;
using Services.DataTables;
using Services.Helpers;

namespace FitnessCenter.Areas.Instructor.Controllers
{
    public class PlanController : FitController
    {
        // GET: Trainers/PlanPT
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(int id)
        {
            var plan = new DataServicePlanKelas().GetobjById(id);
            var pt = new trPlanKela()
            {
                PlanKelasID = plan.PlanKelasID,
                WaktuMulai = plan.WaktuMulai,
                WaktuSelesai = plan.WaktuSelesai,
                LocationID = plan.LocationID,
                PersonBOIDInstruktur = plan.tUserBackOffice_PersonBOIDInstruktur.tPerson.PersonID
            };
            return new Jsonp()
            {
                Data = pt,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult Edit(trPlanKela plan, FormCollection form)
        {
            if (!ModelState.IsValid)
            {
                return null;
            }
            var pl = new DataServicePlanKelas();
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
            var plan = new trPlanKela();
            {
                //                WaktuMulai = Convert.ToDateTime(Request.Form["WaktuSesi"]),
                //                ActionPTID = Convert.ToInt32(form.GetValue("ActionPTCreate").AttemptedValue),
                //                PersonBOIDPT = form.GetValue("BOIDCreate").AttemptedValue,
                //                LocationID = User.ActiveLocation,
                //                MemberID = form.GetValue("MemberCreate").AttemptedValue,
                //                Note = form.GetValue("NoteCreate").AttemptedValue
            };
            var pt = new DataServicePlanKelas();
            pt.InsertPlan(plan);

            return null;
        }

        [HttpGet]
        public ActionResult GetEvents()
        {

            var dPlan = new DataServicePlanKelas();

            var periodNow = DateTime.Now.ToString("yyyyMM");
            var rows = (from d in dPlan.LoadAllData().ToEnumerable()
                        where d.period == DateTime.Now.ToString("yyyyMM")
                        select new
                        {
                            id = d.PlanKelasID.ToString(),
                            title = d.tUserBackOffice_PersonBOIDInstruktur.tPerson.PNama + " - " + d.tKela.KNamaKelas,
                            start = DateTime.Now.StartOfWeek(DayOfWeek.Sunday).AddDays(d.DayOfWeek).AddHours(d.WaktuMulai.Hours).AddMinutes(d.WaktuMulai.Minutes).ToString("yyyy-MM-ddTHH:mm:ssZ").Replace('.', ':'),
                            end = DateTime.Now.StartOfWeek(DayOfWeek.Sunday).AddDays(d.DayOfWeek).AddHours(d.WaktuSelesai.Hours).AddMinutes(d.WaktuSelesai.Minutes).ToString("yyyy-MM-ddTHH:mm:ssZ").Replace('.', ':'),
                            allDay = false,
                            background = d.tUserBackOffice_PersonBOIDInstruktur.Background,
                            boid = d.tUserBackOffice_PersonBOIDInstruktur.BOIDNO,
                            day = d.DayOfWeek,
                            period = d.period,
                            ptname = d.tUserBackOffice_PersonBOIDInstruktur.tPerson.PNama
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
        public ActionResult PlanInsert(trPlanKela plan)
        {
            var dplan = new DataServicePlanKelas();
            plan.period = DateTime.Now.ToString("yyyyMM");
            plan.LocationID = User.ActiveLocation;
            plan.PersonBOIDAdm = User.Person.PersonID;


            using (TransactionScope scope = new TransactionScope())
            {
                dplan.InsertPlan(plan);
                scope.Complete();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult PlanUpdate(trPlanKela plan)
        {
            var dplan = new DataServicePlanKelas();
            plan.period = DateTime.Now.ToString("yyyyMM");
            dplan.UpdatePlan(plan);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveEvent(int PlanKelasID = 0)
        {
            if (PlanKelasID == 0)
            {
                return Json(null);
            }

            var dplan = new DataServicePlanKelas();
            dplan.Delete(PlanKelasID);

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
                    //                    id = memberId.MemberID?.ToStringInvariant(),
                    text = $"{memberId.trMembership.tMember.tPerson.PNama} - {memberId.trMembership.tMember.tPerson.PAlamat} "
                };

                var nama = from p in memberId.trPersonalTrainers
                           select new
                           {
                               nama = $"{p.trMembership.MemberID} - {p.trMembership.tMember.tPerson.PNama}",
                               alamat = p.trMembership.tMember.tPerson.PAlamat
                           };

                result.note = new string[nama.Count() - 1];
                for (int i = 0; i < nama.Count() - 1; i++)
                {
                    result.note[i] = nama.ToArray()[i].nama + " - " + nama.ToArray()[i].alamat;
                }

                memberList.Results.Add(result);
            }

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
            DataServicePlanKelas dPlan = new DataServicePlanKelas();
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