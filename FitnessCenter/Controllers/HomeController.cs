using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using DataAccessService.Activity;
using DataAccessService.Master;
using DataAccessService.PT;
using DataAccessService.Registrasi;
using DataObjects.Context;
using DataObjects.Entities;
using IdentityModel.Config;
using IdentityModel.Model;
using Microsoft.AspNet.Identity.Owin;
using Services.Helpers;
using ViewModel.Identity;

namespace FitnessCenter.Controllers
{
    [Authorize]
    public class HomeController : FitController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [HttpGet]
        public ActionResult ClearSession()
        {
            Session.Clear();
            return Json("Data Has Been Cleared");
        }

        public ActionResult Index()
        {

            DataServiceCheckInOut dActivityMember = new DataServiceCheckInOut();
            DataServiceMembership dMembership = new DataServiceMembership();
            DataServicePersonalTrainer dPersonalTrainer = new DataServicePersonalTrainer();
            ViewModelDashboard dashboard = new ViewModelDashboard()
            {
                MemberActivity = dActivityMember.LoadAllData(),
                Membership = dMembership.LoadAllData(),
                PersonalTrainers = dPersonalTrainer.LoadAllData()
            };

            return View(dashboard);
        }

        public ActionResult LocationIndex()
        {
            return View();
        }

