using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataObjects.Entities;

namespace DataAccessService.Registrasi
{
    public class DataServicePaymentMember : DbDataAccessRegistrasi
    {
        private IQueryable<strPaymentMember> _payment { get; set; }

        public DataServicePaymentMember() { }

        public DataServicePaymentMember(IQueryable<strPaymentMember> memberships)
        {
            _payment = memberships;
        }

        public IQueryable<strPaymentMember> LoadAllData()
        {
            return (from p in DbMembership.strPaymentMembers
                    select p);
        }

        public IQueryable<strPayment> LoadAllDataPayment()
        {
            return (from p in DbMembership.strPayments
                    select p);
        }

        public trMembership GetobjByID(int Id)
        {
            var membership = (from p in DbMembership.trMemberships
                              where p.trMembershipID == Id
                              select p).SingleOrDefault();
            return membership;
        }

        public IEnumerable<strPaymentMember> GetPaymentByMembership(int membershipId)
        {
            var payment = (from p in DbMembership.strPaymentMembers
                           where p.trMembershipID == membershipId
                           select p);

            return payment;
        }

        public IEnumerable<strPaymentMember> GetPaymentByPTId(int trMembershipID)
        {
            var payment = (from p in DbMembership.strPaymentMembers
                           where p.trMembershipID == trMembershipID
                           select p);

            return payment;
        }

        public void Insert(strPaymentMember obj)
        {
            DbMembership.strPaymentMembers.Add(obj);
            Save();
        }

        public void InsertstrPayment(strPayment obj)
        {
            DbMembership.strPayments.Add(obj);
            Save();
        }


        public void Update(strPaymentMember obj)
        {
            DbMembership.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public List<strPaymentMember> GetData(string searchTerm, int pageSize, int pageNum)
        {
            _payment = LoadAllData();
            return GetDataQuery(searchTerm).OrderBy(m => m.trMembershipID)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize).ToList();
        }



        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }

        public IQueryable<strPaymentMember> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return _payment.Where(a => a.trMembership_trMembershipID.tMember.MemberNO.Contains(searchTerm));
        }

        public int GetIdByName(string paymentType)
        {
            var membership = (from p in DbMembership.tPaymentTypes
                              where p.NamaType == paymentType
                              select p).Single().PaymentTypeID;
            return membership;
        }
    }
}