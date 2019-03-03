using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Identity;
using FitnessCenter.Controllers;
using IdentityModel.Model;
using Services.Class;
using Services.DataTables;

namespace FitnessCenter.Areas.Auth.Controllers
{
    public class FormRoleController : FitController
    {
        // GET: Group
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GroupAddForm()
        {
            var form = new FormRoleDataService().Create();
            return View(form);
        }

        [HttpPost]
        public ActionResult GroupAddForm([Bind(Prefix = "AspFormAuthorization")] AspFormAuthorization group, string selectedForm, FormCollection frm)
        {

            var listAuth = (from form in selectedForm.Split(' ')
                            where form != ""
                            select new AspFormAuthorization()
                            {
                                FormID = Int32.Parse(form),
                                GroupId = @group.GroupId
                            });
            using (var scope = new TransactionScope())
            {
                var dbAuth = new FormRoleDataService();
                dbAuth.Insert(listAuth);
                scope.Complete();
            }
            return RedirectToAction("Index");
        }

        public ActionResult GroupEditForm(string id)
        {
            var form = new FormRoleDataService().Edit(id);
            return View(form);
        }


        [HttpPost]
        public ActionResult GroupEditForm([Bind(Prefix = "AspFormAuthorization")] AspFormAuthorization group, string selectedForm, FormCollection frm)
        {
            var listAuth = (from form in selectedForm.Split(' ')
                            where form != ""
                            select new AspFormAuthorization()
                            {
                                FormID = Int32.Parse(form),
                                GroupId = @group.GroupId
                            });
            using (var scope = new TransactionScope())
            {
                var dbAuth = new FormRoleDataService();
                dbAuth.Update(listAuth);
                scope.Complete();
            }
            return RedirectToAction("Index");
        }

        public ActionResult GroupFormResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            Counter<ApplicationGroup> grup = new FormRoleDataService().LoadData(requestModel);
            int count = requestModel.Start + 1;
            IEnumerable<string[]> result = from d in grup.ListClass
                                           select new[]
                {
                    Convert.ToString(count++),
                    d.Name,
                    d.Description,
                    Convert.ToString(d.Active),
                    d.Id
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, grup.TotalFilter,
                grup.Total), JsonRequestBehavior.AllowGet);
        }
    }
}