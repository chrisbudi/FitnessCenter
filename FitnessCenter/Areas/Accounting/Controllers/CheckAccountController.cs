using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessService.PT;
using DataAccessService.Registrasi;
using Services.DataTables;
using Services.Helpers;

namespace FitnessCenter.Areas.Accounting.Controllers
{
    public class CheckAccountController : Controller
    {
        // GET: Accounting/Accounting
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {

            decimal? amount = 0;
            var membership = new DataServiceMembership().LoadDataAccounting(requestModel, EnumDaily.PersonalTrainer, EnumAcountingStatus.Closed, out amount);
            var count = requestModel.Start + 1;
            var result = from d in membership.ListClass
                         select new[]
                         {
                    Convert.ToString(count++),
//                    d.trPersonalTrainerID.ToString(),
//                    d.trMembership.tMember.MemberNO,
//                    d.trMembership.tMember.tPerson.PNama,
//                    d.trMembership.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
//                    d.trMembership.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
//                    d.tUserBackOffice?.tPerson.PNama,
//                    (d.trMembership.Subtotal)?.ToString("N2"),
//                    (d.trMembership.Disc)?.ToString("N2"),
//                    (d.trMembership.Total)?.ToString("N2"),
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, membership.TotalFilter,
                membership.Total, amount?.ToString("N2")), JsonRequestBehavior.AllowGet);

            //            membership = new DataServicePersonalTrainer().LoadDataClosing(requestModel, EnumDaily.Membership);

        }

        public ActionResult CheckPayment()
        {
            throw new NotImplementedException();
        }
    }
}