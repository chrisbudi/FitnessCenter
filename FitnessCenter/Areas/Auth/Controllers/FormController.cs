using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Identity;
using FitnessCenter.Controllers;
using IdentityModel.Model;
using Services.Class;
using Services.DataTables;

namespace FitnessCenter.Areas.Auth.Controllers
{
    public class FormController : FitController
    {
        // GET: Forms
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateModule()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateModule(AspForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var bank = new FormsDataService();
            bank.Insert(form);

            return RedirectToAction("Index");
        }

        public ActionResult EditModule(int id)
        {
            AspForm bank = new FormsDataService().GetobjByID(id);
            return View(bank);
        }

        [HttpPost]
        public ActionResult EditModule(AspForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var bank = new FormsDataService();
            bank.Update(form);
            return View("Index");
        }

        public ActionResult CreateDetailForm(int id)
        {
            AspForm bank = new FormsDataService().GetObjParentById(id);
            return View(bank);
        }

        [HttpPost]
        public ActionResult CreateDetailForm(
            [Bind(Include = "parent_ID, Module ,title ,desciption ,controller ,action ,area")] AspForm form,
            int parentId, FormCollection forms)
        {
            form.AspForm2 = null;
            form.parent_ID = parentId;
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var bank = new FormsDataService();
            bank.Insert(form);
            new RolesDataService().FormRoleSave(form.controller);
            return RedirectToAction("Index");
        }

        public ActionResult EditDetailForm(int id)
        {
            AspForm form = new FormsDataService().GetobjByID(id);
            return View(form);
        }

        [HttpPost]
        public ActionResult EditDetailForm(
            [Bind(Include = "FormId, parent_ID, Module ,title ,desciption ,controller ,action ,area")] AspForm form,
            string formName)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var bank = new FormsDataService();
            bank.Update(form);
            var role = new RolesDataService();
            role.FormRoleDelete(formName);
            role.FormRoleSave(form.controller);

            return RedirectToAction("Index");
        }

        public ActionResult FormResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<AspForm> form = new FormsDataService().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<string[]> result = from d in form.ListClass
                                           select new[]
                {
                    Convert.ToString(d.FormId),
                    Convert.ToString(count++),
                    Convert.ToString(d.Module),
                    d.title,
                    d.desciption
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, form.TotalFilter,
                form.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FormDetailResult([ModelBinder(typeof(MyBinder))] MyCustomRequest requestModel)
        {
            Counter<AspForm> diagnosa = new FormsDataService().CustomLoadDataa(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<string[]> result = from d in diagnosa.ListClass
                                           select new[]
                {
                    Convert.ToString(count++),
                    d.title,
                    d.controller,
                    d.action,
                    d.desciption,
                    d.FormId.ToString()
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, diagnosa.TotalFilter,
                diagnosa.Total), JsonRequestBehavior.AllowGet);
        }



    }
}