        public ActionResult LocationIndex(int location)
        {
            User.ActiveLocation = location;
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        [Authorize(Roles = "homecontact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult PostAction()
        {
            var fitMember = new FitEntity();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
            {
                try
                {
                    foreach (var e in fitMember.ExcelUploadDBs.Where(m => m.prfix != "A" &&
                    m.MembershipTypeFix != "Voucher Java" &&
                    m.MembershipTypeFix != "Insidentil" &&
                    m.MembershipTypeFix != "Groupon 2 Month" &&
                    m.MembershipTypeFix != "2 Month" &&
                    m.MembershipTypeFix != "Monthly" &&
                    m.MembershipTypeFix != null &&
                    m.No > 1500 &&
                    !m.MembershipTypeFix.Equals(string.Empty) &&
                    m.AgreementFix != "LP0000000"))
                    {
                        Debug.WriteLine(e.No);
                        var dPerson = new DataServicePerson();
                        var dMember = new DataServiceMember();
                        var dMembership = new DataServiceMembership();
                        var dPaymentMember = new DataServicePaymentMember();
                        var dpaymentWith = new DataServicePaymentWith();
                        var dPaymentType = new DataServicePaymentMember();
                        var dStatusMember = new DataServiceStatusMember();
                        var dMemberType = new DataServiceMemberType();
                        var ac = new AccountController(UserManager);

                        var person = new tPerson()
                        {
                            PNama = e.MemberName,
                            PIdentitas = "",
                            PGender = "M",
                            PTglLahir = e.DateOfBirth,
                            PHP1 = e.Telp,
                            PAlamat = e.Address,
                            PTelp = e.Telp
                        };
                        Debug.WriteLine("person");
                        dPerson.InsertWithoutValidation(person);
                        // end Insert data person.

                        //start insert member
                        //var memberType = dMemberType.GetobjByMemberType(e.MembershipType);//(e.LongTime ?? 0).ToString("00");

                        var personId = person.PersonID;
                        var agreemenId = e.AgreementFix;
                        var memberNO = agreemenId + e.prfix + (e.LongTime ?? 0).ToString("00");
                        Debug.WriteLine(e.MembershipTypeFix);
                        var member = new tMember()
                        {
                            PersonID = personId,
                            MemberTypeID = dMemberType.GetobjByMemberType(e.MembershipTypeFix).MemberTypeID,
                            MemberNO = memberNO,
                        };
                        //                    throw new Exception();
                        Debug.WriteLine("member");
                        dMember.Insert(member);

                        //                        throw new Exception();
                        //EndInsertMember

                        //start insert transactionMemberCore
                        var transactionMember = new trMembership
                        {
                            Admin = e.Paid1,
                            AgreementID = agreemenId,
                            CountMember = 0,
                            GenBayar = 0,
                            MSTglMulai = e.Date ?? DateTime.Now,
                            MSTglSelesai = (e.Date ?? DateTime.Now).AddMonths((e.LongTime ?? 0)),
                            Note = e.Remarks,
                            StatusMID = dStatusMember.GetStatusId(EnumStatusMember.Membership),
                            seq = 1,
                            Prorate = 0,
                            TotalMonth = (e.LongTime ?? 0),
                            Total = (e.Paid2 ?? 0) + (e.Paid3 ?? 0),
                            AccountingStatus = EnumStatusAccounting.Post.ToString("F"),
                            ActivationCode = "",
                            CardStatus = 4,
                            LocationID = User.ActiveLocation,
                            MemberID = member.MemberID,
                            PersonBOIDSales = User.Person.PersonID,
                            PersonBOIDADM = User.Person.PersonID,
                        };


                        transactionMember.Subtotal = transactionMember.Total;
                        //(1 - (memberCreate.MemberType.DiscountPct / 100));
                        //throw new Exception();
                        transactionMember.Disc = 0;
                        transactionMember.LocationID = User.ActiveLocation;
                        Debug.WriteLine("transactionmembership");
                        Debug.WriteLine(transactionMember);
                        dMembership.Insert(transactionMember);


                        //Insert Location Member
                        var strLocMember = new strLocMember
                        {
                            MemberID = member.MemberID,
                            LocationID = User.ActiveLocation
                        };
                        dMembership.MemberLocSave(strLocMember);
                        //End Insert Location Member
                        //                        var statusBayar = false || (i == 0 || i == memberCreate.MemberType.CountMember - 1);

                        var paymentmember = new strPaymentMember()
                        {
                            pembayaranke = 0,//i,
                            statusBayar = true,//statusBayar,
                            Tanggal = DateTime.Now,
                            Note = "Generate Program ",
                            MemberTypeID = dMemberType.GetobjByMemberType(e.MembershipTypeFix).MemberTypeID,
                            trMembershipID = transactionMember.trMembershipID
                        };
                        Debug.WriteLine("dPaymentMember");
                        dPaymentMember.Insert(paymentmember);
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


                        var trpaymentWith = new trPaymentWith()
                        {
                            BankID = null,
                            PaymentTypeID = dPaymentType.GetIdByName("Cash"),
                            NoKartu = "",//payment.CardNo,
                            TraceCode = "",//payment.TraceCode,
                            ApprCode = "",//payment.ApprCode,
                            ValidUntil = DateTime.MinValue.AddYears(1989),//payment.ValidUntil,
                            payAmount = (e.Paid2 ?? 0) + (e.Paid3 ?? 0)
                        };
                        Debug.WriteLine("paymentwith");
                        dpaymentWith.Insert(trpaymentWith);
                        var payMember = dPaymentMember.GetPaymentByMembership(transactionMember.trMembershipID).First();
                        var pay = new strPayment
                        {
                            trPaymentID = payMember.trPaymentID,
                            PaymentWithID = trpaymentWith.PaymentWithID
                        };
                        dPaymentMember.InsertstrPayment(pay);

                        var applicationUser = new ApplicationUser { UserName = member.MemberNO, EmailConfirmed = true };
                        var user = Task.Run(() => ac.RegAsync(applicationUser, "123456"));

                        var personFinish = dPerson.LoadAllData().Single(m => m.PersonID == personId);
                        personFinish.Id = user.Result.Id;

                        person.Id = personFinish.Id;

                        dPerson.Update(person);

                        Debug.WriteLine("==============================================");

                    }

                    scope.Complete();

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult PostMonthly()
        {
            var fitMember = new FitEntity();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
            {
                try
                {
                    foreach (var e in fitMember.ExcelUploadDBs.Where(m => m.prfix != "A" &&
                    m.MembershipTypeFix == "Monthly" &&
                    !m.MembershipTypeFix.Equals(string.Empty) &&
                    m.AgreementFix != "LP0000000"))
                    {
                        Debug.WriteLine(e.No);
                        var dPerson = new DataServicePerson();
                        var dMember = new DataServiceMember();
                        var dMembership = new DataServiceMembership();
                        var dPaymentMember = new DataServicePaymentMember();
                        var dpaymentWith = new DataServicePaymentWith();
                        var dPaymentType = new DataServicePaymentMember();
                        var dStatusMember = new DataServiceStatusMember();
                        var dMemberType = new DataServiceMemberType();
                        var ac = new AccountController(UserManager);

                        var person = new tPerson()
                        {
                            PNama = e.MemberName,
                            PIdentitas = "",
                            PGender = "M",
                            PTglLahir = e.DateOfBirth,
                            PHP1 = e.Telp,
                            PAlamat = e.Address,
                            PTelp = e.Telp
                        };
                        Debug.WriteLine("person");
                        dPerson.InsertWithoutValidation(person);
                        // end Insert data person.

                        //start insert member
                        //var memberType = dMemberType.GetobjByMemberType(e.MembershipType);//(e.LongTime ?? 0).ToString("00");

                        var personId = person.PersonID;
                        var agreemenId = e.AgreementFix;
                        var memberNO = agreemenId + e.prfix + (e.LongTime ?? 0).ToString("00");
                        Debug.WriteLine(e.MembershipTypeFix);
                        var member = new tMember()
                        {
                            PersonID = personId,
                            MemberTypeID = dMemberType.GetobjByMemberType(e.MembershipTypeFix).MemberTypeID,
                            MemberNO = memberNO,
                        };
                        //                    throw new Exception();
                        Debug.WriteLine("member");
                        dMember.Insert(member);

                        //                        throw new Exception();
                        //EndInsertMember

                        //start insert transactionMemberCore
                        var transactionMember = new trMembership
                        {
                            Admin = e.Paid1,
                            AgreementID = agreemenId,
                            CountMember = 0,
                            GenBayar = 0,
                            MSTglMulai = e.Date ?? DateTime.Now,
                            MSTglSelesai = (e.Date ?? DateTime.Now).AddMonths((e.LongTime ?? 0)),
                            Note = e.Remarks,
                            StatusMID = dStatusMember.GetStatusId(EnumStatusMember.Membership),
                            seq = 1,
                            Prorate = 0,
                            TotalMonth = (e.LongTime ?? 0),
                            Total = (e.Paid2 ?? 0) + (e.Paid3 ?? 0),
                            AccountingStatus = EnumStatusAccounting.Post.ToString("F"),
                            ActivationCode = "",
                            CardStatus = 4,
                            LocationID = User.ActiveLocation,
                            MemberID = member.MemberID,
                            PersonBOIDSales = User.Person.PersonID,
                            PersonBOIDADM = User.Person.PersonID,
                        };


                        transactionMember.Subtotal = transactionMember.Total;
                        //(1 - (memberCreate.MemberType.DiscountPct / 100));
                        //throw new Exception();
                        transactionMember.Disc = 0;
                        transactionMember.LocationID = User.ActiveLocation;
                        Debug.WriteLine("transactionmembership");
                        Debug.WriteLine(transactionMember);
                        dMembership.Insert(transactionMember);


                        //Insert Location Member
                        var strLocMember = new strLocMember
                        {
                            MemberID = member.MemberID,
                            LocationID = User.ActiveLocation
                        };
                        dMembership.MemberLocSave(strLocMember);
                        //End Insert Location Member
                        //                        var statusBayar = false || (i == 0 || i == memberCreate.MemberType.CountMember - 1);


                        var datePaymentMember = transactionMember.MSTglMulai.Date;
                        var secondpaymentdate = new DateTime(transactionMember.MSTglMulai.Year, transactionMember.MSTglMulai.Month, 1);
                        var firstpaymentdate = new DateTime(transactionMember.MSTglMulai.Year, transactionMember.MSTglMulai.Month, 15);

                        if (datePaymentMember.Ticks >= firstpaymentdate.Ticks &&
                            datePaymentMember.Ticks <= secondpaymentdate.Ticks)
                        {
                            if (transactionMember.MSTglMulai.Month == 2)
                            {

                                datePaymentMember = new DateTime(transactionMember.MSTglMulai.Year, transactionMember.MSTglMulai.Month, 30);

                            }
                            else
                            {
                                datePaymentMember = new DateTime(transactionMember.MSTglMulai.Year, transactionMember.MSTglMulai.Month, 28);
                            }
                        }

                        else
                            datePaymentMember = new DateTime(transactionMember.MSTglMulai.Year, transactionMember.MSTglMulai.Month, 15);

                        for (int i = 0; i <= 11; i++)
                        {
                            var paymentMember = new strPaymentMember()
                            {
                                Note = "",
                                pembayaranke = i + 1,
                                statusBayar = i == 0 || i == 11,
                                trMembershipID = transactionMember.trMembershipID,
                                MemberTypeID = member.MemberTypeID,
                                Tanggal = (datePaymentMember.AddMonths(i + 1)),
                                MembershipDTLID = i == 0 ? transactionMember.trMembershipID : new int()
                            };
                            if (paymentMember.MembershipDTLID == 0)
                            {
                                paymentMember.MembershipDTLID = null;
                            }

                            Debug.WriteLine("dPaymentMember");
                            dPaymentMember.Insert(paymentMember);
                            //                            throw new Exception();
                        }
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


                        var trpaymentWith = new trPaymentWith()
                        {
                            BankID = null,
                            PaymentTypeID = dPaymentType.GetIdByName("Cash"),
                            NoKartu = "",//payment.CardNo,
                            TraceCode = "",//payment.TraceCode,
                            ApprCode = "",//payment.ApprCode,
                            ValidUntil = DateTime.MinValue.AddYears(1989),//payment.ValidUntil,
                            payAmount = (e.Paid2 ?? 0) + (e.Paid3 ?? 0)
                        };
                        Debug.WriteLine("paymentwith");
                        dpaymentWith.Insert(trpaymentWith);
                        var payMember = dPaymentMember.GetPaymentByMembership(transactionMember.trMembershipID).First();
                        var pay = new strPayment
                        {
                            trPaymentID = payMember.trPaymentID,
                            PaymentWithID = trpaymentWith.PaymentWithID
                        };
                        dPaymentMember.InsertstrPayment(pay);

                        var applicationUser = new ApplicationUser { UserName = member.MemberNO, EmailConfirmed = true };
                        var user = Task.Run(() => ac.RegAsync(applicationUser, "123456"));

                        var personFinish = dPerson.LoadAllData().Single(m => m.PersonID == personId);
                        personFinish.Id = user.Result.Id;

                        person.Id = personFinish.Id;

                        dPerson.Update(person);

                        Debug.WriteLine("==============================================");

                    }

                    scope.Complete();

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public ActionResult PostInsidentil()
        {
            var fitMember = new FitEntity();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
            {
                try
                {
                    foreach (var e in fitMember.ExcelUploadDBs.Where(m => m.prfix != "A" &&
                        (m.MembershipTypeFix == "Monthly Insidentil" || m.MembershipTypeFix == "Groupon" || m.MembershipTypeFix == "2 Month Insidentil") &&
                        m.AgreementFix != "LP0000000"))
                    {
                        Debug.WriteLine(e.No);
                        var dPerson = new DataServicePerson();
                        var dMember = new DataServiceMember();
                        var dMembership = new DataServiceMembership();
                        var dPaymentMember = new DataServicePaymentMember();
                        var dpaymentWith = new DataServicePaymentWith();
                        var dPaymentType = new DataServicePaymentMember();
                        var dStatusMember = new DataServiceStatusMember();
                        var dMemberType = new DataServiceMemberType();
                        var ac = new AccountController(UserManager);

                        var person = new tPerson()
                        {
                            PNama = e.MemberName,
                            PIdentitas = "",
                            PGender = "M",
                            PTglLahir = e.DateOfBirth,
                            PHP1 = e.Telp,
                            PAlamat = e.Address,
                            PTelp = e.Telp
                        };
                        Debug.WriteLine("person");
                        dPerson.InsertWithoutValidation(person);
                        // end Insert data person.

                        //start insert member
                        //var memberType = dMemberType.GetobjByMemberType(e.MembershipType);//(e.LongTime ?? 0).ToString("00");

                        var personId = person.PersonID;
                        var agreemenId = e.AgreementFix;
                        var memberNO = agreemenId + e.prfix + (e.LongTime ?? 0).ToString("00");
                        Debug.WriteLine(e.MembershipTypeFix);
                        var member = new tMember()
                        {
                            PersonID = personId,
                            MemberTypeID = dMemberType.GetobjByMemberType(e.MembershipTypeFix).MemberTypeID,
                            MemberNO = memberNO,
                        };
                        //                    throw new Exception();
                        Debug.WriteLine("member");
                        dMember.Insert(member);

                        //                        throw new Exception();
                        //EndInsertMember

                        //start insert transactionMemberCore
                        var transactionMember = new trMembership
                        {
                            Admin = e.Paid1,
                            AgreementID = agreemenId,
                            CountMember = 0,
                            GenBayar = 0,
                            MSTglMulai = e.Date ?? DateTime.Now,
                            MSTglSelesai = (e.Date ?? DateTime.Now).AddMonths((e.LongTime ?? 0)),
                            Note = e.Remarks,
                            StatusMID = dStatusMember.GetStatusId(EnumStatusMember.Membership),
                            seq = 1,
                            Prorate = 0,
                            TotalMonth = (e.LongTime ?? 0),
                            Total = (e.Paid2 ?? 0) + (e.Paid3 ?? 0),
                            AccountingStatus = EnumStatusAccounting.Post.ToString("F"),
                            ActivationCode = "",
                            CardStatus = 4,
                            LocationID = User.ActiveLocation,
                            MemberID = member.MemberID,
                            PersonBOIDSales = User.Person.PersonID,
                            PersonBOIDADM = User.Person.PersonID,
                        };


                        transactionMember.Subtotal = transactionMember.Total;
                        //(1 - (memberCreate.MemberType.DiscountPct / 100));
                        //throw new Exception();
                        transactionMember.Disc = 0;
                        transactionMember.LocationID = User.ActiveLocation;
                        Debug.WriteLine("transactionmembership");
                        Debug.WriteLine(transactionMember);
                        dMembership.Insert(transactionMember);


                        //Insert Location Member
                        var strLocMember = new strLocMember
                        {
                            MemberID = member.MemberID,
                            LocationID = User.ActiveLocation
                        };
                        dMembership.MemberLocSave(strLocMember);
                        //End Insert Location Member
                        //                        var statusBayar = false || (i == 0 || i == memberCreate.MemberType.CountMember - 1);

                        var paymentmember = new strPaymentMember()
                        {
                            pembayaranke = 0, //i,
                            statusBayar = true, //statusBayar,
                            Tanggal = DateTime.Now,
                            Note = "Generate Program ",
                            MemberTypeID = dMemberType.GetobjByMemberType(e.MembershipTypeFix).MemberTypeID,
                            trMembershipID = transactionMember.trMembershipID
                        };
                        Debug.WriteLine("dPaymentMember");
                        dPaymentMember.Insert(paymentmember);
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


                        var trpaymentWith = new trPaymentWith
                        {
                            BankID = null,
                            PaymentTypeID = dPaymentType.GetIdByName("Cash"),
                            NoKartu = "",
                            TraceCode = "",
                            ApprCode = "",
                            ValidUntil = DateTime.MinValue.AddYears(1989),
                            payAmount = (e.Paid2 ?? 0) + (e.Paid3 ?? 0)
                        };

                        Debug.WriteLine("paymentwith");
                        dpaymentWith.Insert(trpaymentWith);
                        var payMember = dPaymentMember.GetPaymentByMembership(transactionMember.trMembershipID).First();
                        var pay = new strPayment
                        {
                            trPaymentID = payMember.trPaymentID,
                            PaymentWithID = trpaymentWith.PaymentWithID
                        };
                        dPaymentMember.InsertstrPayment(pay);

                        var applicationUser = new ApplicationUser { UserName = member.MemberNO, EmailConfirmed = true };
                        var user = Task.Run(() => ac.RegAsync(applicationUser, "123456"));

                        var personFinish = dPerson.LoadAllData().Single(m => m.PersonID == personId);
                        personFinish.Id = user.Result.Id;

                        person.Id = personFinish.Id;

                        dPerson.Update(person);

                        Debug.WriteLine("==============================================");

                    }

                    scope.Complete();

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
