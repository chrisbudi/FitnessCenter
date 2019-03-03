using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.PT;
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
    public class FastMembershipController : FitController
    {
        private readonly IServiceCardStatus _cardstatusManager;
        public FastMembershipController(ServiceCardStatus cardstatusManager)
        {
            this._cardstatusManager = cardstatusManager;
        }

        // GET: Registrasi/Membership
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ViewMembership(int membershipId)
        {
            var model = new trMembership()
            {
                seq = 0,
                tMember = new tMember()
                {
                    tPerson = new tPerson()
                    {
                        PGender = "M"
                    }
                }
            };

            return PartialView("Editor/PersonEntityEditor", model);
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

        [HttpPost]
        public ActionResult AddDataPerson(int seq)
        {
            var model = new trMembership()
            {
                seq = seq + 1,
                tMember = new tMember()
                {
                    tPerson = new tPerson()
                    {
                        PGender = "M"
                    }
                }
            };

            return PartialView("Editor/PersonEntityEditor", model);
        }



        [HttpPost]
        public ActionResult AddDataPersonAddress(string memberId, string membershipId, string personnama)
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

        public ActionResult CreateMembership()
        {
            var dMembership = new DataServiceMembership();
            var membership = dMembership.Create(User.ActiveLocation);

            return View(membership);
        }

        #region CreateMembership
        [HttpPost]
        public ActionResult CreateMembership(ViewModelMembershipCreate member, FormCollection form)
        {
            var dMembership = new DataServiceMembership();
            var dPaymentMember = new DataServicePaymentMember();
            var dpaymentWith = new DataServicePaymentWith();
            var dStatusMember = new DataServiceStatusMember();
            var dMemberType = new DataServiceMemberType();
            var dPaketPT = new DataServicePaketPT();
            var dPersonalTrainer = new DataServicePersonalTrainer();
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
                    //trMembership.AgreementID = dMembership.GenerateMembershipAggrementId(User.ActiveLocation);
                    trMembership.StatusMID = dStatusMember.GetStatusId(EnumStatusMember.CalonMember);
                    trMembership.MSTglSelesai = trMembership.MSTglMulai.AddMonths(trMembership.TotalMonth);
                    trMembership.MSTglMulai = trMembership.MSTglMulai;
                    trMembership.LocationID = User.ActiveLocation;
                    trMembership.ParentID = parrentId;
                    trMembership.tMember.MemberTypeID = member.PaymentMember.MemberTypeID;
                    trMembership.DiscType = member.DiscType;
                    trMembership.DiscVal = member.DiscVal;
                    trMembership.Disc = member.DiscNominal;
                    trMembership.MSInput = DateTime.Now;


                    dMembership.InsertWithoutValidation(trMembership);

                    if (member.PaketPTId != null)
                    {
                        var paket = dPaketPT.GetobjById(member.PaketPTId.Value);
                        var pt = new trPersonalTrainer()
                        {
                            trMembershipID = trMembership.trMembershipID,
                            tPaketPTID = member.PaketPTId.Value,
                            SisaJam = paket.PPTJam,
                            Masa = paket.PPTMasa
                        };
                        dPersonalTrainer.InsertPT(pt);
                    }


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

            #region membershipOldWay
            //throw new Exception();

            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

            //            var ac = new AccountController(UserManager);
            //            var dPerson = new DataServicePerson();
            //            throw new Exception();
            //            var dMember = new DataServiceMember();
            //            var listCard = new List<ViewModelCardPrint>();1

            //var persons = memberCreate.Persons;
            //var membership = new List<trMembership>()
            //{
            //    new trMembership()
            //    {
            //        BOID = User.BackOffice.BOID,
            //        LocationID = User.ActiveLocation,
            //        MSTglSelesai = DateTime.Now,
            //        MSTglMulai = DateTime.Now,
            //        Subtotal = memberCreate.MemberType.Payment,
            //        Admin = memberCreate.MemberType.Admin,
            //        Prorate = memberCreate.MemberType.Prorate,
            //        Total = memberCreate.MemberType.TotalAmount,
            //        Disc = memberCreate.MemberType.Discount,
            //        Note = "member awal",
            //        StatusMID = new DataServiceStatusMember().GetStatusId(EnumStatusMember.Calon),
            //        PersonID = memberCreate.Persons.First().PersonID,
            //        CountMember = memberCreate.MemberType.CountMember,
            //        seq = 1
            //    }
            //};


            //            var parentId = 0;
            //                    var paymentPerson = memberCreate.Payments.Single(m => m.seq == entityPerson.Seq);
            //                    var memberType = memberCreate.MemberType.Prefix + paymentPerson.TotalMonth.Value.ToString("00");

            //                    var person = new tPerson()
            //                    {
            //                        PNama = entityPerson.PNama,
            //                        PGender = entityPerson.PGender,
            //                        PTglLahir = entityPerson.PTglLahir,
            //                        PHP1 = entityPerson.PHP1,
            //                        PAlamat = entityPerson.PAlamat,
            //                        PIdentitas = entityPerson.PIdentitas
            //                    };


            ////process to insert data person
            //                    if (entityPerson.Seq == 1)
            //                    {
            //                        person.PPropinsi = memberCreate.FirstPerson.PPropinsi;
            //                        person.PKota = memberCreate.FirstPerson.PKota;
            //                        person.PKelurahan = memberCreate.FirstPerson.PKelurahan;
            //                        person.PKecamatan = memberCreate.FirstPerson.PKecamatan;
            //                    person.PAlamat = entityPerson.PAlamat;
            //                    person.PIdentitas = entityPerson.PIdentitas;
            //                }


            // end Insert data person.

            //start insert member
            //                    var personId = person.PersonID;
            //                    var agreemenId = dMembership.GenerateMembershipAggrementId(User.ActiveLocation);
            //                    var memberId = agreemenId + memberType + DateTime.Now.ToString("MMyyyy");



            /// Create membership tidak langsung menjadi member
            /// harus di aktivkan terlebih dahulu

            //                    var member = new tMember()
            //                    {
            //                        MemberID = memberId,
            //                        PersonID = personId,
            //                        MemberTypeID = memberCreate.MemberType.MemberType,
            //                        MMulai = DateTime.Now
            //                    };
            //                    //                    throw new Exception();
            //                    dMember.Insert(member);
            //EndInsertMember

            //start insert transactionMemberCore
            //                    var transactionMember = new trMembership
            //                    {
            //                        //                        Admin = memberCreate.MemberType.Admin,
            //                        AgreementID = "",//agreemenId,
            //                        PersonBOIDADM = User.Person.PersonID,
            //                        //                        CountMember = memberCreate.MemberType.CountMember,
            //                        MemberID = 0,
            //                        GenBayar = 0,
            //                        LocationID = 0,
            //                        MSTglMulai = memberCreate.Payments.Single(m => m.seq == entityPerson.Seq).startDatetime,
            //                        MSTglSelesai = memberCreate.Payments.Single(m => m.seq == entityPerson.Seq).startDatetime.AddMonths(memberCreate.MemberCount),
            //                        Note = "",
            //                        StatusMID = dStatusMember.GetStatusId(EnumStatusMember.Membership),
            //                        seq = entityPerson.Seq,
            //                        //                        Prorate = memberCreate.MemberType.Prorate,
            //                        PersonBOIDSales = memberCreate.MasterPayment.MemberType.Value,
            //                        //CardPrint = true,
            //                        TotalMonth = memberCreate.Payments.Single(m => m.seq == entityPerson.Seq).TotalMonth.Value,
            //                        Total = paymentPerson.Totalpayment,
            //                        ActivationCode = Guid.NewGuid().GetHashCode().ToString("x")
            //                    };


            //                    decimal percent = 1 - ((memberCreate.MemberType.DiscountPct.HasValue ? memberCreate.MemberType.DiscountPct.Value : 0) / 100);

            //                    var subtotal = paymentPerson.Totalpayment;

            //                    transactionMember.Subtotal = decimal.Parse((transactionMember.Total / percent ?? 0).ToString("N2"));
            //(1 - (memberCreate.MemberType.DiscountPct / 100));
            //throw new Exception();
            //                    transactionMember.Disc = transactionMember.Subtotal - transactionMember.Total;
            //                    //                    throw new Exception();
            //                    //throw new Exception();
            //                    if (entityPerson.Seq > 1)
            //                    {
            //                        transactionMember.ParentID = parentId;
            //                    }
            //
            //                    transactionMember.LocationID = User.ActiveLocation;
            //                    dMembership.Insert(transactionMember);
            //
            //                    if (parentId == 0)
            //                    {
            //                        parentId = transactionMember.trMembershipID;
            //                    }

            //Insert Location Member
            //                    var strLocMember = new strLocMember
            //                    {
            //                        MemberID = member.MemberID,
            //                        LocationID = User.ActiveLocation
            //                    };
            //                    dMembership.MemberLocSave(strLocMember);
            //End Insert Location Member

            //                    if (memberCreate.MemberType.Prorate != 0)
            //                    {
            //                        for (var i = 0; i <= memberCreate.MemberType.CountMember - 1; i++)
            //                        {
            //                            var statusBayar = false || (i == 0 || i == memberCreate.MemberType.CountMember - 1);
            //
            //                            var payMember = new strPaymentMember()
            //                            {
            //                                trMembershipID = membershipId,
            //                                pembayaranke = i,
            //                                statusBayar = statusBayar,
            //                                Tanggal = DateTime.Now,
            //                                MemberTypeID = memberCreate.MemberType.MemberType,
            //                                Note = ""
            //                            };
            //                            dPaymentMember.Insert(payMember);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        var payMember = new strPaymentMember()
            //                        {
            //                            trMembershipID = membershipId,
            //                            pembayaranke = 0,
            //                            statusBayar = true,
            //                            Tanggal = DateTime.Now,
            //                            MemberTypeID = memberCreate.MemberType.MemberType,
            //                            Note = ""
            //                        };
            //                        dPaymentMember.Insert(payMember);
            //                    }


            //                    ViewModelCardPrint card = new ViewModelCardPrint()
            //                    {
            //                        MemberType = memberCreate.MemberType.MemberTypeDef,
            //                        MemberId = member.MemberID,
            //                        MemberName = entityPerson.PNama
            //                    };
            //                    listCard.Add(card);


            // End All insert data to database

            // Create membership
            //                    var applicationUser = new ApplicationUser { UserName = member.MemberID, EmailConfirmed = true };
            //                    var user = Task.Run(() => ac.RegAsync(applicationUser, "123456"));

            //                    var personFinish = dPerson.LoadAllData().Single(m => m.PersonID == personId);
            //                    personFinish.Id = user.Result.Id;



            //var memberId = agreemenId + memberCreate.MemberType.MemberType.Value +
            //                (membershipCreate.Payments.Single(m => m.seq == tmembership.seq)
            //                .TotalMonth.Value.ToString("00") + DateTime.Now.ToString("yyMM"));






            //var aggreementId = dMembership.GenerateMembershipAggrementID(User.ActiveLocation);

            //tmembership.AgreementID +
            //            dPaymentMember.GetPaymentByMembership(tmembership.trMembershipID)
            //                .First()
            //                .tMemberType.prFix +

            //                (membershipCreate.Payments.Single(m => m.seq == tmembership.seq)
            //                .TotalMonth.Value.ToString("00") + DateTime.Now.ToString("yyMM"));

            //var applicationUser = new ApplicationUser { UserName = "", EmailConfirmed = true };

            //var str = Task.Run(() => ac.Reg(applicationUser, "123456"));

            //var personID = 0;
            //if (tPersons.seq == 1)
            //{
            //    var tmembership1 = tPersons;
            //    var ps = dPerson.GetobjById(tmembership1.PersonID);
            //    var person = memberCreate.Persons.First();
            //    ps.PNama = person.PNama;
            //    ps.PPinBB = person.PPinBB;
            //    ps.PTglLahir = person.PTglLahir;
            //    ps.PHP1 = person.PHP1;
            //    ps.PAlamat = person.PAlamat;
            //    ps.PTelp = person.PTelp;
            //    ps.PEmail = person.PEmail;
            //    ps.Id = str.Result.Id;
            //    ps.PGender = person.PGender;
            //    ps.PIdentitas = person.PIdentitas;

            //    personID = person.PersonID;

            //    dPerson.Update(ps);
            //}

            //if (tPersons.seq == 2)
            //{
            //    var tmembership1 = tPersons;

            //    var ps = memberCreate.Persons.Single(p => p.AgreementId == tmembership1.AgreementID);
            //    var person = new tPerson
            //    {
            //        PNama = ps.PNama,
            //        PAlamat = ps.PAlamat,
            //        PTglLahir = ps.PTglLahir,
            //        PHP1 = ps.PHP1,
            //        PTelp = ps.PTelp,
            //        PPinBB = ps.PPinBB,
            //        PEmail = ps.PAlamat,
            //        Id = str.Result.Id,
            //        PGender = ps.PGender,
            //        PIdentitas = ps.PIdentitas
            //    };

            //    dPerson.Insert(person);


            //    if (tPersons.seq == 2)
            //    {
            //        personID = dPerson.GetLastPersonId();
            //        tPersons.PersonID = personID;
            //    }
            //}
            //tPersons.StatusMID = new DataServiceStatusMember().GetStatusId(EnumStatusMember.Member);
            //var member = new tMember()
            //{
            //    PersonID = personID,
            //    MemberID = "",
            //    MMulai = DateTime.Now,
            //};
            //dMember.Insert(member);
            //var strLocMember = new strLocMember
            //{
            //    MemberID = member.MemberID,
            //    LocationID = User.ActiveLocation
            //};

            //dMembership.MemberLocSave(strLocMember);
            //tPersons.MemberID = member.MemberID;
            //dMembership.Update(tPersons);

            //var payM = dPaymentMember.GetPaymentByMembership(tPersons.trMembershipID).First();
            //payM.Note = memberCreate.Payments.First(m => m.seq == tPersons.seq).Note;
            //dPaymentMember.Update(payM);

            //foreach (var payment in memberCreate.PaymentDetails)
            //{
            //    if (payment.AgreementId == tPersons.AgreementID)
            //    {
            //        var payWith = new trPaymentWith
            //        {
            //            BankID = payment.Bank,
            //            PaymentTypeID = dPaymentType.GetIdByName(payment.PaymentType),
            //            NoKartu = payment.NoKartu,
            //            Pemegang = payment.Pemegang,
            //            ValidUntil = payment.ValidUntil,
            //            payAmount = payment.PaymentAmount
            //        };
            //        dpaymentWith.Insert(payWith);
            //        var payMember = dPaymentMember.GetPaymentByMembership(tPersons.trMembershipID).First();
            //        var pay = new strPayment
            //        {
            //            trPaymentID = payMember.trPaymentID,
            //            PaymentWithID = payWith.PaymentWithID
            //        };
            //        dPaymentMember.InsertstrPayment(pay);
            //    }
            //}

            //Export to CSV file
            //                string folderName = DateTime.Now.ToString("HH-mm-ss");
            //
            //                string folderPath = string.Format(@"C:\FitnessExcel\Membership\{0}", folderName);
            //                string fileName = "Member" + folderName + ".csv";
            //
            //                listCard.ToCSV(path: folderPath, fileName: fileName);

            //
            //                foreach (var member in listCard)
            //                {
            //                    QRCodeEncoder encoder = new QRCodeEncoder
            //                    {
            //                        QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M,
            //                        QRCodeScale = 10
            //                    };
            //
            //                    // 30%
            //
            //                    Bitmap img = encoder.Encode(@"https:\\App.flashfitnessindonesia.com\Member\" + member.MemberId);
            //                    //            LogoUpload.SaveAs(path + LogoUpload.FileName);
            //                    Image logoFullScale = FitnessCenter.Properties.Resources.Logo;
            //                    Image logo = new Bitmap(logoFullScale, new Size(100, 75));
            //
            //                    int left = (331 / 2) - (logo.Width / 2);
            //                    int top = (331 / 2) - (logo.Height / 2);
            //
            //                    Graphics g = Graphics.FromImage(img);
            //                    g.DrawImage(logo, new Point(left, top));
            //                    img.Save(folderPath + "\\" + member.MemberId + ".jpg", ImageFormat.Jpeg);
            //                }
            #endregion

            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult MembershipResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var cm = new DataServiceMembership().LoadData(requestModel, User.Person.PersonID);
            var count = requestModel.Start + 1;
            var result = from d in cm.ListClass
                         select new[]
                         {
                    //d.trMembershipID.ToString(),
                    Convert.ToString(count++),
                    d.tMember.tPerson.PNama,
                    d.tMember.tPerson.PGender == "M" ? "Male" : "Female",
                    d.tMember.tPerson.PAlamat,
                    d.tMember.tPerson.PTelp,
                    d.tMember.tPerson.PHP1,
                    d.strAktivitasSales.Any()?
                    d.strAktivitasSales.OrderByDescending(m => m.AktivitasSalesID)
                        .Take(1)
                        .Single()
                        .tSalesAction.ActionName:"",
                    d.strAktivitasSales.Any()?
                    d.strAktivitasSales.OrderByDescending(m => m.AktivitasSalesID)
                        .Take(1)
                        .Single()
                        .tMemberState.MemberStateName:"",
                    Convert.ToString(d.trMembershipID),
                    Convert.ToString(d.trMembershipID),
                    Convert.ToString(d.tMember.tPerson.PersonID)
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
                    d.tMember.tPerson.PTglLahir?.ToString("dd-MM-yyyy")??"",
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


        public ActionResult MembershipList(string searchTerm, int pageSize, int pageNum)
        {
            var ar = new DataServiceMembership();
            int count;
            var type = ar.GetMember(searchTerm, pageSize, pageNum, out count).ToList();



            var jsonAttendees = new Select2PagedResult
            {
                Results = new List<Select2StringResult>()
            };

            foreach (var action in type)
            {
                jsonAttendees.Results.Add(new Select2StringResult
                {
                    id = action.tMember.MemberNO,
                    text = action.tMember.MemberNO,
                    note = new string[] { action.tMember.tPerson.PAlamat, action.tMember.tPerson.PNama }
                });
            }

            jsonAttendees.Total = count;

            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult MembershipListByMemberNo(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(null);

            DataServiceMembership dType = new DataServiceMembership();

            var types = dType.LoadAllData().Where(m => m.tMember.MemberNO == id);

            var jsonAttendees = new Select2PagedResult { Results = new List<Select2StringResult>() };

            foreach (var action in types.ToList())
            {
                jsonAttendees.Results.Add(new Select2StringResult { id = action.tMember.MemberNO, text = action.tMember.MemberNO });
            }

            jsonAttendees.Total = types.Count();

            return new Jsonp
            {
                Data = jsonAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}