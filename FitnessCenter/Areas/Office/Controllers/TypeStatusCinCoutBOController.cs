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
    public class TypeStatusCinCoutBOController : FitController
    {
        // GET: Master/TypeStatusCinCoutBO
        //[Authorize(Roles="TypeStatusCinCoutBO_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            tTypeStatusCinCout mt = new DataServiceTypeStatusCinCoutBO().GetobjById(id);
            return View(mt);
        }

        //[Authorize(Roles="TypeStatusCinCoutBO_C")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(tTypeStatusCinCout stat)
        {
            if (!ModelState.IsValid)
            {
                return View(stat);
            }
            var dStatus = new DataServiceTypeStatusCinCoutBO();
            dStatus.Insert(stat);
            return RedirectToAction("Details", new { id = stat.TypeStatusInOut });
        }

        //[Authorize(Roles = "TypeStatusCinCoutBO_U")]
        public ActionResult Edit(int id)
        {
            var stat = new DataServiceTypeStatusCinCoutBO().GetobjById(id);
            return View(stat);
        }

        [HttpPost]
        public ActionResult Edit(tTypeStatusCinCout stat, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(stat);
            }
            var dStatus = new DataServiceTypeStatusCinCoutBO();
            dStatus.Update(stat);

            return RedirectToAction("Details", new { id = stat.TypeStatusInOut });
        }

        //[Authorize(Roles = "TypeStatusCinCoutBO_D")]
        [HttpPost]
        public ActionResult DeleteSM(tTypeStatusCinCout stat)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", stat);
            }
            var dStatus = new DataServiceTypeStatusCinCoutBO();
            dStatus.Delete(stat.TypeStatusInOut);

            return RedirectToAction("Index");
        }

        public ActionResult TypeStatusCinCoutBOResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tTypeStatusCinCout> loadStatus = new DataServiceTypeStatusCinCoutBO().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadStatus.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.NameStatusInOut,
                             Convert.ToString(d.TypeStatusInOut)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadStatus.TotalFilter,
                loadStatus.Total), JsonRequestBehavior.AllowGet);
        }
    }
}