using System;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.PT;
using DataAccessService.Registrasi;
using Services.DataTables;
using Services.Helpers;
using ViewModel.Accounting;

namespace FitnessCenter.Areas.Accounting.Controllers
{
    public class CheckClosingController : Controller
    {
        // GET: Accounting/DailyClosing
        public ActionResult Index()
        {
            var dMembership = new DataServiceMembership();
            var dPersonalTrainer = new DataServicePersonalTrainer();
            var daily = new ViewModelDailyClosing
            {
                trPersonalTrainers = dPersonalTrainer.LoadAllData().Where(m => m.trMembership.AccountingStatus == null),
                trMemberships = dMembership.LoadAllData().Where(m => m.AccountingStatus == null)
            };

            return View(daily);
        }
        
        [HttpGet]
        public ActionResult MembershipAmount(string membership)
        {
            var dMembership = new DataServiceMembership();
           //            Console.Write(membership);
            var member = string.IsNullOrEmpty(membership) ? null : Array.ConvertAll(membership.Split(','), int.Parse);

            var memberships = member == null ? 0 : (from p in dMembership.LoadAllData()
                where (from q in member
                    select q).Contains(p.trMembershipID)
                select p).Sum(m => m.Total);

            return Json(memberships, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TransferAmount(string membership)
        {
            var dMembership = new DataServiceMembership();
            //            Console.Write(membership);
            var member = string.IsNullOrEmpty(membership) ? null : Array.ConvertAll(membership.Split(','), int.Parse);

            var memberships = member == null ? 0 : (from p in dMembership.LoadAllData()
                where (from q in member
                    select q).Contains(p.trMembershipID)
                select p).Sum(m => m.Total);

            return Json(memberships, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CollectionAmount(string membership)
        {
            var dMembership = new DataServiceMembership();
            //            Console.Write(membership);
            var member = string.IsNullOrEmpty(membership) ? null : Array.ConvertAll(membership.Split(','), int.Parse);

            var memberships = member == null ? 0 : (from p in dMembership.LoadAllData()
                where (from q in member
                    select q).Contains(p.trMembershipID)
                select p).Sum(m => m.Total);

            return Json(memberships, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FreezeAmount(string membership)
        {
            var dMembership = new DataServiceMembership();
            //            Console.Write(membership);
            var member = string.IsNullOrEmpty(membership) ? null : Array.ConvertAll(membership.Split(','), int.Parse);

            var memberships = member == null ? 0 : (from p in dMembership.LoadAllData()
                where (from q in member
                    select q).Contains(p.trMembershipID)
                select p).Sum(m => m.Total);

            return Json(memberships, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult POSAmount(string membership)
        {
            var dMembership = new DataServicePersonalTrainer();
            //            Console.Write(membership);
            var member = string.IsNullOrEmpty(membership) ? null : Array.ConvertAll(membership.Split(','), int.Parse);

            var memberships = member == null ? 0 : (from p in dMembership.LoadAllData()
                where (from q in member
                    select q).Contains(p.trPersonalTrainerID)
                select p).Sum(m => m.trMembership.Total);

            return Json(memberships, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TrainerAmount(string membership)
        {
            var dMembership = new DataServicePersonalTrainer();
            //            Console.Write(membership);
            var member = string.IsNullOrEmpty(membership) ? null : Array.ConvertAll(membership.Split(','), int.Parse);

            var memberships = member == null ? 0 : (from p in dMembership.LoadAllData()
                where (from q in member
                    select q).Contains(p.trPersonalTrainerID)
                select p).Sum(m => m.trMembership.Total);

            return Json(memberships, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ProjectAmount(string membership)
        {
            var dMembership = new DataServiceMembership();
            //            Console.Write(membership);
            var member = string.IsNullOrEmpty(membership) ? null : Array.ConvertAll(membership.Split(','), int.Parse);

            var memberships = member == null ? 0 : (from p in dMembership.LoadAllData()
                where (from q in member
                    select q).Contains(p.trMembershipID)
                select p).Sum(m => m.Total);

            return Json(memberships, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ClosingMembership(int[] membershipId, FormCollection form)
        {
            if (!membershipId.Any())
                return RedirectToAction("Index");
            var dMembership = new DataServiceMembership();
            var members = from p in dMembership.LoadAllData()
                where (from id in membershipId
                    select id).Contains(p.trMembershipID)
                select p;
            using (var ts = new TransactionScope())
            {
                foreach (var member in members)
                {
                    member.AccountingStatus = EnumAcountingStatus.Checked.ToString("F");
                    dMembership.Update(member);
                }
                ts.Complete();
                ViewBag.note = "Closing data membership sukses";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ClosingProject(int[] projectId, FormCollection form)
        {
            if (!projectId.Any())
                return RedirectToAction("Index");
            var dMembership = new DataServiceMembership();
            var members = from p in dMembership.LoadAllData()
                where (from id in projectId
                    select id).Contains(p.trMembershipID)
                select p;
            using (var ts = new TransactionScope())
            {
                foreach (var member in members)
                {
                    member.AccountingStatus = EnumAcountingStatus.Checked.ToString("F");
                    dMembership.Update(member);
                }
                ts.Complete();
                ViewBag.note = "Closing data Project sukses";

                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult ClosingPersonalTrainer(int[] ptId, FormCollection form)
        {
            if (!ptId.Any())
                return RedirectToAction("Index");
            var dMembership = new DataServicePersonalTrainer();
            var members = from p in dMembership.LoadAllData()
                where (from id in ptId
                    select id).Contains(p.trPersonalTrainerID)
                select p;

            using (var ts = new TransactionScope())
            {
                foreach (var member in members)
                {
                    member.trMembership.AccountingStatus = EnumAcountingStatus.Checked.ToString("F");
                    dMembership.UpdatePT(member);
                }
                ts.Complete();
                ViewBag.note = "Closing data Personal Trainer sukses";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ClosingPOS(int[] posId, FormCollection form)
        {
            if (!posId.Any())
                return RedirectToAction("Index");
            var dMembership = new DataServicePersonalTrainer();
            var members = from p in dMembership.LoadAllData()
                where (from id in posId
                    select id).Contains(p.trPersonalTrainerID)
                select p;
            using (var ts = new TransactionScope())
            {
                foreach (var member in members)
                {
                    member.trMembership.AccountingStatus = EnumAcountingStatus.Checked.ToString("F");
                    dMembership.UpdatePT(member);
                }
                ts.Complete();
                ViewBag.note = "Closing data Personal Trainer sukses";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ClosingFreeze(int[] freezeId, FormCollection form)
        {
            if (!freezeId.Any())
                return RedirectToAction("Index");
            var dMembership = new DataServiceMembership();
            var members = from p in dMembership.LoadAllData()
                where (from id in freezeId
                    select id).Contains(p.trMembershipID)
                select p;
            using (var ts = new TransactionScope())
            {
                foreach (var member in members)
                {
                    member.AccountingStatus = EnumAcountingStatus.Checked.ToString("F");
                    dMembership.Update(member);
                }
                ts.Complete();
                ViewBag.note = "Closing data Freeze sukses";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ClosingCollection(int[] collectionId, FormCollection form)
        {
            if (!collectionId.Any())
                return RedirectToAction("Index");
            var dMembership = new DataServiceMembership();
            var members = from p in dMembership.LoadAllData()
                where (from id in collectionId
                    select id).Contains(p.trMembershipID)
                select p;
            using (var ts = new TransactionScope())
            {
                foreach (var member in members)
                {
                    member.AccountingStatus = EnumAcountingStatus.Checked.ToString("F");
                    dMembership.Update(member);
                }
                ts.Complete();
                ViewBag.note = "Closing data Collection sukses";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ClosingTransfer(int[] transferId, FormCollection form)
        {
            if (!transferId.Any())
                return RedirectToAction("Index");
            var dMembership = new DataServiceMembership();
            var members = from p in dMembership.LoadAllData()
                where (from id in transferId
                    select id).Contains(p.trMembershipID)
                select p;
            using (var ts = new TransactionScope())
            {
                foreach (var member in members)
                {
                    member.AccountingStatus = EnumAcountingStatus.Checked.ToString("F");
                    dMembership.Update(member);
                }
                ts.Complete();
                ViewBag.note = "Closing data Transfer sukses";

                return RedirectToAction("Index");
            }
        }


        public ActionResult MembershipResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            decimal? amount = 0;
            var membership = new DataServiceMembership().LoadDataClosing(requestModel, EnumDaily.Membership, EnumAcountingStatus.Closed, out amount);
            var count = requestModel.Start + 1;
            var result = from d in membership.ListClass
                select new[]
                {
                    Convert.ToString(count++),
                    d.trMembershipID.ToString(),
                    d.tMember.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    (d.Subtotal ?? 0).ToString("N2"),
                    (d.Admin ?? 0).ToString("N2"),
                    (d.Disc ?? 0).ToString("N2"),
                    (d.Total ?? 0).ToString("N2")
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, membership.TotalFilter,
                membership.Total, amount?.ToString("N2")), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProjectResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            decimal? amount = 0;
            var membership = new DataServiceMembership().LoadDataClosing(requestModel, EnumDaily.Membership, EnumAcountingStatus.Closed, out amount);
            var count = requestModel.Start + 1;
            var result = from d in membership.ListClass
                select new[]
                {
                    Convert.ToString(count++),
                    d.trMembershipID.ToString(),
                    d.tMember.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    (d.Subtotal ?? 0).ToString("N2"),
                    (d.Admin ?? 0).ToString("N2"),
                    (d.Disc ?? 0).ToString("N2"),
                    (d.Total ?? 0).ToString("N2")
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, membership.TotalFilter,
                membership.Total, amount?.ToString("N2")), JsonRequestBehavior.AllowGet);
        }


        public ActionResult TrainerResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {

            decimal? amount = 0;
            var membership = new DataServicePersonalTrainer().LoadDataClosing(requestModel, EnumDaily.PersonalTrainer, EnumAcountingStatus.Closed, out amount);
            var count = requestModel.Start + 1;
            var result = from d in membership.ListClass
                select new[]
                {
                    Convert.ToString(count++),
                    d.trPersonalTrainerID.ToString(),
                    d.trMembership.tMember.MemberNO,
                    d.trMembership.tMember.tPerson.PNama,
                    d.trMembership.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    d.trMembership.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
                    d.tUserBackOffice?.tPerson.PNama,
                    (d.trMembership.Subtotal)?.ToString("N2"),
                    (d.trMembership.Disc)?.ToString("N2"),
                    (d.trMembership.Total)?.ToString("N2"),
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, membership.TotalFilter,
                membership.Total, amount?.ToString("N2")), JsonRequestBehavior.AllowGet);

            //            membership = new DataServicePersonalTrainer().LoadDataClosing(requestModel, EnumDaily.Membership);

        }

        public ActionResult POSResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var membership = new DataServicePersonalTrainer().LoadDataClosing(requestModel, EnumDaily.PersonalTrainer);
            var count = requestModel.Start + 1;
            var result = from d in membership.ListClass
                select new[]
                {
                    Convert.ToString(count++),
                    d.trPersonalTrainerID.ToString(),
                    d.trMembership.tMember.MemberNO,
                    d.trMembership.tMember.tPerson.PNama,
                    d.trMembership.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    d.trMembership.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
                    d.tUserBackOffice?.tPerson.PNama,
                    (d.trMembership.Subtotal)?.ToString("N2"),
                    (d.trMembership.Disc)?.ToString("N2"),
                    (d.trMembership.Total)?.ToString("N2"),
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, membership.TotalFilter,
                membership.Total), JsonRequestBehavior.AllowGet);
        }


        public ActionResult CollectionResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            decimal? amount = 0;
            var membership = new DataServiceMembership().LoadDataClosing(requestModel, EnumDaily.Membership, EnumAcountingStatus.Closed, out amount);
            var count = requestModel.Start + 1;
            var result = from d in membership.ListClass
                select new[]
                {
                    Convert.ToString(count++),
                    d.trMembershipID.ToString(),
                    d.tMember.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
                    (d.Subtotal ?? 0).ToString("N2"),
                    (d.Admin ?? 0).ToString("N2"),
                    (d.Disc ?? 0).ToString("N2"),
                    (d.Total ?? 0).ToString("N2")
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, membership.TotalFilter,
                membership.Total, amount?.ToString("N2")), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FreezeResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            decimal? amount = 0;
            var membership = new DataServiceMembership().LoadDataClosing(requestModel, EnumDaily.Membership, EnumAcountingStatus.Closed, out amount);
            var count = requestModel.Start + 1;
            var result = from d in membership.ListClass
                select new[]
                {
                    Convert.ToString(count++),
                    d.trMembershipID.ToString(),
                    d.tMember.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
                    (d.Subtotal ?? 0).ToString("N2"),
                    (d.Admin ?? 0).ToString("N2"),
                    (d.Disc ?? 0).ToString("N2"),
                    (d.Total ?? 0).ToString("N2")
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, membership.TotalFilter,
                membership.Total, amount?.ToString("N2")), JsonRequestBehavior.AllowGet);
        }


        public ActionResult TransferResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            decimal? amount = 0;
            var membership = new DataServiceMembership().LoadDataClosing(requestModel, EnumDaily.Membership, EnumAcountingStatus.Closed, out amount);
            var count = requestModel.Start + 1;
            var result = from d in membership.ListClass
                select new[]
                {
                    Convert.ToString(count++),
                    d.trMembershipID.ToString(),
                    d.tMember.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    d.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    (d.Subtotal ?? 0).ToString("N2"),
                    (d.Admin ?? 0).ToString("N2"),
                    (d.Disc ?? 0).ToString("N2"),
                    (d.Total ?? 0).ToString("N2")
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, membership.TotalFilter,
                membership.Total, amount?.ToString("N2")), JsonRequestBehavior.AllowGet);
        }
    }
}
