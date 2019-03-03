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
    public class MemberStateController : FitController
    {
        // GET: Master/MemberState
        //[Authorize(Roles="MemberState_R")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetailsMS(int id)
        {
            tMemberState ms = new DataServiceMemberState().GetobjById(id);
            return View(ms);
        }

        //[Authorize(Roles="MemberState_C")]
        public ActionResult CreateMS()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMS(tMemberState mem)
        {
            if (!ModelState.IsValid)
            {
                return View(mem);
            }
            var memberState = new DataServiceMemberState();
            memberState.Insert(mem);
            return RedirectToAction("DetailsMS", new { id = mem.MemberStateID });
        }

        //[Authorize(Roles = "MemberState_U")]
        public ActionResult EditMS(int id)
        {
            var state = new DataServiceMemberState().GetobjById(id);
            return View(state);
        }

        [HttpPost]
        public ActionResult EditMS(tMemberState state, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(state);
            }

            var memberState = new DataServiceMemberState();
            memberState.Update(state);
            return RedirectToAction("DetailsMS", new { id = state.MemberStateID });
        }

        //[Authorize(Roles = "MemberState_D")]
        [HttpPost]
        public ActionResult DeleteMS(tMemberState state)
        {
            if (!ModelState.IsValid)
            {
                return View("EditMS", state);
            }

            var memberState = new DataServiceMemberState();
            memberState.Delete(state.MemberStateID);
            return RedirectToAction("Index");
        }

        public ActionResult MemberStateResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tMemberState> loadState = new DataServiceMemberState().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadState.ListClass
                select new object[]
                {
                    Convert.ToString(count++),
                    d.MemberStateName,
                    d.Note,
                    Convert.ToString(d.MemberStateID)
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadState.TotalFilter,
                loadState.Total), JsonRequestBehavior.AllowGet);
        }
    }
}