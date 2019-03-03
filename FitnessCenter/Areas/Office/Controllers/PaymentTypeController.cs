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
    public class PaymentTypeController : FitController
    {
        // GET: Master/JenisKartu
        //[Authorize(Roles="JenisKartu_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsPaymentType(int id)
        {
            tPaymentType jns = new DataServicePaymentType().GetobjById(id);
            return View(jns);
        }

        //[Authorize(Roles="JenisKartu_C")]
        public ActionResult CreatePaymentType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePaymentType(tPaymentType jns)
        {
            if (!ModelState.IsValid)
            {
                return View(jns);
            }
            var jenis = new DataServicePaymentType();
            jenis.Insert(jns);
            return RedirectToAction("DetailsPaymentType", new { id = jns.PaymentTypeID });
        }

        //[Authorize(Roles = "JenisKartu_U")]
        public ActionResult EditPaymentType(int id)
        {
            var jns = new DataServicePaymentType().GetobjById(id);
            return View(jns);
        }

        [HttpPost]
        public ActionResult EditPaymentType(tPaymentType jns, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(jns);
            }
            var jenis = new DataServicePaymentType();
            jenis.Update(jns);

            return RedirectToAction("DetailsPaymentType", new { id = jns.PaymentTypeID });
        }

        //[Authorize(Roles = "JenisKartu_D")]
        [HttpPost]
        public ActionResult DeletePaymentType(tPaymentType jns)
        {
            if (!ModelState.IsValid)
            {
                return View("EditPaymentType", jns);
            }

            var jenis = new DataServicePaymentType();
            jenis.Delete(jns.PaymentTypeID);

            return RedirectToAction("Index");
        }

        public ActionResult PaymentResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tPaymentType> loadJenisKartu = new DataServicePaymentType().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadJenisKartu.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.NamaType,
                             Convert.ToString(d.PaymentTypeID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadJenisKartu.TotalFilter,
                loadJenisKartu.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJenisKartuByName(string id)
        {
            var jns = new DataServicePaymentType();
            IQueryable<tPaymentType> attendees = jns.LoadAllData().Where(m => m.NamaType == id);
            int attendeeCount = 1;

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult GetJenisKartu(string searchTerm, int pageSize, int pageNum)
        {
            var jns = new DataServicePaymentType();
            IQueryable<tPaymentType> attendees = jns.GetData(searchTerm, pageSize, pageNum).AsQueryable();
            int attendeeCount = jns.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private Select2PagedResult AttendeesToSelect2Format(IQueryable<tPaymentType> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.PaymentTypeID.ToString(), text = a.NamaType });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }
    }
}