using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccessService.Master;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.Class;
using Services.DataTables;
using ViewModel.Master;

namespace FitnessCenter.Areas.Office.Controllers
{
    public class StatusActionController : FitController
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
            return View("DetailsStatus", mt);
        }

        //[Authorize(Roles="StatusMember_C")]
        public ActionResult CreateStatus()
        {
            var dStatus = new DataServiceStatusMember();
            string currentPeriod = DateTime.Now.ToString("yyyyMM");
            //            var statusMember = dStatus.LoadAllData().FirstOrDefault(m => m.SPeriod == currentPeriod && m.STKet == "Freeze");

            //            if (statusMember == null)
            //            {
            StatusMemberPriceViewModel stat = new StatusMemberPriceViewModel()
            {
                Price = decimal.Zero,
                Period = DateTime.Now.ToString("yyyyMM")
            };

            return View(stat);
            //            }
            //
            //
            //            return View("DetailsStatus", statusMember);
        }

        [HttpPost]
        public ActionResult CreateStatus(StatusMemberPriceViewModel stat)
        {
            if (!ModelState.IsValid)
            {
                return View(stat);
            }
            var dStatus = new DataServiceStatusMember();

            dStatus.Insert(stat.StatusMember);
            dStatus.InsertDetail(new tStatusMemberPrice()
            {
                Period = stat.Period,
                Price = stat.Price,
                StatusMID = stat.StatusMember.StatusMID
            });

            return RedirectToAction("DetailsSM", new { id = stat.StatusMember.StatusMID });
        }

        //        public ActionResult CreateCancel()
        //        {
        //
        //            var dStatus = new DataServiceStatusMember();
        //            string currentPeriod = DateTime.Now.ToString("yyyyMM");
        //            var statusMember = dStatus.LoadAllData().FirstOrDefault(m => m.tStatusMemberPrices.Any(md => md.Period == currentPeriod) && m.STKet == "Cancel");
        //
        //            if (statusMember == null)
        //            {
        //                tStatusMember stat = new tStatusMember()
        //                {
        //                    STKet = "Cancel",
        //                };
        //
        //                return View(stat);
        //            }
        //
        //
        //            return View("DetailsSM", statusMember);
        //        }

        //        [HttpPost]
        //        public ActionResult CreateCancel(tStatusMember stat)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return View(stat);
        //            }
        //            var dStatus = new DataServiceStatusMember();
        //            dStatus.Insert(stat);
        //            return RedirectToAction("DetailsSM", new { id = stat.StatusMID });
        //        }

        public ActionResult EditStatus(int id)
        {
            var stat = new DataServiceStatusMember().GetobjById(id);
            var statLatest = stat.tStatusMemberPrices.OrderByDescending(m => m.Period).First();
            StatusMemberPriceViewModel vm = new StatusMemberPriceViewModel()
            {
                StatusMember = stat,
                Price = statLatest.Price,
                Period = statLatest.Period
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult EditStatus(StatusMemberPriceViewModel stat, FormCollection frm)
        {
            if (!ModelState.IsValid)
            {
                return View(stat);
            }

            var dStatus = new DataServiceStatusMember();
            dStatus.Update(stat.StatusMember);

            dStatus.UpdateDetail(new tStatusMemberPrice()
            {
                Period = stat.Period,
                Price = stat.Price
            });

            return RedirectToAction("DetailsSM", new { id = stat.StatusMember.StatusMID });
        }

        public ActionResult StatusPaymentResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, FormCollection form)
        {
            Counter<tStatusMember> loadStatus = new DataServiceStatusMember().LoadDataPayment(requestModel);

            int count = requestModel.Start + 1;
            IEnumerable<object[]> result = from d in loadStatus.ListClass
                                           select new object[]
                         {
                             Convert.ToString(count++),
                             d.STKet,
                             d.tStatusMemberPrices.OrderByDescending(m => m.Period)?.First().Price.ToString("N2"),
                             d.tStatusMemberPrices.OrderByDescending(m => m.Period)?.First().Period,
                             Convert.ToString(d.StatusMID)
                         };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, loadStatus.TotalFilter,
                loadStatus.Total), JsonRequestBehavior.AllowGet);
        }
    }
}