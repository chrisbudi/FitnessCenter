using System;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Identity;
using FitnessCenter.Controllers;
using IdentityModel.Model;
using Services.DataTables;
using ViewModel.Identity;

namespace FitnessCenter.Areas.Auth.Controllers
{
    public class FormAuthController : FitController
    {
        // GET: FormAuth
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateFormAuth()
        {
            var form = new FormAuthDataService().Create();
            return View(form);
        }

        [HttpPost]
        public ActionResult CreateFormAuth(FormAuth form)
        {
            var app = (from op in form.Crud
                       where op.Selected == "on"
                       select new ApplicationGroupRole
                       {
                           ApplicationGroupId = form.GroupId,
                           ApplicationRoleId = op.Id
                       }).ToList();
            var data = new FormAuthDataService();
            data.Insert(app);


            return RedirectToAction("Index");
        }

        public ActionResult EditFormAuth(string id)
        {
            var form = new FormAuthDataService().Create();
            return View(form);
        }

        [HttpPost]
        public ActionResult EditFormAuth(FormAuth form)
        {
            var app = (from op in form.Crud
                       where op.Selected == "on"
                       select new ApplicationGroupRole
                       {
                           ApplicationGroupId = form.GroupId,
                           ApplicationRoleId = op.Id
                       }).ToList();
            var data = new FormAuthDataService();
            data.Update(app);
            return View();
        }

        public ActionResult FormResult([ModelBinder(typeof(MyBinder))] MyCustomRequest requestModel)
        {
            throw new Exception("Not woking yet");
            //            var form = new FormAuthDataService().CustomLoadDataCreate(requestModel);
            //            var count = requestModel.Start + 1;
            //            var controlCount = 0;
            //            var result = from d in form.ListClass
            //                         select new[]
            //                {
            //                    Convert.ToString(count),
            //                    d.Title,
            //                    d.ControlRead + "_" + (controlCount++),
            //                    d.ControlCreate + "_" + (controlCount++),
            //                    d.ControlUpdate + "_" + (controlCount++),
            //                    d.ControlDelete + "_" + (controlCount++),
            //                    Convert.ToString(count++)
            //                };
            //            return Json(new DataTablesResponse(
            //                requestModel.Draw,
            //                result, diagnosa.TotalFilter,
            //                diagnosa.Total), JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index");
        }


    }
}