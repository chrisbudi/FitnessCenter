using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using FitnessCenter.Models.Crystal_Report;
using Services.Class;
using Services.DataTables;
using Services.Helpers;
using ViewModel.Membership.Registrasi;

namespace FitnessCenter.Areas.Membership.Controllers
{
    public class MemberRenewalController : FitController
    {
        private readonly IServiceCardStatus _cardstatusManager;

        public MemberRenewalController(ServiceCardStatus cardstatusManager)
        {
            this._cardstatusManager = cardstatusManager;
        }

        // GET: Registrasi/Membership
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddDataPaymentDetail(decimal amount, string paymentType, string memberId, string membershipId)
        {
            var pay = new DataServicePaymentType();
            var paymentdata = pay.LoadAllData().Single(m => m.NamaType == paymentType);
            var model = new trPaymentWith()
            {
                payAmount = amount,
                PaymentTypeID = paymentdata.PaymentTypeID,
                tPaymentType = paymentdata,
                paidAmount = amount
            };
            ViewBag.memberId = memberId;
            ViewBag.membershipId = membershipId;
            return PartialView("Editor/PaymentDetailEntityEditor", model);
        }


        /*untuk sementara di gunakan metode orang pertama adalah entity yang di select di index
         *seharusnya ketika di pilih langsung keluar data seperti ketika membuat transaksi membership
         */
        [HttpGet]
        public ActionResult AddDataPerson(int seq, int memberId)
        {
            //            var dMember = new DataServiceMembership();
            //                var membership = dMember.LoadAllData().Single(m => m.trMembershipID == memberId);
            //            if (membership.First().ParentID != null)
            //            {
            //                var parentId = membership.First().ParentID;
            //                membership = dMember.LoadAllData().Where(m => m.trMembershipID == parentId || m.ParentID == parentId);
            //            }

            var model = new trMembership()
            {
                seq = seq,
            };

            if (seq == 1)
            {
                var dMember = new DataServiceMembership();
                model = dMember.LoadAllData().Single(m => m.trMembershipID == memberId);
            }
            else
            {
                model.tMember = new tMember()
                {
                    tPerson = new tPerson()
                    {
                        PGender = "M"
                    }
                };
            }

            return PartialView("Editor/PersonEntityEditor", model);
        }

        [HttpPost]
        public ActionResult AddDataPersonAddress(string memberId, string membershipId, string personnama, int transId, int seq, int lastRow, string memberNo)
        {
            var dMember = new DataServiceMembership();
            var dstatusMember = new DataServiceStatusMember();
            var model = new trMembership();

            if (memberNo != "")
            {
                var status = dstatusMember.GetStatusId(EnumStatusMember.Membership);
                model = dMember.LoadAllData().Where(m => m.tMember.MemberNO == memberNo && m.StatusMID == status).OrderByDescending(m => m.trMembershipID).First();
            }
            else
            {
                model.tMember = new tMember()
                {
                    tPerson = new tPerson()
                    {
                        PNama = personnama
                    }
                };
            }

            ViewBag.memberId = memberId;
            ViewBag.membershipId = membershipId;

            return PartialView("Editor/PersonAddressEntityEditor", model);
        }


        [HttpPost]
        public ActionResult AddDataPersonPayment(string memberId, string membershipId, string personnama)
        {
            var model = new trMembership()
            {
                tMember = new tMember()
                {
                    tPerson = new tPerson()
                    {
                        PNama = personnama
                    }
                }
            };
            ViewBag.memberId = memberId;
            ViewBag.membershipId = membershipId;

            return PartialView("Editor/PaymentEntityEditor", model);
        }

