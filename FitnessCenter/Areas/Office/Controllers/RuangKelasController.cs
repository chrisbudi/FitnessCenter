using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessService.Identity;
using DataAccessService.Master;
using DataObjects.Entities;
using FitnessCenter.Areas.Office.Models;
using Services.Class;
using Services.DataTables;
using Services.Helpers;


namespace FitnessCenter.Areas.Office.Controllers
{
    public class RuangKelasController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetRuangKelas()
        {
            DataServiceRuangKelas druang = new DataServiceRuangKelas();

            var actions = druang.LoadAllData();

            var jsonAttendees = new Select2PagedResult() { Results = new List<Select2StringResult>() };

            var tSalesActions = actions as IList<tRuangKela> ?? actions.ToList();
            foreach (var action in tSalesActions.ToList())
            {
                jsonAttendees.Results.Add(item: new Select2StringResult { id = action.RuangKelasID.ToString(), text = action.NRuangKelas });
            }
            jsonAttendees.Total = tSalesActions.Count();
            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public ActionResult DetailsRuangKelas(int id)
        {
            tRuangKela sa = new DataServiceRuangKelas().GetobjById(id);
            return View(sa);
        }

        //[Authorize(Roles="SalesAction_C")]
        public ActionResult CreateRuangKelas()
        {
            tRuangKela ruang = new tRuangKela() { BackGround = "#ffffff" };
            return View(ruang);
        }

        [HttpPost]
        public ActionResult CreateRuangKelas(tRuangKela act)
        {
            if (!ModelState.IsValid)
            {
                return View(act);
            }
            var dSales = new DataServiceRuangKelas();
            dSales.Insert(act);
            return RedirectToAction("DetailsRuangKelas", new { id = act.RuangKelasID });
        }

        //[Authorize(Roles = "SalesAction_U")]
        public ActionResult EditRuangKelas(int id)
        {
            var act = new DataServiceRuangKelas().GetobjById(id);
            return View(act);
        }

        [HttpPost]
        public ActionResult EditRuangKelas(tRuangKela act, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(act);
            }

            var dSales = new DataServiceRuangKelas();
            dSales.Update(act);
            return RedirectToAction("DetailsRuangKelas", new { id = act.RuangKelasID });
        }

        //[Authorize(Roles = "SalesAction_D")]
        [HttpPost]
        public ActionResult DeleteRuangKelas(tRuangKela act)
        {
            if (!ModelState.IsValid)
            {
                return View("EditRuangKelas", act);
            }

            var dSales = new DataServiceSalesAction();
            dSales.Delete(act.RuangKelasID);
            return RedirectToAction("Index");
        }

        public ActionResult RuangKelasResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tRuangKela> loadAct = new DataServiceRuangKelas().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadAct.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.NRuangKelas,
                             d.BackGround,
                             Convert.ToString(d.RuangKelasID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadAct.TotalFilter,
                loadAct.Total), JsonRequestBehavior.AllowGet);
        }
    }
}