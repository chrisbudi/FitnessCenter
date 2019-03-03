using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Fit.Control.DataTables;
using Fit.Control.Helpers;
using Fit.DataAccessService.Master;
using Fit.DataAccessService.Registrasi;
using Fit.Membership.Model;
using Fit.ModelMaster.Model;
using Fit.ViewModel.Registrasi;
using Fit.ViewModel.Registrasi.Activity;
using Fit.ViewModel.Registrasi.CalonMember;
using FitnessCenter.Controllers;

namespace FitnessCenter.Areas.Registrasi.Controllers
{
    public class CalonSewaPTController : FitController
    {
        // GET: Registrasi/CalonMember
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAct(int id = 0)
        {
            if (id == 0)
                return HttpNotFound();

            DataServiceAktivitasSales dsale = new DataServiceAktivitasSales();

            ViewModelCreateActv acv = new ViewModelCreateActv()
            {
                Act = dsale.Create(id),

            };

            return View("CreateActSales", acv);
        }

        [HttpPost]
        public ActionResult CreateSewaPt(ViewModelCreateActv actv, int id)
        {
            if (!ModelState.IsValid)
                return View("CreateActSales", actv);

            //initialize data service
            var dMembership = new DataServiceMembership();
            var dActv = new DataServiceAktivitasSales();
            var dState = new DataServiceMemberState();
            var dStrPayment = new DataServicePaymentMember();
            var dStatus = new DataServiceStatusMember();


            var strPaymentMember = new strPaymentMember();

            var trMemberships = new List<trMembership>();

            using (var ts = new TransactionScope())
            {
                if (actv.Act.MemberStateID == dState.GetMemberstateId(EnumMemberState.Closed))
                {
                    var membership = dMembership.GetobjByID(id);
                    var AggreementID = dMembership.GenerateMembershipAggrementID(User.ActiveLocation);
                    membership.AgreementID = AggreementID;
                    membership.StatusMID = dStatus.GetStatusId(EnumStatusMember.Process);
                    membership.CountMember = actv.CountMember;
                    membership.Subtotal = actv.Payment;
                    membership.Admin = actv.Admin;
                    membership.Prorate = actv.Prorate;
                    membership.Total = actv.TotalAmount;
                    membership.Disc = actv.Discount;
                    membership.seq = 1;
                    dMembership.Update(membership);
                    trMemberships.Add(membership);

                    actv.Act.trMembersipID = membership.trMembersipID;
                    actv.Act.date = DateTime.Now;
                    dActv.Insert(actv.Act);

                    strPaymentMember.MemberTypeID = actv.MemberType;
                    strPaymentMember.Tanggal = DateTime.Now;
                    strPaymentMember.trMembersipID = membership.trMembersipID;
                    dStrPayment.Insert(strPaymentMember);

                    for (var i = 0; i <= actv.CountMember - 2; i++)
                    {
                        var newAggreementID = dMembership.GenerateMembershipAggrementID(User.ActiveLocation);
                        var newMembership = new trMembership()
                        {
                            PersonID = membership.PersonID,
                            AgreementID = newAggreementID,
                            StatusMID = dStatus.GetStatusId(EnumStatusMember.Process),
                            MSTglMulai = DateTime.Now,
                            CountMember = actv.CountMember,
                            LocationID = User.ActiveLocation,
                            Note = "Member Close by Index",
                            BOID = User.BackOffice.BOID,
                            seq = 2
                        };
                        dMembership.Insert(newMembership);
                        membership.trMembersipID = dMembership.GetLastMemberShipID();

                        actv.Act.trMembersipID = membership.trMembersipID;
                        actv.Act.date = DateTime.Now;
                        dActv.Insert(actv.Act);

                        strPaymentMember.MemberTypeID = actv.MemberType;
                        strPaymentMember.Tanggal = DateTime.Now;
                        strPaymentMember.trMembersipID = membership.trMembersipID;
                        dStrPayment.Insert(strPaymentMember);
                    }
                }
                else
                {
                    actv.Act.date = DateTime.Now;
                    dActv.Insert(actv.Act);
                }

                ts.Complete();
            }
            return RedirectToAction("Index");
        }



        public ActionResult CreateCPT()
        {
            var dState = new DataServiceMemberState();
            //var pm = new CalonMemberViewModel()
            //{
            //    Personmember = dState.GetMemberstateId(EnumMemberState.Calon)
            //};

            return View();
        }

        [HttpPost]
        public ActionResult CreateCalonPT(tPerson member, ViewModelCalonMember personViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(personViewModel);
            }

            //initialize data service
            var dPerson = new DataServicePerson();
            var dMembership = new DataServiceMembership();
            //var dMember = new DataServiceMember();
            var dActv = new DataServiceAktivitasSales();
            var dState = new DataServiceMemberState();
            var dSales = new DataServiceSalesAction();
            var dStatus = new DataServiceStatusMember();
            var dStrPayment = new DataServicePaymentMember();

            var strPaymentMember = new strPaymentMember();

