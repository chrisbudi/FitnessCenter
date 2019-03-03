using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.Membership
{
    public class PaymentMemberRepo : DataRepoBase<strPaymentMember>
    {
        protected override strPaymentMember AddEntity(FitEntity entityContext, strPaymentMember entity)
        {
            return entityContext.strPaymentMembers.Add(entity);
        }

        protected override IQueryable<strPaymentMember> GetEntities(FitEntity entityContext)
        {
            return entityContext.strPaymentMembers;
        }

        protected override strPaymentMember GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.strPaymentMembers
                    where e.trPaymentID == id
                    select e).FirstOrDefault();
        }

        protected override strPaymentMember UpdateEntity(FitEntity entityContext, strPaymentMember entity)
        {
            return (from e in entityContext.strPaymentMembers
                    where e.trPaymentID == entity.trPaymentID
                    select e).FirstOrDefault();
        }
    }
}
