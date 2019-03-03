using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using FitnessCenter.Models.Crystal_Report;
using PagedList;
using Services.Class;
using Services.DataTables;
using Services.Helpers;
using ViewModel.Membership.FreezeCancel;
using ViewModel.Membership.Registrasi;

namespace FitnessCenter.Areas.Membership.Controllers
{
    public class MemberManagementController : FitController
    {
        private IServiceCardStatus _card;

        public MemberManagementController(ServiceCardStatus card)
        {
            _card = card;
        }


        // GET: Registrasi/Membership
        // GET: Student
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var dMember = new DataServiceMembership();

            var membership = from s in dMember.LoadAllData()
                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                membership = membership.Where(s => s.tMember.tPerson.PNama.Contains(searchString)
                                                   || s.tMember.tPerson.PIdentitas.Contains(searchString)
                                                   || s.tMember.MemberNO.Contains(searchString)
                                                   || s.tMember.tPerson.PAlamat.Contains(searchString));
            }
            membership = membership.OrderByDescending(s => s.tMember.MemberNO).ThenBy(m => m.tMember.tPerson.PNama);

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            var resultajax = (ActionResult)PartialView("PartialView/MemberList", membership.ToPagedList(pageNumber, pageSize));

            return Request.IsAjaxRequest()
                ? resultajax
                : View(membership.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult MemberProcess(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var dMembership = new DataServiceMembership();
            var dStatus = new DataServiceStatusMember();
            var member = dMembership.GetobjByMemberNo(id);

            if (member.MSTglSelesai < DateTime.Now)
                ViewBag.Note = "Member is expired";

            if (member.TotalMonth == 1)
                ViewBag.Note = "<br />This membership only 1 Month";

            if (!string.IsNullOrWhiteSpace(ViewBag.Note))
            {
                return Json(new { message = ViewBag.Note }, JsonRequestBehavior.AllowGet);
            }

            var status = dStatus.LoadAllData();
            var sPriceFreeze = status.First(m => m.STKet == "Freeze").tStatusMemberPrices.OrderByDescending(m => m.Period).First().Price;
            var spriceCancel = status.First(m => m.STKet == "Transfer").tStatusMemberPrices.OrderByDescending(m => m.Period).First().Price;

            member.tMember.MemberNO = "";
            var type = dMembership.GetTypeMembership(member.trMembershipID);

            var memberManagement = new MemberManagementModel
            {
                Membership = member,
                StartDate = member.MSTglMulai.ToString("dd/MM/yyyy"),
                EndDate = member.MSTglSelesai.ToString("dd/MM/yyyy"),
                FreezePrice = sPriceFreeze,
                TransferPrice = spriceCancel,
                TotalMonthFreeze = 1,
                Note = "",
                TotalFreezePrice = (sPriceFreeze),
                DateTimeNow = DateTime.Now.ToString("dd/MM/yyyy"),
                CurrentMemberType = type.MemberType,
                MasterPayment = new ViewModelMasterPayment()
                {
                    MSTglMulai = DateTime.Now
                }
            };

            //if (!CheckdatestatusAvaliable(member.MSTglSelesai))
            memberManagement.AvaliableMenu.Add("Freeze");

            //if (CheckdatestatusAvaliable(member.MSTglMulai))
            memberManagement.AvaliableMenu.Add("Upgrade");

            //if (CountDifferentMonthFromNow(member.MSTglMulai) >= 6 &&
            //(CheckdatestatusAvaliable(member.MSTglSelesai)))
            memberManagement.AvaliableMenu.Add("Transfer");

            return View(memberManagement);
        }

        [HttpPost]
        public ActionResult MemberProcess(MemberManagementModel form)
        {
            var dMembership = new DataServiceMembership();
            var dStatus = new DataServiceStatusMember();
            var price = form.StatusAction == "Freeze" ? form.FreezePrice : form.TransferPrice;

            var status = 0;

            if (form.StatusAction == "Freeze")
            {
                status = StatusAction.Freeze.Id;
            }
            if (form.StatusAction == "Upgrade")
            {
                status = StatusAction.Upgrade.Id;
            }
            if (form.StatusAction == "Transfer")
            {
                status = StatusAction.Transfer.Id;
            }

            var membership = new trMembership
            {
                Admin = 0,
                MemberID = form.Membership.MemberID,
                PersonBOIDADM = User.Person.PersonID,
                PersonBOIDSales = form.Membership.PersonBOIDSales,
                CardStatus = _card.FinishedCard().CardStatusID,
                CountMember = 0,
                Disc = 0,
                GenBayar = 0,
                ParentID = form.Membership.trMembershipID,
                LocationID = User.ActiveLocation,
                Note = form.Note,
                MSTglMulai = DateTime.Parse(form.StartDate),
                MSTglSelesai = DateTime.Parse(form.EndDate),
                Total = price,
                Subtotal = price,
                StatusMID = status,
                seq = 1,
                AgreementID = form.Membership.AgreementID,
                Prorate = 0,
                TotalMonth = form.TotalMonthFreeze
            };


            var newMembership = new trMembership
            {
                Admin = 0,
                MemberID = form.Membership.MemberID,
                PersonBOIDADM = User.Person.PersonID,
                PersonBOIDSales = form.Membership.tUserBackOffice_PersonBOIDSales.PersonBOID,
                CardStatus = 0,
                CountMember = 0,
                Disc = 0,
                GenBayar = 0,
                ParentID = form.Membership.trMembershipID,
                LocationID = User.ActiveLocation,
                Note = form.Note,
                MSTglMulai = DateTime.Parse(form.StartDate),
                MSTglSelesai = DateTime.Parse(form.EndDate),
                Total = price,
                Subtotal = price,
                StatusMID = status,
                seq = 1,
                AgreementID = form.Membership.AgreementID,
                Prorate = 0,
                TotalMonth = form.TotalMonthFreeze
            };



            using (var ts = new TransactionScope())
            {
                dMembership.Insert(membership);

                //jika type sama dengan freeze ubah tanggal selesai  membership
                trMembership firstMembership;
                if (form.StatusAction == "Freeze")
                {
                    firstMembership =
                        dMembership.LoadAllData().Single(m => m.trMembershipID == form.Membership.trMembershipID);
                    var totalMonth =
                        dMembership.LoadAllData()
                            .Where(m => m.ParentID == form.Membership.trMembershipID)
                            .Sum(m => m.TotalMonth);
                    firstMembership.MSTglSelesai =
                        firstMembership.MSTglMulai.AddMonths(firstMembership.TotalMonth).AddMonths(totalMonth);
                    dMembership.Update(firstMembership);
                }
                else if (form.StatusAction == "Cancel")
                {
                    firstMembership = dMembership.LoadAllData().OrderByDescending(m => m.trMembershipID).First();
                    firstMembership.MSTglSelesai = firstMembership.MSTglMulai;
                    firstMembership.StatusMID = status;
                    dMembership.Update(firstMembership);
                }
                ts.Complete();
            }
            return RedirectToAction("Index");
        }

        public ActionResult MemberEdit(int id)
        {
            var dMember = new DataServiceMembership();
            var membership = dMember.LoadAllData().Single(m => m.trMembershipID == id);
            var freezeCancel = new MemberManagementModel
            {
                Membership = membership,
                StartDate = membership.MSTglMulai.ToString("dd/MM/yyyy"),
                EndDate = membership.MSTglSelesai.ToString("dd/MM/yyyy"),
                FreezePrice = (membership.Total ?? 0),
                TransferPrice = (membership.Total ?? 0),
                TotalMonthFreeze = membership.TotalMonth,
                Note = membership.Note,
                DateTimeNow = membership.MSTglMulai.ToString("dd/MM/yyyy"),
                StatusAction = membership.tStatusMember.STKet
            };
            return View(freezeCancel);
        }

        [HttpPost]
        public ActionResult FreezeAndCancelEdit(MemberManagementModel model)
        {
            var dMember = new DataServiceMembership();
            var membership = dMember.LoadAllData().Single(m => m.trMembershipID == model.Membership.trMembershipID);
            membership.Note = model.Note;

            dMember.Update(membership);
            return RedirectToAction("Index");
        }

        public ActionResult PrintCancelFreezeResult(int id)
        {
            var dMembership = new DataServiceMembership();
            var m = dMembership.LoadAllData().Single(mem => mem.trMembershipID == id);

            if (m.tStatusMember.STKet == EnumStatusAction.Cancel.ToString("F"))
            {
                string reportPath =
             Server.MapPath(@"~\bin\Areas\Membership\Report\PrintOutCancel.rpt");
                var reportSource = new PrintoutCancel()
                {

                    Alamat = m.tMember.tPerson.PAlamat,
                    BOID = m.tUserBackOffice_PersonBOIDSales.BOIDNO,
                    Barcode = m.tMember.MemberNO,
                    Email = m.tMember.tPerson.PEmail,
                    HP = m.tMember.tPerson.PHP1,
                    HP2 = m.tMember.tPerson.PHP2,
                    Nama = m.tMember.tPerson.PNama,
                    Telp = m.tMember.tPerson.PTelp,
                    identitas = m.tMember.tPerson.PIdentitas,
                    lokasiKlub = m.tLocFitnessCenter.LAlamat,
                    namaStaf = m.tUserBackOffice_PersonBOIDADM.tPerson.PNama,
                    tglHariIni = DateTime.Now,
                    tglBatal = m.MSTglMulai
                };
                return new CrystalReportPdfResult(reportPath, new[] { reportSource }, null);
            }
            else if (m.tStatusMember.STKet == EnumStatusAction.Freeze.ToString("F"))
            {
                string reportPath =
               Server.MapPath(@"~\bin\Areas\Membership\Report\PrintOutFreeze.rpt");
                var reportSource = new PrintoutFreeze()
                {

                    Alamat = m.tMember.tPerson.PAlamat,
                    Barcode = "",
                    Email = m.tMember.tPerson.PEmail,
                    HP = m.tMember.tPerson.PHP1,
                    HP2 = m.tMember.tPerson.PHP2,
                    Nama = m.tMember.tPerson.PNama,
                    Telp = m.tMember.tPerson.PTelp,
                    identitas = m.tMember.tPerson.PIdentitas,
                    lokasiKlub = m.tLocFitnessCenter.LAlamat,
                    tglHariIni = DateTime.Now,
                    namaStaf = m.tUserBackOffice_PersonBOIDSales.tPerson.PNama,
                    lamaBeku = m.TotalMonth,
                    tglMulaiBeku = m.MSTglMulai,
                    tglAkhirBeku = m.MSTglSelesai,
                    BOID = m.tUserBackOffice_PersonBOIDADM.BOIDNO
                };
                return new CrystalReportPdfResult(reportPath, new[] { reportSource }, null);

            }
            else
            {
                return HttpNotFound();
            }

        }





        public ActionResult FreezeMember(MemberManagementModel form)
        {
            var dMembership = new DataServiceMembership();
            var dStatus = new DataServiceStatusMember();
            var price = form.StatusAction == "Freeze" ? form.FreezePrice : form.TransferPrice;

            var status = dStatus.GetStatusActionId(form.StatusAction == "Freeze"
                ? EnumStatusAction.Freeze.ToString("F")
                : EnumStatusAction.Cancel.ToString("F"));


            var membership = new trMembership
            {
                Admin = 0,
                MemberID = form.Membership.MemberID,
                PersonBOIDADM = User.Person.PersonID,
                PersonBOIDSales = form.Membership.PersonBOIDSales,
                CardStatus = _card.FinishedCard().CardStatusID,
                CountMember = 0,
                Disc = 0,
                GenBayar = 0,
                ParentID = form.Membership.trMembershipID,
                LocationID = User.ActiveLocation,
                Note = "Freeze Membership - " + form.Note,
                MSTglMulai = DateTime.Parse(form.StartDate),
                MSTglSelesai = DateTime.Parse(form.EndDate),
                Total = form.TotalFreezePrice,
                Subtotal = price,
                StatusMID = status,
                seq = 1,
                AgreementID = form.Membership.AgreementID,
                Prorate = 0,
                TotalMonth = form.TotalMonthFreeze
            };

            using (var ts = new TransactionScope())
            {
                dMembership.Insert(membership);

                ts.Complete();
            }
            return RedirectToAction("Index");
        }

        public ActionResult TransferMember(MemberManagementModel member)
        {

            var dMembership = new DataServiceMembership();
            var dStatus = new DataServiceStatusMember();
            var dPerson = new DataServicePerson();
            var currentMembership = dMembership.GetobjByID(member.Membership.trMembershipID);
            var statusCancel = dStatus.GetStatusActionId(EnumStatusAction.Cancel.ToString("F"));
            var statusMember = dStatus.GetStatusActionId(EnumStatusMember.Membership.ToString("F"));


            var membership = new trMembership
            {
                Admin = 0,
                MemberID = member.Membership.MemberID,
                PersonBOIDADM = User.Person.PersonID,
                PersonBOIDSales = member.Membership.PersonBOIDSales,
                CardStatus = _card.FinishedCard().CardStatusID,
                CountMember = 0,
                Disc = 0,
                GenBayar = 0,
                ParentID = member.Membership.trMembershipID,
                LocationID = User.ActiveLocation,
                Note = "Transfer Membership - " + member.Note,
                MSTglMulai = DateTime.Now,
                MSTglSelesai = DateTime.Now,
                Total = member.TransferPrice,
                Subtotal = member.TransferPrice,
                StatusMID = statusCancel,
                seq = 1,
                AgreementID = member.Membership.AgreementID,
                Prorate = 0,
                TotalMonth = member.TotalMonthFreeze
            };

            using (var ts = new TransactionScope())
            {

                dMembership.InsertWithoutValidation(membership);

                int personId;
                if (member.Membership.tMember.MemberNO != "")
                {
                    personId = member.Membership.tMember.tPerson.PersonID;
                }
                else
                {
                    var newPersonmember = new tPerson()
                    {
                        PNama = member.Person.PNama,
                        PAlamat = member.Person.PAlamat,
                        PEmail = member.Person.PEmail,
                        PGender = member.Person.PAlamat,
                        PHP1 = member.Person.PHP1,
                        PHP2 = member.Person.PHP2,
                        PIdentitas = member.Person.PIdentitas,
                        PKecamatan = member.Person.PKecamatan,
                        PKelurahan = member.Person.PKelurahan,
                        PKota = member.Person.PKota,
                        PPinBB = member.Person.PPinBB,
                        PPropinsi = member.Person.PPropinsi,
                        PRT = member.Person.PRT,
                        PRW = member.Person.PRW,
                        PTelp = member.Person.PTelp,
                        PTempLahir = member.Person.PTempLahir,
                        PTglLahir = member.Person.PTglLahir,
                    };

                    dPerson.Insert(newPersonmember);
                    personId = newPersonmember.PersonID;
                }

                Debug.WriteLine(currentMembership.MSTglSelesai);

                var newMemberT = new trMembership
                {
                    Admin = 0,
                    ParentID = membership.trMembershipID,
                    PersonBOIDADM = User.Person.PersonID,
                    PersonBOIDSales = member.Membership.PersonBOIDSales,
                    CardStatus = _card.FinishedCard().CardStatusID,
                    CountMember = 0,
                    Disc = 0,
                    GenBayar = 0,
                    LocationID = User.ActiveLocation,
                    Note = "TransferMembership - " + member.Note,
                    MSTglMulai = DateTime.Now,
                    MSTglSelesai = (currentMembership.MSTglSelesai),
                    Total = member.TransferPrice,
                    Subtotal = member.TransferPrice,
                    StatusMID = statusMember,
                    seq = 1,
                    AgreementID = member.Membership.AgreementID,
                    Prorate = 0,
                    TotalMonth = ((currentMembership.MSTglSelesai.Year - DateTime.Now.Year) * 12) + currentMembership.MSTglSelesai.Month - DateTime.Now.Month, //DateTime.Compare(DateTime.Now, currentMembership.MSTglSelesai),
                    ActivationCode = Guid.NewGuid().GetHashCode().ToString("x"),
                    tMember = new tMember()
                    {
                        PersonID = personId,
                        MemberTypeID = member.Membership.tMember.MemberTypeID
                    }
                };

                dMembership.Insert(newMemberT);

                currentMembership = dMembership.GetobjByID(member.Membership.trMembershipID);
                currentMembership.MSTglSelesai = DateTime.Now;
                dMembership.Update(currentMembership);

                ts.Complete();
            }

            return RedirectToAction("Index");
        }
        public ActionResult UpgradeMember(MemberManagementModel member)
        {

            var dMembership = new DataServiceMembership();
            var dStatus = new DataServiceStatusMember();
            var dpaymentWith = new DataServicePaymentWith();
            var dPaymentMember = new DataServicePaymentMember();
            var currentMembership = dMembership.GetobjByID(member.Membership.trMembershipID);
            currentMembership.StatusMID = 0;

            dMembership.Update(currentMembership);


            var transactionMember = new trMembership
            {
                Admin = 0,
                AgreementID = currentMembership.AgreementID,//agreemenId,
                PersonBOIDADM = User.Person.PersonID,
                CountMember = currentMembership.CountMember,
                MemberID = currentMembership.MemberID,
                CardStatus = _card.FinishedCard().CardStatusID,
                GenBayar = 0,
                LocationID = 0,
                MSTglMulai = (currentMembership.MSTglMulai),
                MSTglSelesai = (currentMembership.MSTglSelesai),
                Note = "Upgrade Membership - " + currentMembership.AgreementID,
                StatusMID = dStatus.GetStatusId(EnumStatusMember.Membership),
                seq = 1,
                Prorate = member.MemberType.Prorate,
                PersonBOIDSales = currentMembership.PersonBOIDSales,
                TotalMonth = member.MasterPayment.TotalMonth,
                Total = member.MasterPayment.Total,
                ActivationCode = Guid.NewGuid().GetHashCode().ToString("x"),
                ParentID = currentMembership.trMembershipID
            };


            decimal percent = 1 - ((member.MemberType.DiscountPct.HasValue ? member.MemberType.DiscountPct.Value : 0) / 100);


            transactionMember.Subtotal = transactionMember.Total / percent;
            //(1 - (memberCreate.MemberType.DiscountPct / 100));
            //throw new Exception();
            transactionMember.Disc = transactionMember.Subtotal - transactionMember.Total;
            //                    throw new Exception();


            transactionMember.LocationID = User.ActiveLocation;

            using (var ts = new TransactionScope())
            {
                dMembership.Insert(transactionMember);

                foreach (var payWith in member.PaymentsWith.Select(payment => new trPaymentWith
                {
                    BankID = payment.BankID,
                    PaymentTypeID = payment.PaymentTypeID,
                    NoKartu = payment.NoKartu,
                    TraceCode = payment.TraceCode,
                    ApprCode = payment.ApprCode,
                    ValidUntil = payment.ValidUntil,
                    payAmount = payment.payAmount
                }))
                {
                    dpaymentWith.Insert(payWith);
                    //throw new Exception();
                    var payMember = dPaymentMember.GetPaymentByMembership(transactionMember.trMembershipID).First();
                    var pay = new strPayment
                    {
                        trPaymentID = payMember.trPaymentID,
                        PaymentWithID = payWith.PaymentWithID
                    };
                    dPaymentMember.InsertstrPayment(pay);
                }

                ts.Complete();

            }
            return RedirectToAction("Index");
        }


        public ActionResult ManagementResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var cm = new DataServiceMembership().LoadDataRenewal(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in cm.ListClass
                         select new[]
                         {
                    Convert.ToString(count++),
                    d.tMember.tPerson.PNama,
                    d.tMember.tPerson.PGender == "M" ? "Male" : "Female",
                    d.tMember.tPerson.PAlamat,
                    d.tMember.tPerson.PTelp,
                    d.tMember.tPerson.PHP1,
                    d.tStatusMember.STKet + " " + d.TotalMonth + " Month",
                    d.Note,
                    Convert.ToString(d.trMembershipID)
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cm.TotalFilter,
                cm.Total), JsonRequestBehavior.AllowGet);
        }
    }
}