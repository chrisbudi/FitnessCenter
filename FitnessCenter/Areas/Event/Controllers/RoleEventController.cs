using DataAccessService.Event;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.Class;
using Services.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FitnessCenter.Areas.Event.Controllers
{
    public class RoleEventController : FitController
    {
        private IServiceRoleEvent _roleManager;

        public RoleEventController(IServiceRoleEvent role)
        {
            _roleManager = role;
        }

        // GET: Event/RoleEvent
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(tRoleEvent role, string submit)
        {
            if (ModelState.IsValid)
            {
                _roleManager.Insert(role);
                ShowMessage(Services.Helpers.EnumMessageType.Information,
                    "Data " + role.EvRoleName + " has been saved!", true);
                return RedirectToAction("Index");
            }
            return View(role);
        }

        public ActionResult Edit(int id)
        {
            return View(_roleManager.Get(id));
        }

        [HttpPost]
        public ActionResult Edit(tRoleEvent role, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                _roleManager.Insert(role);
                ShowMessage(Services.Helpers.EnumMessageType.Information,
                    "Data " + role.EvRoleName + " has been updated!", true);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult RoleEventResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tRoleEvent> loadBank = _roleManager.RoleEventDTable(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadBank.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.EvRoleId,
                             Convert.ToString(d.EvRoleName)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadBank.TotalFilter,
                loadBank.Total), JsonRequestBehavior.AllowGet);
        }
    }
}