using System;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.PT;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using FitnessCenter.Models.Crystal_Report;
using Services.DataTables;
using Services.Helpers;
using ViewModel.Trainer.RegisterPT;
using ViewModel.Membership.Registrasi;

namespace FitnessCenter.Areas.Trainers.Controllers
{
    public class RegisterPTController : FitController
    {

        private readonly IServiceCardStatus _cardstatusManager;

        public RegisterPTController(ServiceCardStatus cardstatusManager)
        {
            this._cardstatusManager = cardstatusManager;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            trPersonalTrainer pkt = new DataServicePersonalTrainer().GetobjById(id);
            return View(pkt);
            //return null;
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
        public ActionResult AddDataPersonPayment(string memberId, string membershipId, string personnama, int pttypeId)
        {

            DataServicePaketPT servicePT = new DataServicePaketPT();

            var PT = servicePT.GetobjById(pttypeId);

            var model = new trPersonalTrainer()
            {
                Masa = PT.PPTMasa,
                SisaJam = PT.PPTJam,
                trMembership = new trMembership
                {
                    tMember = new tMember()
                    {
                        tPerson = new tPerson()
                        {
                            PNama = personnama
                        }
                    }
                }
            };
            ViewBag.memberId = memberId;
            ViewBag.membershipId = membershipId;

            return PartialView("Editor/PaymentEntityEditor", model);
        }

        [HttpGet]
        public ActionResult AddDataPerson(int seq, int memberid = 0)
        {
            var membership = new trPersonalTrainer();
            if (seq == 1)
            {
                var dMember = new DataServiceMembership();
                membership = dMember.LoadAllData().Where(m => m.trMembershipID == memberid).ToList()
                    .Select(m => new trPersonalTrainer()
                    {
                        trMembership = new trMembership()
                        {
                            tMember = m.tMember,
                            MemberID = m.MemberID
                        },
                        seq = seq,
                        //                        MemberID = m.MemberID
                    }).SingleOrDefault();
            }
            else
            {
                membership = new trPersonalTrainer()
                {
                    seq = seq
                };
            }


            return PartialView("Editor/PersonEntityEditor", membership);
        }

        public ActionResult CreatePersonalTrainer(int id = 0)
        {

            if (id == 0)
            {
                return HttpNotFound();
            }


            ViewBag.TransId = id;
            var reg = new DataServicePersonalTrainer();
            var pt = reg.Create(id, User.Person.PersonID, User.ActiveLocation);

            //            pt.PersonalTrainer.trMembership.Total = 0;
            //            pt.PersonalTrainer.trMembership.Subtotal = 0;
            //            pt.PersonalTrainer.trMembership.Disc = 0;

            return View(pt);
        }

        [HttpPost]
        public ActionResult CreatePersonalTrainer(ViewModelPTCreate trainer, FormCollection form)
        {
            //            trainer.PersonalTrainer.tMember = null;

            var dReg = new DataServicePersonalTrainer();
            var dPaymentMember = new DataServicePaymentMember();
            var dpaymentWith = new DataServicePaymentWith();
            var dStatusMember = new DataServiceStatusMember();

            var parentId = 0;
            using (var ts = new TransactionScope())
            {
                foreach (var pTrainer in trainer.PersonalTrainers)
                {
                    if (parentId != 0)
                    {
                        pTrainer.ParentID = parentId;
                    }
                    var cardId = _cardstatusManager.Get("Skip").CardStatusID;


                    //                    throw new Exception();
                    pTrainer.trMembership.AccountingStatus = null;
                    pTrainer.trMembership.MSTglMulai = trainer.PersonalTrainer.trMembership.MSTglMulai;
                    pTrainer.trMembership.MSTglSelesai = trainer.PersonalTrainer.trMembership.MSTglMulai.AddDays(trainer.PersonalTrainer.Masa ?? 0);
                    pTrainer.trMembership.LocationID = User.ActiveLocation;
                    pTrainer.trMembership.PersonBOIDSales = trainer.PersonalTrainer.trMembership.PersonBOIDSales;
                    pTrainer.trMembership.PersonBOIDADM = User.Person.PersonID;
                    pTrainer.PersonBOIDPT = trainer.PersonalTrainer.PersonBOIDPT;
                    pTrainer.tPaketPTID = trainer.PersonalTrainer.tPaketPTID;
                    pTrainer.Masa = trainer.PersonalTrainer.trMembership.TotalMonth;

                    pTrainer.trMembership.tMember = null;
                    pTrainer.trMembership.TotalMonth = 0;
                    pTrainer.trMembership.CardStatus = cardId;

                    pTrainer.trMembership.Disc = trainer.DiscNominal;
                    pTrainer.trMembership.DiscType = trainer.DiscType;
                    pTrainer.trMembership.DiscVal = trainer.DiscVal;
                    pTrainer.trMembership.StatusMID = dStatusMember.GetStatusId(EnumStatusMember.PersonalTrainer);
                    //                    pTrainer.trMembership.GenBayar = 0;
                    //
                    //                    pTrainer.PersonBOIDPT = trainer.PersonalTrainer.PersonBOIDPT;
                    //                    pTrainer.SisaJam = trainer.PersonalTrainer.SisaJam;

                    //                    membership.Subtotal = 0;
                    //                    membership.Total = 0;
                    //                    membership.Disc = 0;
                    //                    pTrainer.trMembership = membership;
                    dReg.InsertPT(pTrainer);

                    parentId = pTrainer.trPersonalTrainerID;

                    var payMember = new strPaymentMember()
                    {
                        pembayaranke = 0,
                        statusBayar = true,
                        Tanggal = DateTime.Now,
                        Note = "Transaksi Personal Trainer id " + parentId,
                        trMembershipID = pTrainer.trMembershipID

                    };
                    dPaymentMember.Insert(payMember);

                    foreach (var payWith in trainer.PaymentsWith.Select(payment => new trPaymentWith
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
                        dpaymentWith.Insert(payWith);

                        var strPayMember = payMember;
                        var pay = new strPayment
                        {
                            trPaymentID = strPayMember.trPaymentID,
                            PaymentWithID = payWith.PaymentWithID
                        };

                        dPaymentMember.InsertstrPayment(pay);
                    }
                }

                ts.Complete();
            }
            return RedirectToAction("Index");
        }


        public ActionResult RegisterPTMemberResult([ModelBinder(typeof(MyBinder))] MyCustomRequest requestModel)
        {
            var pt = new DataServicePersonalTrainer().LoadDataMember(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in pt.ListClass
                         select new object[]
                {
                    Convert.ToString(count++),
                    d.tMember.MemberNO,
                    d.tMember.tPerson.PNama,
                    d.tMember.tPerson.PAlamat,
                    (d.MSTglSelesai.Month - DateTime.Now.Month) + 12 * (d.MSTglSelesai.Year - DateTime.Now.Year),
                    d.trMembershipID
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, pt.TotalFilter,
                pt.Total), JsonRequestBehavior.AllowGet);

        }

        public ActionResult RegisterPTResult([ModelBinder(typeof(MyBinder))] MyCustomRequest requestModel)
        {
            if (User.BackOffice == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var pt = new DataServicePersonalTrainer().LoadDataRegistPersonalTrainer(requestModel, User.BackOffice.BOID);
            var count = requestModel.Start + 1;
            var result = from d in pt.ListClass
                         select new object[]
                         {
                             Convert.ToString(count++),
                             d.trMembership.tMember.MemberNO,
                             d.trMembership.tMember.tPerson.PNama,
                             d.trMembership.tMember.tPerson.PAlamat,
                             d.trMembership.MSTglMulai.ToString("dd-MM-yyyy"),
                             d.trMembership.MSTglMulai.AddDays(d.tPaketPT.PPTMasa ?? 0).ToString("dd-MM-yyyy"),
                             d.trPersonalTrainerID,
                             d.trPersonalTrainerID,
                             d.trPersonalTrainerID

                         };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, pt.TotalFilter,
                pt.Total), JsonRequestBehavior.AllowGet);
            //return null;
        }

        public CrystalReportPdfResult PrintRegisterPersonalTrainer(int id)
        {
            string reportPath = Server.MapPath(@"~\bin\Areas\Trainers\Report\PrintoutRegisterPT.rpt");
            var dPersonalTrainer = new DataServicePersonalTrainer();

            var reportSource = (from m in dPersonalTrainer.LoadAllData().Where(m => m.trPersonalTrainerID == id).ToList()
                                select new ViewModelPrintRegisterPersonalTrainer()
                                {
                                    Nama = m.trMembership.tMember.tPerson.PNama,
                                    Alamat = m.trMembership.tMember.tPerson.PAlamat,
                                    Telp = m.trMembership.tMember.tPerson.PTelp,
                                    Handphone = m.trMembership.tMember.tPerson.PHP1,
                                    HargaPerSesi = 0,
                                    JumlahKontrak = (m.trMembership.Total.Value),
                                    JumlahPaket = m.tPaketPT.PPTJam,
                                    Lvl = 1,
                                    NOAnggota = m.trMembership.tMember.MemberNO,
                                    NamaTrainer = m.tUserBackOffice.tPerson.PNama,
                                    NoIdentitas = m.trMembership.tMember.tPerson.PIdentitas,
                                    TanggalCetak = (m.trMembership.MSTglMulai),
                                    TanggalExpired = m.trMembership.MSTglMulai.AddDays((m.tPaketPT.PPTMasa ?? 0))
                                }).First();

            return new CrystalReportPdfResult(reportPath, new[] { reportSource }, null);
        }

        public CrystalReportPdfResult PrintOrPersonalTrainer(int id)
        {
            string reportPath = Server.MapPath(@"~\bin\Areas\Office\Views\Member\Report\PrintoutQR.rpt");
            var dPersonalTrainer = new DataServicePersonalTrainer();

            var reportSource = (from m in dPersonalTrainer.LoadAllData().Where(m => m.trPersonalTrainerID == id).ToList()
                                select new PrintoutQr()
                                {
                                    MemberType = m.trMembership.tMember.tMemberType.MemberType,
                                    Nama = m.trMembership.tMember.tPerson.PNama,
                                    Alamat = m.trMembership.tMember.tPerson.PNama,
                                    TanggalMulai = (m.trMembership.MSTglMulai)
                                }).First();

            return new CrystalReportPdfResult(reportPath, new[] { reportSource }, null);
        }

        [HttpGet]
        public ActionResult GetMemberNo(string memberNo)
        {
            var m = new DataServiceMembership();
            var member = m.GetobjByMemberNo(memberNo);
            var memberpersonIdentity = new
            {
                memberid = member.MemberID,
                nama = member.tMember.tPerson.PNama,
                memberType = member.tMember.tMemberType.MemberType,
                alamat = member.tMember.tPerson.PAlamat,
                usia = DateTime.Now.Year - member.tMember.tPerson.PTglLahir?.Year,
                sisa = ((DateTime.Now.Year - member.MSTglMulai.Year) * 12) + DateTime.Now.Month - member.MSTglMulai.Month

            };
            //Return the data as a jsonp result
            return new Jsonp
            {
                Data = memberpersonIdentity,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}