        public ActionResult GetMemberObj(int id)
        {
            var dPerson = new DataServicePerson();
            var dMember = new DataServiceMember();

            var member = dMember.GetobjByPersonId(id);
            var person = dPerson.GetobjById(member.PersonID);

            var pvm = new
            {
                person.PNama,
                PGender = person.PGender == "M" ? "Male" : "Female",
                person.PIdentitas,
                tglLahirString = person.PTglLahir.Value.ToString("dd-MM-yyyy"),
                person.PHP1,
                person.PersonID,
                person.PPinBB,
                member.MemberID
            };

            return new Jsonp
            {
                Data = pvm,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult RenewMembership(int id)
        {
            ViewBag.TransId = id;
            var dMembership = new DataServiceMembership();
            var membership = dMembership.Create(User.ActiveLocation, id);
            membership.Membership.Admin = 0;

            return View(membership);
        }

        #region RenewMembership
        [HttpPost]
        public ActionResult RenewMembership(ViewModelMembershipCreate member, FormCollection form)
        {
            var dMembership = new DataServiceMembership();
            var dPaymentMember = new DataServicePaymentMember();
            var dpaymentWith = new DataServicePaymentWith();
            var dStatusMember = new DataServiceStatusMember();
            var dMemberType = new DataServiceMemberType();
            using (var ts = new TransactionScope())
            {
                int? parrentId = null;
                var cardId = _cardstatusManager.Get("New").CardStatusID;
                foreach (var trMembership in member.Memberships)
                {
                    //                    if (trMembership.seq == 1)
                    //                    {
                    //                        trMembership.tMember.tPerson.PKelurahan = member.FPerson.PKelurahan;
                    //                        trMembership.tMember.tPerson.PKecamatan = member.FPerson.PKecamatan;
                    //                        trMembership.tMember.tPerson.PPropinsi = member.FPerson.PPropinsi;
                    //                        trMembership.tMember.tPerson.PKota = member.FPerson.PKota;
                    //                    }

                    trMembership.CardStatus = cardId;
                    trMembership.ActivationCode = Guid.NewGuid().GetHashCode().ToString("x");
                    trMembership.PersonBOIDSales = member.Membership.PersonBOIDSales;
                    trMembership.PersonBOIDADM = User.Person.PersonID;
                    //                    trMembership.AgreementID = dMembership.GenerateMembershipAggrementId(User.ActiveLocation);
                    trMembership.StatusMID = dStatusMember.GetStatusId(EnumStatusMember.CalonMember);
                    trMembership.MSTglSelesai = trMembership.MSTglMulai.AddMonths(trMembership.TotalMonth);
                    trMembership.LocationID = User.ActiveLocation;
                    trMembership.ParentID = parrentId;
                    trMembership.tMember.MemberTypeID = member.PaymentMember.MemberTypeID;
                    trMembership.tMember.tPerson = null;
                    trMembership.tMember.MemberNO = null;

                    dMembership.InsertWithoutValidation(trMembership);

                    if (trMembership.seq == 1)
                        parrentId = trMembership.trMembershipID;

                    if (trMembership.CountMember == 1 && member.PaymentMember.tMemberType.IsPaket == false)
                    {
                        var datePaymentMember = DateTime.Now.Date;
                        var secondpaymentdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        var firstpaymentdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);

                        if (datePaymentMember.Ticks >= firstpaymentdate.Ticks &&
                            datePaymentMember.Ticks <= secondpaymentdate.Ticks)
                        {
                            if (DateTime.Now.Month == 2)
                            {

                                datePaymentMember = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);

                            }
                            else
                            {
                                datePaymentMember = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);
                            }
                        }

                        else
                            datePaymentMember = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15);

                        for (int i = 0; i <= 11; i++)
                        {
                            var payMember = new strPaymentMember()
                            {
                                Note = "",
                                pembayaranke = i + 1,
                                statusBayar = i == 0 || i == 11,
                                trMembershipID = trMembership.trMembershipID,
                                MemberTypeID = member.PaymentMember.MemberTypeID,
                                Tanggal = (datePaymentMember.AddMonths(i + 1)),
                                MembershipDTLID = i == 0 ? trMembership.trMembershipID : new int()
                            };
                            if (payMember.MembershipDTLID == 0)
                            {
                                payMember.MembershipDTLID = null;
                            }
                            dPaymentMember.Insert(payMember);
                            //                            throw new Exception();
                        }
                    }
                    else
                    {
                        var strPaymentMember = new strPaymentMember()
                        {
                            Note = "",
                            pembayaranke = 0,
                            statusBayar = true,
                            trMembershipID = trMembership.trMembershipID,
                            MemberTypeID = member.PaymentMember.MemberTypeID,
                            Tanggal = DateTime.Now,
                            MembershipDTLID = trMembership.trMembershipID
                        };
                        dPaymentMember.Insert(strPaymentMember);
                    }
                    var firstPayment = 0;
                    foreach (var payWith in member.PaymentsWith.Select(payment => new trPaymentWith
                    {
                        BankID = payment.BankID,
                        PaymentTypeID = payment.PaymentTypeID,
                        NoKartu = payment.NoKartu,
                        TraceCode = payment.TraceCode,
                        ApprCode = payment.ApprCode,
                        ValidUntil = payment.ValidUntil,
                        payAmount = payment.payAmount,
                        paidAmount = payment.paidAmount,
                        MBRAmount = payment.MBRAmount,
                        Pemegang = payment.Pemegang,
                        EDCID = payment.EDCID,
                        I = payment.I,
                        PaymentWithID = payment.PaymentWithID
                    }))
                    {
                        var strpayMember = dPaymentMember.GetPaymentByMembership(trMembership.trMembershipID).First();
                        var tmemberType = dMemberType.GetobjById(strpayMember.MemberTypeID);
                        if (tmemberType.IsPaket)
                        {

                            dpaymentWith.Insert(payWith);

                            var pay = new strPayment
                            {
                                trPaymentID = strpayMember.trPaymentID,
                                PaymentWithID = payWith.PaymentWithID
                            };
                            dPaymentMember.InsertstrPayment(pay);
                        }
                        else
                        {
                            var subtotal = (trMembership.Subtotal ?? 0) / 2;

                            payWith.payAmount = (firstPayment == 0) ? subtotal + (trMembership.Admin ?? 0) : subtotal;
                            payWith.paidAmount = (firstPayment == 0) ? subtotal + (trMembership.Admin ?? 0) : subtotal;

                            dpaymentWith.Insert(payWith);

                            //                            count += 1;
                            //
                            //                            if (count == payment)
                            //                            {
                            //
                            //                            }

                            var pay = new strPayment
                            {
                                trPaymentID = strpayMember.trPaymentID,
                                PaymentWithID = payWith.PaymentWithID
                            };
                            dPaymentMember.InsertstrPayment(pay);
                        }

                    }
                }
                ts.Complete();
            }

            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult MemberResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var cm = new DataServiceMembership().LoadDataRenewal(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in cm.ListClass
                         select new[]
                         {
                    Convert.ToString(count++),
                    d.tMember.MemberNO,
                    d.tMember.tPerson.PNama,
                    d.tMember.tPerson.PGender == "M" ? "Male" : "Female",
                    d.MSTglMulai.ToString("dd/MM/yyyy"),
                    d.MSTglSelesai.ToString("dd/MM/yyyy"),
                    d.tMember.tPerson.PAlamat,
                    d.tMember.tMemberType.MemberType,
                    Convert.ToString(d.trMembershipID)
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cm.TotalFilter,
                cm.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PersonList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel,
            int memberType)
        {
            var cm = new DataServiceMembership().PersonLoadData(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in cm.ListClass
                         select new[]
                         {
                    Convert.ToString(count++),
                    Convert.ToString(d.tMember.tPerson.PersonID),
                    d.tMember.MemberNO,
                    d.tMember.tPerson.PNama,
                    d.tMember.tPerson.PGender == "M" ? "Male" : "Female",
                    d.tMember.tPerson.PIdentitas,
                    d.tMember.tPerson.PTglLahir.Value.ToString("dd-MM-yyyy"),
                    d.tMember.tPerson.PHP1,
                    d.tMember.tPerson.PPinBB
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cm.TotalFilter,
                cm.Total), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMember(string searchTerm, int pageSize, int pageNum)
        {
            var m = new DataServiceMembership();
            List<trMembership> attendees = m.GetData(searchTerm, pageSize, pageNum);
            int attendeeCount = m.GetDataCount(searchTerm);

            //Translate the attendees into a format the select2 dropdown expects
            var pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult GetMemberNo(string memberNo)
        {
            var m = new DataServiceMembership();
            var member = m.GetobjByMemberNo(memberNo);
            var memberpersonIdentity = new
            {
                nama = member.tMember.tPerson.PNama,
                identitas = member.tMember.tPerson.PIdentitas,
                gender = member.tMember.tPerson.PGender,
                alamat = member.tMember.tPerson.PAlamat,
                tgllahir = member.tMember.tPerson.PTglLahir.Value.ToString("yyyy-MM-dd"),
                handphone = member.tMember.tPerson.PHP1,
                propinsi = member.tMember.tPerson.PPropinsi,
                kota = member.tMember.tPerson.PKota,
                kelurahan = member.tMember.tPerson.PKelurahan,
                kecamatan = member.tMember.tPerson.PKecamatan,
                hp1 = member.tMember.tPerson.PHP1,
                hp2 = member.tMember.tPerson.PHP2,
                pinbb = member.tMember.tPerson.PPinBB,
                email = member.tMember.tPerson.PEmail
            };
            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = memberpersonIdentity,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private Select2PagedResult AttendeesToSelect2Format(List<trMembership> attendees, int totalAttendees)
        {
            Select2PagedResult jsonAttendees = new Select2PagedResult();
            jsonAttendees.Results = new List<Select2StringResult>();

            //Loop through our attendees and translate it into a text value and an id for the select list
            foreach (var a in attendees)
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = a.MemberID.ToString(CultureInfo.InvariantCulture), text = a.tMember.tPerson.PNama });
            }
            //Set the total count of the results from the query.
            jsonAttendees.Total = totalAttendees;

            return jsonAttendees;
        }

        public CrystalReportPdfResult PrintAggreement(int id)
        {
            string reportPath = Server.MapPath(@"~\bin\Areas\Office\Views\Member\Report\PrintOutCreateMembership.rpt");
            var dMembership = new DataServiceMembership();
            var reportSource = (from m in dMembership.LoadAllData().Where(m => m.trMembershipID == id).ToList()
                                select new PrintoutMembership()
                                {
                                    AdministrationFee = m.Admin ?? 0,
                                    Alamat = m.tMember.tPerson.PAlamat,
                                    Barcode = "",
                                    Email = m.tMember.tPerson.PEmail,
                                    HP = m.tMember.tPerson.PHP1,
                                    HP2 = m.tMember.tPerson.PHP2,
                                    JenisKelamin = m.tMember.tPerson.PGender == "M" ? "Male" : "Female",
                                    KeanggotaanNo = m.tMember.MemberNO,
                                    KeanggotaanMulai = m.MSTglMulai,
                                    Nama = m.tMember.tPerson.PNama,
                                    Telp = m.tMember.tPerson.PTelp,
                                    identitas = m.tMember.tPerson.PIdentitas,
                                    tglLahir = m.tMember.tPerson.PTglLahir ?? DateTime.MinValue.AddYears(1899),
                                    Total = m.Total ?? 0,
                                    TypeMember = m.tMember.tMemberType.MemberType,
                                }).First();


            return new CrystalReportPdfResult(reportPath, new[] { reportSource }, null);

        }
    }
}