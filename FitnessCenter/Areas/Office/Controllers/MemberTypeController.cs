using DataAccessService.Master;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.Class;
using Services.DataTables;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class MemberTypeController : FitController
    {
        // GET: Master/MemberType
        //[Authorize(Roles="MemberType_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsMemberType(int id)
        {
            tMemberType mt = new DataServiceMemberType().GetobjById(id);
            return View(mt);
        }

        //[Authorize(Roles="MemberType_C")]
        public ActionResult CreateMemberType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMemberType(tMemberType mem)
        {
            if (!ModelState.IsValid)
            {
                return View(mem);
            }
            var memberType = new DataServiceMemberType();
            memberType.Insert(mem);
            return RedirectToAction("DetailsMemberType", new { id = mem.MemberTypeID });
        }

        //[Authorize(Roles = "MemberType_U")]
        public ActionResult EditMemberType(int id)
        {
            var tipe = new DataServiceMemberType().GetobjById(id);
            return View(tipe);
        }

        [HttpPost]
        public ActionResult EditMemberType(tMemberType tipe, FormCollection frm)
        {

            if (!ModelState.IsValid)
            {
                return View(tipe);
            }
            var memberType = new DataServiceMemberType();

            memberType.Update(tipe);
            return RedirectToAction("DetailsMemberType", new { id = tipe.MemberTypeID });
        }

        //[Authorize(Roles = "MemberType_D")]
        [HttpPost]
        public ActionResult DeleteMemberType(tMemberType tipe)
        {
            if (!ModelState.IsValid)
            {
                return View("EditMemberType", tipe);
            }
            var memberType = new DataServiceMemberType();
            memberType.Delete(tipe.MemberTypeID);
            return RedirectToAction("Index");
        }

        public ActionResult MemberTypeResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tMemberType> loadTipe = new DataServiceMemberType().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadTipe.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.MemberType,
                             d.Biaya,
                             d.JmlBulan,
                             d.IsPaket == true ? "Ya" : "Tidak",

                             d.ShareMin.HasValue ?
                             d.ShareMin.Value + " - " + d.Share :
                             d.Share.ToString(),

                             d.Periode,
                             d.LocationID,
                             Convert.ToString(d.MemberTypeID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadTipe.TotalFilter,
                loadTipe.Total), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMemberTypeById(int id = 0)
        {
            if (id == 0)
                return Json(null);

            DataServiceMemberType dType = new DataServiceMemberType();

            var types = dType.LoadAllData().Where(m => m.MemberTypeID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var action in types.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = action.MemberTypeID.ToString(), text = action.MemberType });
            }

            jsonAttendees.Total = types.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetMemberSharedById(int id = 0)
        {
            if (id == 0)
                return Json(null);

            DataServiceMemberType dType = new DataServiceMemberType();

            var types = dType.GetobjById(id);

            var type = new tMemberType()
            {
                Admin = types.Admin,
                Biaya = types.Biaya,
                IsPaket = types.IsPaket,
                JmlBulan = types.JmlBulan,
                LocationID = types.LocationID,
                MemberType = types.MemberType,
                MemberTypeID = types.MemberTypeID,
                Periode = types.Periode,
                Prorate = types.Prorate,
                Share = types.Share,
                prFix = types.prFix,
                ShareMin = types.ShareMin,
                tPaketPersonalTrainerID = types.tPaketPersonalTrainerID,
                tPaketPT = new tPaketPT()
                {
                    PPTNama = types.tPaketPT?.PPTNama
                }
            };

            return new Jsonp()
            {
                Data = type,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetAllMemberByMemberType(int id, string searchTerm,
            int pageSize, int pageNum, string[] MemberID)
        {
            if (id == 0)
                return Json(null);

            DataServicePaymentMember dType = new DataServicePaymentMember();

            var jsonAttendees = new Select2PagedResult
            {
                Results = new List<Select2StringResult>()
            };

            var types = dType.LoadAllData().Where(m => m.MemberTypeID == id &&
                !(from p in MemberID select p).Contains(m.trMembership_trMembershipID.tMember.MemberNO) &&
                m.trMembership_trMembershipID.tMember.MemberNO.Contains(searchTerm))
                .OrderBy(m => m.trMembershipID)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize);

            foreach (var action in types.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = action.trMembership_trMembershipID.MemberID.ToString(), text = action.trMembership_trMembershipID.tMember.MemberNO });
            }
            return new Jsonp()
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult GetMemberType(string searchTerm, int pageSize, int pageNum)
        {
            var ar = new DataServiceMemberType();
            var type = ar.GetData(searchTerm, pageSize, pageNum).Where(m => m.Status);
            int typeCount = ar.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = MemberTypeToSelect2Format(type, typeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private object MemberTypeToSelect2Format(IEnumerable<tMemberType> attendees, int attendeeCount)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.MemberTypeID.ToString(), text = a.MemberType });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = attendeeCount;

            return jsonAttendees;
        }

    }
}