using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessCenter.Areas.Accounting.Controllers
{
    public class CollectionController : Controller
    {
        // GET: Accounting/FormCollection
        public ActionResult Index()
        {
            return View();
        }

        // GET: Accounting/FormCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Accounting/FormCollection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounting/FormCollection/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Accounting/FormCollection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Accounting/FormCollection/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
