using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Master;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.Class;
using Services.DataTables;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class SalesActionController : FitController
    {
        // GET: Master/SalesAction
        //[Authorize(Roles="SalesAction_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsSA(int id)
        {
            tSalesAction sa = new DataServiceSalesAction().GetobjById(id);
            return View(sa);
        }

        //[Authorize(Roles="SalesAction_C")]
        public ActionResult CreateSA()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSA(tSalesAction act)
        {
            if (!ModelState.IsValid)
            {
                return View(act);
            }
            var dSales = new DataServiceSalesAction();
            dSales.Insert(act);
            return RedirectToAction("DetailsSA", new { id = act.SalesActionID });
        }

        //[Authorize(Roles = "SalesAction_U")]
        public ActionResult EditSA(int id)
        {
            var act = new DataServiceSalesAction().GetobjById(id);
            return View(act);
        }

        [HttpPost]
        public ActionResult EditSA(tSalesAction act, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(act);
            }

            var dSales = new DataServiceSalesAction();
            dSales.Update(act);
            return RedirectToAction("DetailsSA", new { id = act.SalesActionID });
        }

        //[Authorize(Roles = "SalesAction_D")]
        [HttpPost]
        public ActionResult DeleteSA(tSalesAction act)
        {
            if (!ModelState.IsValid)
            {
                return View("EditSA", act);
            }

            var dSales = new DataServiceSalesAction();
            dSales.Delete(act.SalesActionID);
            return RedirectToAction("Index");
        }

        public ActionResult SalesActionResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tSalesAction> loadAct = new DataServiceSalesAction().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadAct.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.ActionName,
                             d.Note,
                             Convert.ToString(d.SalesActionID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadAct.TotalFilter,
                loadAct.Total), JsonRequestBehavior.AllowGet);
        }
    }
}