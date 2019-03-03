using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using DataAccessService.Master;
using DataAccessService.Registrasi;
using DataObjects.Entities;
using FitnessCenter.Controllers;
using Services.DataTables;
using Services.Helpers;
using ViewModel.Membership.Activity;
using ViewModel.Membership.CalonMember;

namespace FitnessCenter.Areas.Registrasi.Controllers
{
    public class CalonMemberController : FitController
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
        public ActionResult CreateAct(ViewModelCreateActv actv, int id)
        {
            if (!ModelState.IsValid)
                return View("CreateActSales", actv);

            //initialize data service
            var dMembership = new DataServiceMembership();
            var dActv = new DataServiceAktivitasSales();
            var dState = new DataServiceMemberState();
            var dStrPayment = new DataServicePaymentMember();
            var dStatus = new DataServiceStatusMember();

            //initialize object
            //var tMember = new tMember();
            //var strlocMember = new strLocMember();
            var strPaymentMember = new strPaymentMember();

            var trMemberships = new List<trMembership>();


            using (var ts = new TransactionScope())
            {
                if (actv.MemberStatus == EnumMemberState.Closed.ToString())
                {
                    var membership = dMembership.GetobjByID(id);
                    var AggreementID = dMembership.GenerateMembershipAggrementId(User.ActiveLocation);
                    membership.AgreementID = AggreementID;
                    membership.StatusMID = dStatus.GetStatusId(EnumStatusMember.InProcessMembership);
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
                    actv.Act.MemberStateID = dState.GetMemberstateId(EnumMemberState.Member);
                    dActv.Insert(actv.Act);

                    strPaymentMember.MemberTypeID = actv.MemberType;
                    strPaymentMember.Tanggal = DateTime.Now;
                    strPaymentMember.trMembersipID = membership.trMembersipID;
                    dStrPayment.Insert(strPaymentMember);
                    var parrentMembershipId = membership.trMembersipID;

                    for (var i = 0; i <= actv.CountMember - 2; i++)
                    {
                        var newAggreementId = dMembership.GenerateMembershipAggrementId(User.ActiveLocation);
                        var newMembership = new trMembership()
                        {
                            PersonID = membership.PersonID,
                            AgreementID = newAggreementId,
                            StatusMID = dStatus.GetStatusId(EnumStatusMember.InProcessMembership),
                            MSTglMulai = DateTime.Now,
                            CountMember = actv.CountMember,
                            LocationID = User.ActiveLocation,
                            Note = "Member Close by Index",
                            BOIDAdm = User.BackOffice.BOID,
                            seq = 2 + i,
                            ParentID = parrentMembershipId
                        };

                        dMembership.Insert(newMembership);
                        membership.trMembersipID = dMembership.GetLastMemberShipId();

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
                    actv.Act.MemberStateID = dState.GetMemberstateId(EnumMemberState.Calon);
                    actv.Act.date = DateTime.Now;
                    dActv.Insert(actv.Act);
                }

                ts.Complete();
            }
            return RedirectToAction("Index");
        }



        public ActionResult CreateCm()
        {
            //            var dState = new DataServiceMemberState();
            //var pm = new PersonViewModel()
            //{
            //    MemberStatus = dState.GetMemberstateId(EnumMemberState.Calon)
            //};

            return View();
        }

        [HttpPost]
        public ActionResult CreateCm(ViewModelCalonMember personViewModel)
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

            //initialize object
            //var tMember = new tMember();
            //var strlocMember = new strLocMember();
            var strPaymentMember = new strPaymentMember();

            //var trMemberships = new List<trMembership>();

            if (dMembership.GetobjByPersonID(personViewModel.Person.PersonID) != null) return null;

            var membership = new trMembership
            {
                BOIDAdm = User.BackOffice.BOID,
                LocationID = User.ActiveLocation,
                MSTglSelesai = DateTime.Now,
                MSTglMulai = DateTime.Now,
                Subtotal = personViewModel.MemberType.Payment,
                Admin = personViewModel.MemberType.Admin,
                Prorate = personViewModel.MemberType.Prorate,
                Total = personViewModel.MemberType.TotalAmount,
                Disc = personViewModel.MemberType.Discount,
                Note = "member awal",
                StatusMID = new DataServiceStatusMember().GetStatusId(EnumStatusMember.InProcessMembership),
                PersonID = personViewModel.Person.PersonID,
                CountMember = personViewModel.MemberType.CountMember,
                seq = 1
            };


            if (personViewModel.MemberType.MemberStatus == EnumMemberState.Closed.ToString("F"))
            {
                membership.StatusMID = dStatus.GetStatusId(EnumStatusMember.InProcessMembership);
                strPaymentMember.MemberTypeID = personViewModel.MemberType.MemberType;
                strPaymentMember.Tanggal = DateTime.Now;
            }

            using (var ts = new TransactionScope())
            {
                var parrentMembershipId = 0;
                dPerson.Insert(personViewModel.Person);
                var personId = dPerson.GetLastPersonId();

                var aggreementId = dMembership.GenerateMembershipAggrementId(User.ActiveLocation);
                membership.PersonID = personId;
                membership.AgreementID = aggreementId;
                dMembership.Insert(membership);
                parrentMembershipId = membership.trMembersipID;


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

                if (personViewModel.MemberType.MemberStatus == EnumMemberState.Closed.ToString("F"))
                {
                    for (var i = 0; i <= personViewModel.MemberType.CountMember - 2; i++)
                    {

                        aggreementId = dMembership.GenerateMembershipAggrementId(User.ActiveLocation);

                        var newMembership = new trMembership()
                        {
                            PersonID = membership.PersonID,
                            AgreementID = aggreementId,
                            StatusMID = dStatus.GetStatusId(EnumStatusMember.InProcessMembership),
                            MSTglMulai = DateTime.Now,
                            CountMember = personViewModel.MemberType.CountMember,
                            LocationID = User.ActiveLocation,
                            Note = "Member Close by Transaction",
                            BOIDAdm = User.BackOffice.BOID,
                            seq = 2 + i,
                            ParentID = parrentMembershipId
                        };


                        dMembership.Insert(newMembership);
                        //                        membership.Total = null;
                        //                        membership.Admin = null;
                        //                        membership.Disc = null;
                        //                        membership.Prorate = null;
                        //                        membership.Subtotal = null;
                        //
                        //                        membership.trMembersipID = dMembership.GetLastMemberShipID();
                        //                        membership.seq = 2;

                        aktv = new strAktivitasSale
                        {
                            MemberStateID = dState.GetMemberstateId(EnumMemberState.Closed),
                            date = DateTime.Now,
                            Note = "",
                            SalesActionID = dSales.GetSalesActionId(EnumSalesAction.First),
                            trMembersipID = newMembership.trMembersipID,
                        };
                        dActv.Insert(aktv);

                        strPaymentMember.trMembersipID = newMembership.trMembersipID;
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

        public ActionResult PostCm(int status, int act, int membershipId, int memberCount, int memberType)
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
                    membership.StatusMID = dStatus.GetStatusId(EnumStatusMember.Membership);
                    dMember.Update(membership);
                }
                dSales.Insert(aktv);
                ts.Complete();
            }
            return RedirectToAction("Index");
        }

        public ActionResult CMResult([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            var cm = new DataServiceMembership().CMLoad(requestModel);
            var count = requestModel.Start + 1;
            var result = from d in cm.ListClass
                         select new[]
                {
                    Convert.ToString(count++),
                    d.tPerson.PNama,
                    d.tPerson.PGender,
                    d.tPerson.PAlamat,
                    d.tPerson.PTelp,
                    d.tPerson.PHP1,
                    Convert.ToString(d.trMembersipID)
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cm.TotalFilter,
                cm.Total), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActResult([ModelBinder(typeof(MyBinder))] MyCustomRequest requestModel)
        {
            var cm = new DataServiceAktivitasSales().ActLoad(request: requestModel);
            var count = requestModel.Start + 1;
            var result = from d in cm.ListClass
                         select new[]
                {
                    d.date.HasValue? d.date.Value.ToString("dd-MM-yyyy"):DateTime.MinValue.ToString("dd-MM-yyyy"),
                    d.tSalesAction.ActionName,
                    d.tMemberState.MemberStateName,
                    d.Note
                };
            return Json(new DataTablesResponse(
                requestModel.Draw,
                result, cm.TotalFilter,
                cm.Total), JsonRequestBehavior.AllowGet);
        }
    }
}