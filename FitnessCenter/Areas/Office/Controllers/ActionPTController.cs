using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Master;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.Class;
using Services.DataTables;
using System.Globalization;
using Services.Helpers;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class ActionPtController : FitController
    {
        // GET: Master/ActionPT
        //[Authorize(Roles="ActionPT_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsActPT(int id)
        {
            tActionPT act = new DataServiceActionPT().GetobjByID(id);
            return View(act);
        }

        //[Authorize(Roles="ActionPT_C")]
        public ActionResult CreateActPT()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateActPT(tActionPT act)
        {
            if (!ModelState.IsValid)
            {
                return View(act);
            }
            var actPt = new DataServiceActionPT();

            actPt.Insert(act);
            return RedirectToAction("DetailsActPT", new { id = act.ActionPTID });
        }

        //[Authorize(Roles = "ActionPT_U")]
        public ActionResult EditActPT(int id)
        {
            var act = new DataServiceActionPT().GetobjByID(id);
            return View(act);
        }

        [HttpPost]
        public ActionResult EditActPT(tActionPT act, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(act);
            }
            var actPt = new DataServiceActionPT();
            actPt.Update(act);
            return RedirectToAction("DetailsActPT", new { id = act.ActionPTID });
        }

        //[Authorize(Roles = "ActionPT_D")]
        [HttpPost]
        public ActionResult DeleteActPT(tActionPT act)
        {
            if (!ModelState.IsValid)
            {
                return View("EditActPT", act);
            }
            var actPt = new DataServiceActionPT();
            actPt.Delete(act.ActionPTID);
            return RedirectToAction("Index");
        }

        public ActionResult ActionPTResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tActionPT> loadAct = new DataServiceActionPT().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadAct.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.ActionPTName,
                             d.ActionPTKet,
                             Convert.ToString(d.ActionPTID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadAct.TotalFilter,
                loadAct.Total), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetActionPT(string searchTerm, int pageSize, int pageNum)
        {
            var act = new DataServiceActionPT();
            List<tActionPT> attendees = act.GetData(searchTerm, pageSize, pageNum);
            int attendeeCount = act.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetActionPTByid(int id = 0)
        {
            if (id == 0)
                return Json(null, JsonRequestBehavior.AllowGet);
            DataServiceActionPT dOffice = new DataServiceActionPT();

            var offices = dOffice.LoadAllData().Where(m => m.ActionPTID == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var office in offices.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = office.ActionPTID.ToString(), text = office.ActionPTName });
            }

            jsonAttendees.Total = offices.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet  
            };
        }

        


        private Select2PagedResult AttendeesToSelect2Format(List<tActionPT> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.ActionPTID.ToString(CultureInfo.InvariantCulture), text = a.ActionPTName });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }
    }
}