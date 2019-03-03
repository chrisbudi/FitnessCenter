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
    public class CardStatusController : FitController
    {
        private IServiceCardStatus _cardManager;

        public CardStatusController(ServiceCardStatus cardManager)
        {
            _cardManager = cardManager;
        }

        // GET: Master/ActionPT
        //[Authorize(Roles="ActionPT_R")]
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles="ActionPT_C")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(tCardStatu act)
        {
            if (!ModelState.IsValid)
            {
                return View(act);
            }

            _cardManager.Insert(act);
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "ActionPT_U")]
        public ActionResult Edit(int id)
        {
            var act = _cardManager.Get(id);
            return View(act);
        }

        [HttpPost]  
        public ActionResult Edit(tCardStatu act, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(act);
            }
            _cardManager.Insert(act);
            return RedirectToAction("Index");
        }

        public ActionResult CardResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<tCardStatu> loadAct = _cardManager.CardDTable(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadAct.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.CardStatus,
                             d.CardDesc,
                             d.FinalStatus,
                             Convert.ToString(d.CardStatusID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadAct.TotalFilter,
                loadAct.Total), JsonRequestBehavior.AllowGet);
        }

    }
}