            var trMemberships = new List<trMembership>();


            var membership = new trMembership
            {
                BOID = User.BackOffice.BOID,
                LocationID = User.ActiveLocation,
                MSTglSelesai = DateTime.Now,
                MSTglMulai = DateTime.Now,
                Subtotal = personViewModel.MemberType.Payment,
                Admin = personViewModel.MemberType.Admin,
                Prorate = personViewModel.MemberType.Prorate,
                Total = personViewModel.MemberType.TotalAmount,
                Disc = personViewModel.MemberType.Discount,
                Note = "member awal",
                StatusMID = new DataServiceStatusMember().GetStatusId(EnumStatusMember.Calon),
                PersonID = member.PersonID,
                CountMember = personViewModel.MemberType.CountMember,
                seq = 1
            };

            if (personViewModel.MemberType.MemberStatus == EnumMemberState.Closed.ToString("F"))
            {
                membership.StatusMID = dStatus.GetStatusId(EnumStatusMember.Process);


                strPaymentMember.MemberTypeID = personViewModel.MemberType.MemberType;
                strPaymentMember.Tanggal = DateTime.Now;
            }

            using (var ts = new TransactionScope())
            {
                dPerson.Insert(member);
                var personId = dPerson.GetLastPersonId();


                if (personViewModel.MemberType.MemberStatus == EnumMemberState.Closed.ToString("F"))
                {
                    for (var i = 0; i <= personViewModel.MemberType.CountMember - 1; i++)
                    {

                        var AggreementID = dMembership.GenerateMembershipAggrementID(User.ActiveLocation);
                        membership.PersonID = personId;
                        membership.AgreementID = AggreementID;
                        dMembership.Insert(membership);

                        membership.Total = null;
                        membership.Admin = null;
                        membership.Disc = null;
                        membership.Prorate = null;
                        membership.Subtotal = null;

                        membership.trMembersipID = dMembership.GetLastMemberShipID();
                        membership.seq = 2;

                        var aktv = new strAktivitasSale
                        {
                            MemberStateID = dState.GetMemberstateId(EnumMemberState.Closed),
                            date = DateTime.Now,
                            Note = "",
                            SalesActionID = dSales.GetSalesActionId(EnumSalesAction.First),
                            trMembersipID = membership.trMembersipID,
                        };
                        dActv.Insert(aktv);

                        strPaymentMember.trMembersipID = membership.trMembersipID;
                        dStrPayment.Insert(strPaymentMember);
                    }
                }
                else
                {
                    membership.PersonID = personId;
                    dMembership.Insert(membership);
                }

                ts.Complete();
            }


            return RedirectToAction("Index");
        }

        public ActionResult PostCPT(int status, int act, int membershipId, int memberCount, int memberType)
        {
            var dSales = new DataServiceAktivitasSales();
            var dState = new DataServiceMemberState();
            var dStatus = new DataServiceStatusMember();
            var dMember = new DataServiceMembership();

            var trmembership = dMember.GetobjByID(membershipId);

            var aktv = new strAktivitasSale
            {
                MemberStateID = memberType,
                date = DateTime.Now,
                Note = "",
                SalesActionID = act,
                trMembersipID = membershipId
            };

            using (var ts = new TransactionScope())
            {
                if (status == dState.GetMemberstateId(EnumMemberState.Closed))
                {
                    var membership = dMember.GetobjByID(membershipId);
                    membership.StatusMID = dStatus.GetStatusId(EnumStatusMember.Member);
                    dMember.Update(membership);
                }
                dSales.Insert(aktv);
                ts.Complete();
            }
            return RedirectToAction("Index");
        }

        public ActionResult CPTResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var member = new DataServiceMember().LoadData(requestModel, User.ActiveLocation);
            int count = requestModel.Start + 1;
            var result = from d in member.ListClass
                         let tgllahir = d.tPerson.PTglLahir
                         where tgllahir != null
                         select new[]
                {
                    Convert.ToString(count++),
                    d.MemberID,
                    d.tPerson.PAlamat,
                    d.tPerson.PKota,
                    tgllahir.Value.ToString("dd-MM-yyyy"),
                    d.tPerson.PNama,
                    d.tPerson.PGender,
                    d.MMulai.ToString("dd-MM-yyyy"),
                    d.PersonID.ToString()
                };

            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, member.TotalFilter,
                member.Total), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ActResult([ModelBinder(typeof(MyBinder))] MyCustomRequest requestModel)
        {
            var cm = new DataServiceAktivitasSales().ActLoad(request: requestModel);
            var count = requestModel.Start + 1;
            var result = from d in cm.ListClass
                         select new[]
                {
                    d.date.HasValue? d.date.Value.ToString("dd-MM-yyyy"):DateTime.MinValue.ToString("dd-MM-yyyy"),
                    d.SalesActions.ActionName,
                    d.MemberStates.MemberStateName,
                    d.Note
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cm.TotalFilter,
                cm.Total), JsonRequestBehavior.AllowGet);
        }
    }
}