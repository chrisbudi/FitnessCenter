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
    public class StatusMemberController : FitController
    {
        // GET: Master/StatusMember
        //[Authorize(Roles="StatusMember_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsSM(int id)
        {
            tStatusMember mt = new DataServiceStatusMember().GetobjById(id);
            return View(mt);
        }

        //[Authorize(Roles="StatusMember_C")]
        public ActionResult CreateSM()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSM(tStatusMember stat)
        {
            if (!ModelState.IsValid)
            {
                return View(stat);
            }
            var dStatus = new DataServiceStatusMember();
            dStatus.Insert(stat);
            return RedirectToAction("DetailsSM", new { id = stat.StatusMID });
        }

        //[Authorize(Roles = "StatusMember_U")]
        public ActionResult EditSM(int id)
        {
            var stat = new DataServiceStatusMember().GetobjById(id);
            return View(stat);
        }

        [HttpPost]
        public ActionResult EditSM(tStatusMember stat, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(stat);
            }
            var dStatus = new DataServiceStatusMember();
            dStatus.Update(stat);

            return RedirectToAction("DetailsSM", new { id = stat.StatusMID });
        }

        //[Authorize(Roles = "StatusMember_D")]
        [HttpPost]
        public ActionResult DeleteSM(tStatusMember stat)
        {
            if (!ModelState.IsValid)
            {
                return View("EditSM", stat);
            }
            var dStatus = new DataServiceStatusMember();
            dStatus.Delete(stat.StatusMID);

            return RedirectToAction("Index");
        }

        public ActionResult StatusMemberResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, FormCollection form)
        {
            Counter<tStatusMember> loadStatus = new DataServiceStatusMember().LoadData(requestModel);

            string a = Request.Form["action"];
            string b = Request.Form["STKet"];

            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadStatus.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.STKet,
                             Convert.ToString(d.StatusMID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadStatus.TotalFilter,
                loadStatus.Total), JsonRequestBehavior.AllowGet);
        }
    }
}