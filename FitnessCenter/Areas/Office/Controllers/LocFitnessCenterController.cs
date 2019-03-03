using System;
using System.Collections.Generic;
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
    public class LocFitnessCenterController : FitController
    {
        // GET: Master/LocFitnessCenter
        //[Authorize(Roles="LocFitnessCenter_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsLoc(int id)
        {
            tLocFitnessCenter loc = new DataServiceLocFitnessCenter().GetobjById(id);
            return View(loc);
        }

        //[Authorize(Roles="LocFitnessCenter_C")]
        public ActionResult CreateLoc()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateLoc(tLocFitnessCenter loc)
        {
            if (!ModelState.IsValid)
            {
                return View(loc);
            }
            var dLoc = new DataServiceLocFitnessCenter();
            dLoc.Insert(loc);
            return RedirectToAction("DetailsLoc", new { id = loc.LocationID });
        }

        //[Authorize(Roles = "LocFitnessCenter_U")]
        public ActionResult EditLoc(int id)
        {
            var loc = new DataServiceLocFitnessCenter().GetobjById(id);
            return View(loc);
        }

        [HttpPost]
        public ActionResult EditLoc(tLocFitnessCenter loc, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(loc);
            }

            var dLoc = new DataServiceLocFitnessCenter();
            dLoc.Update(loc);
            return RedirectToAction("DetailsLoc", new { id = loc.LocationID });
        }

        //[Authorize(Roles = "LocFitnessCenter_D")]
        [HttpPost]
        public ActionResult DeleteLoc(tLocFitnessCenter loc)
        {
            if (!ModelState.IsValid)
            {
                return View("EditLoc", loc);
            }


            var dLoc = new DataServiceLocFitnessCenter();
            dLoc.Delete(loc.LocationID);
            return RedirectToAction("Index");
        }



        [HttpPost]
        public ActionResult GetLocationSource(string searchTerm, int pageSize, int pageNum)
        {
            var ar = new DataServiceLocFitnessCenter();
            List<tLocFitnessCenter> attendees = ar.GetData(searchTerm, pageSize, pageNum).ToList();
            int attendeeCount = ar.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = ObjToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees
            };
        }


        public ActionResult GetLocationByID(int id = 0)
        {
            if (id == 0)
                return Json(null);

            var ars = new DataServiceLocFitnessCenter().LoadAllData().Where(m => m.LocationID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var action in ars.ToList())
            {
                jsonAttendees.Results.Add(new Select2StringResult
                {
                    id = action.LocationID.ToString(),
                    text = $"{action.LAuth} - {action.LAlamat}"
                });
            }


            jsonAttendees.Total = ars.Count();
            
            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            
        }

        private Select2PagedResult ObjToSelect2Format(List<tLocFitnessCenter> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.LocationID.ToString(), text = $"{a.LAuth} - {a.LAlamat}" });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }

        public ActionResult LocResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tLocFitnessCenter> loadLoc = new DataServiceLocFitnessCenter().LoadDataGrid(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadLoc.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             Convert.ToString(d.LocationID),
                             d.LAlamat,
                             d.LTlp,
                             d.LFax,
                             d.LAuth,
                             d.LSpatial
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadLoc.TotalFilter,
                loadLoc.Total), JsonRequestBehavior.AllowGet);
        }
    }
}