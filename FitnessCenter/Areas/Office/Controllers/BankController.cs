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
    public class BankController : FitController
    {
        // GET: Master/Bank
        //[Authorize(Roles="Bank_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsBank(int id)
        {
            tBank bank = new DataServiceBank().GetobjById(id);
            return View(bank);
        }

        //[Authorize(Roles="Bank_C")]
        public ActionResult CreateBank()
        {
            //var b = new tBank {NamaBank = "absbsbs", BankID = 2};
            return View();
        }

        [HttpPost]
        public ActionResult CreateBank(tBank bank)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var bnk = new DataServiceBank();
            bnk.Insert(bank);
            return RedirectToAction("DetailsBank", new { id = bank.BankID });
        }

        //[Authorize(Roles = "Bank_U")]
        public ActionResult EditBank(int id)
        {
            var bank = new DataServiceBank().GetobjById(id);
            return View(bank);
        }

        [HttpPost]
        public ActionResult EditBank(tBank bnk, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(bnk);
            }
            var bank = new DataServiceBank();
            bank.Update(bnk);

            return RedirectToAction("DetailsBank", new { id = bnk.BankID });
        }

        //[Authorize(Roles = "Bank_D")]
        [HttpPost]
        public ActionResult DeleteBank(tBank bank)
        {
            if (!ModelState.IsValid)
            {
                return View("EditBank", bank);
            }

            var bnk = new DataServiceBank();
            bnk.Delete(bank.BankID);

            return RedirectToAction("Index");
        }

        public ActionResult PosisiResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tBank> loadBank = new DataServiceBank().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadBank.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.NamaBank,
                             Convert.ToString(d.BankID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadBank.TotalFilter,
                loadBank.Total), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetBank(string searchTerm, int pageSize, int pageNum)
        {
            var bank = new DataServiceBank();
            List<tBank> attendees = bank.GetData(searchTerm, pageSize, pageNum);
            int attendeeCount = bank.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private Select2PagedResult AttendeesToSelect2Format(List<tBank> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.BankID.ToString(), text = a.NamaBank });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }
    }
}