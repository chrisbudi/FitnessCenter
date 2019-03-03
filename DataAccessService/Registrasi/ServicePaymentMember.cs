using System;
using System.Linq;
using DataAccessResource.Master;
using DataAccessResource.Membership;
using DataObjects.Entities;
using DataObjects.Shared;
using Scheme.UOW;
using Services.Class;
using Services.DataTables;
using Services.Extensions;

namespace DataAccessService.Registrasi
{
    public interface IServicePaymentMember
    {
        void Insert(strPaymentMember member);
        strPaymentMember Get(int id, out int paymentNo, out DateTime paymentDate);
        strPaymentMember Get(int id);
        IQueryable<strPaymentMember> Get();
    }


    public class ServicePaymentMember : IServicePaymentMember
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicePaymentMember(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //        public decimal FirstPayment(int paymentid)
        //        {
        //            var repo = _unitOfWork.GetDataRepository<PaymentWithRepo>();
        //            return repo.Get();
        //
        //        }

        public strPaymentMember Get(int id, out int paymentNo, out DateTime paymentDate)
        {
            var repo = _unitOfWork.GetDataRepository<PaymentMemberRepo>();
            var paymentMember = repo.Get(id);
            var members = repo.Get().Where(m => m.statusBayar && m.trMembershipID == paymentMember.trMembershipID);
            paymentNo = members.Count();
            paymentDate = members.OrderByDescending(m => m.pembayaranke)
                .First().Tanggal;

            return paymentMember;
        }

        strPaymentMember IServicePaymentMember.Get(int id)
        {
            var repo = _unitOfWork.GetDataRepository<PaymentMemberRepo>();
            return repo.Get(id);
        }

        IQueryable<strPaymentMember> IServicePaymentMember.Get()
        {
            var repo = _unitOfWork.GetDataRepository<PaymentMemberRepo>();
            return repo.Get();
        }

        public void Insert(strPaymentMember member)
        {
            var repo = _unitOfWork.GetDataRepository<PaymentMemberRepo>();
            switch (member.trPaymentID)
            {
                case 0:
                    repo.Add(member);
                    break;
                default:
                    repo.Update(member);
                    break;
            }
        }
    }
}
