using DataAccessService.Event;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.Class;
using Services.DataTables;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FitnessCenter.Areas.Event.Controllers
{
    public class EventController : FitController
    {
        private IServiceEvent _eventManager;

        public EventController(ServiceEvent even)
        {
            _eventManager = even;

        }

        // GET: Event/Event
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(tEvent even, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                _eventManager.Insert(even);
                return RedirectToAction("Index");
            }

            return View(even);
        }

        public ActionResult Edit(int id = 0)
        {
            var even = _eventManager.Get(id);
            return View(even);
        }

        [HttpPost]
        public ActionResult Edit(tEvent even)
        {
            if (ModelState.IsValid)
            {
                _eventManager.Insert(even);
                return RedirectToAction("Index");
            }

            return View(even);
        }

        public ActionResult StepEntryRow()
        {
            return PartialView("StepEntryEditor");
        }

        public ActionResult ScoreEntryTable()
        {
            return PartialView("ScoreEntryTable");
        }

        public ActionResult ScoreEntryRow(string index)
        {
            ViewBag.index = index;
            return PartialView("ScoreEntryEditor");
        }

        public ActionResult EventResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tEvent> load = _eventManager.EventDTable(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in load.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.EvName,
                             d.EvStartDate.ToString("dd-MM-yyyy") + " - " + d.EvEndDate.ToString("dd-MM-yyyy"),
                             d.JumlahPesertaAwal + "(" + d.trPersonEvents.Count() + ")",
                             Convert.ToString(d.EvId)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, load.TotalFilter,
                load.Total), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult SelectEvent(string searchTerm, int pageSize, int? pageNum)
        {
            int count;
            List<tEvent> even = _eventManager.SelectData(searchTerm ?? "", pageSize, pageNum ?? 1, out count);

            Select2PagedResult select2 = new Select2PagedResult();
            select2.Results = (from e in even
                               select new Select2StringResult
                               {
                                   id = e.EvId.ToString(),
                                   text = e.EvName
                               }).ToList();
            select2.Total = count;

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = select2,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult SelectEventSingle(int id = 0)
        {
            if (id == 0)
                return Json(null);

            var types = _eventManager.Get(id);

            var jsonAttendees = new Select2PagedResult
            {
                Results = new List<Select2StringResult>()
                {

                    new Select2StringResult()
                    {
                        id = id.ToString(),
                        text = types.EvName
                    }
                }
            };

            jsonAttendees.Total = 0;

            return new Jsonp
            {
                Data = jsonAttendees,
            };
        }
    }
}