using System.Collections.Generic;
using System.Linq;
using DataObjects.Entities;

namespace DataAccessService.Registrasi
{
    public class DataServicePaymentWith : DbDataAccessRegistrasi
    {
        private IQueryable<trPaymentWith> _payment { get; set; }

        public DataServicePaymentWith() { }

        public DataServicePaymentWith(IQueryable<trPaymentWith> memberships)
        {
            _payment = memberships;
        }

        public IQueryable<trPaymentWith> LoadAllData()
        {
            return (from p in DbMembership.trPaymentWiths
                    select p);
        }

        public trMembership GetobjByID(int Id)
        {
            var membership = (from p in DbMembership.trMemberships
                              where p.trMembershipID == Id
                              select p).SingleOrDefault();
            return membership;
        }

        public IEnumerable<trPaymentWith> GetPaymentByStrPayment(int strPaymentID)
        {
            var payment = (from p in DbMembership.trPaymentWiths
                           where p.strPayments.Single().StrPaymentID == strPaymentID
                           select p);
            return payment;
        }


        public void Insert(trPaymentWith obj)
        {
            DbMembership.trPaymentWiths.Add(obj);
            Save();
        }

        public void Update(trPaymentWith obj)
        {
            //var member = GetobjByID(obj.trMembershipID);
            //member.StatusMID = obj.StatusMID;
            //Save();
        }

        public List<trPaymentWith> GetData(string searchTerm, int pageSize, int pageNum)
        {
            _payment = LoadAllData();
            //return GetDataQuery(searchTerm).OrderBy(m => m.trMembershipID)
            //    .Skip(pageSize * (pageNum - 1))
            //    .Take(pageSize).ToList();
            return null;
        }



        public int GetDataCount(string searchTerm)
        {
            return GetDataQuery(searchTerm).Count();
        }

        public IQueryable<strPaymentMember> GetDataQuery(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return null;
            //return _payment.Where(a => a.trMembership.MemberID.Contains(searchTerm));
        }
    }
}