using DataAccessService.Event;
using DataAccessService.Master;
using Services.Class;
using Services.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataObjects.Entities;
using FitnessCenter.Areas.Event.Models;
using FitnessCenter.Controllers;

namespace FitnessCenter.Areas.Event.Controllers
{
    public class PersonEventController : FitController
    {
        private IServicePersonEvent _personEventManager;
        private IServiceMemberMaster _memberManager;
        private IServiceBackOfficeMaster _backOfficeManager;
        private IServiceRoleEvent _roleManager;

        public PersonEventController(ServicePersonEvent personEvent,
            ServiceMemberMaster member, ServiceBackOfficeMaster bo, ServiceRoleEvent role)
        {
            _personEventManager = personEvent;
            _memberManager = member;
            _backOfficeManager = bo;
            _roleManager = role;
        }

        // GET: Event/PersonEvent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            PersonEventVm vm = new PersonEventVm()
            {
                RoleEvents = _roleManager.Get().ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(trPersonEvent person)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(trPersonEvent person)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult PersonRegisteredResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<trPersonEvent> load = _personEventManager.PersonEventDTable(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in load.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.tRoleEvent.EvRoleName,
                             d.strPersonEvent.NoPeserta,
                             d.tPerson.PNama,
                             d.tPerson.PAlamat,
                             d.strPersonEvent.Gym,
                             Convert.ToString(d.tPerson.PersonID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, load.TotalFilter,
                load.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BackOfficeResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tUserBackOffice> load = _backOfficeManager.BackOfficeDTable(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in load.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.BOIDNO,
                             d.tPerson.PNama,
                             d.tPosisi.PNamaPosisi,
                             d.tPerson.PGender,
                             d.tPerson.PAlamat,
                             "LIPO",
                             Convert.ToString(d.tPerson)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, load.TotalFilter,
                load.Total), JsonRequestBehavior.AllowGet);

        }

        public ActionResult MemberResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tMember> load = _memberManager.MemberDTable(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in load.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.MemberID,
                             d.tPerson.PNama,
                             d.tPerson.PAlamat,
                             d.tPerson.PGender,
                             "LIPO",
                             Convert.ToString(d.tPerson)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, load.TotalFilter,
                load.Total), JsonRequestBehavior.AllowGet);
        }
    }
}