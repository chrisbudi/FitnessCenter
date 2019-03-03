using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Master;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.Class;
using Services.DataTables;
using Services.Helpers;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class PosisiController : FitController
    {
        // GET: Master/Posisi
        //[Authorize(Roles="Posisi_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsPosisi(int id)
        {
            tPosisi pos = new DataServicePosisi().GetobjById(id);
            return View(pos);
        }

        //[Authorize(Roles="Posisi_C")]
        public ActionResult CreatePosisi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePosisi(tPosisi pos)
        {
            if (!ModelState.IsValid)
            {
                return View(pos);
            }
            var dPosisi = new DataServicePosisi();
            dPosisi.Insert(pos);
            return RedirectToAction("DetailsPosisi", new { id = pos.PosisiID });
        }

        //[Authorize(Roles = "Posisi_U")]
        public ActionResult EditPosisi(int id)
        {
            var pos = new DataServicePosisi().GetobjById(id);
            return View(pos);
        }

        [HttpPost]
        public ActionResult EditPosisi(tPosisi pos, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(pos);
            }
            var dPosisi = new DataServicePosisi();
            dPosisi.Update(pos);

            return RedirectToAction("DetailsPosisi", new { id = pos.PosisiID });
        }

        //[Authorize(Roles = "Posisi_D")]
        [HttpPost]
        public ActionResult DeletePosisi(tPosisi pos)
        {
            if (!ModelState.IsValid)
            {
                return View("EditPosisi", pos);
            }

            var dPosisi = new DataServicePosisi();
            dPosisi.Delete(pos.PosisiID);

            return RedirectToAction("Index");
        }

        public ActionResult PosisiResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tPosisi> loadPosisi = new DataServicePosisi().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadPosisi.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.PNamaPosisi,
                             Convert.ToString(d.PosisiID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadPosisi.TotalFilter,
                loadPosisi.Total), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetPosisi(string searchTerm, int pageSize, int pageNum)
        {
            var ps = new DataServicePosisi();
            List<tPosisi> attendees = ps.GetData(searchTerm, pageSize, pageNum);
            int attendeeCount = ps.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees
            };
        }

        private Select2PagedResult AttendeesToSelect2Format(List<tPosisi> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.PosisiID.ToString(CultureInfo.InvariantCulture), text = a.PNamaPosisi });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }

        [HttpGet]
        public ActionResult GetPosisiById(int id = 0)
        {
            if (id == 0)
                return Json(null);

            DataServicePosisi dPosisi = new DataServicePosisi();

            var posisis = dPosisi.LoadAllData().Where(m => m.PosisiID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var posisi in posisis.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = posisi.PosisiID.ToString(), text = posisi.PNamaPosisi });
            }
            jsonAttendees.Total = posisis.Count();
            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}