using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.Membership
{
    public class PaymentWithRepo : DataRepoBase<trPaymentWith>
    {
        protected override trPaymentWith AddEntity(FitEntity entityContext, trPaymentWith entity)
        {
            return entityContext.trPaymentWiths.Add(entity);
        }

        protected override IQueryable<trPaymentWith> GetEntities(FitEntity entityContext)
        {
            return entityContext.trPaymentWiths;
        }

        protected override trPaymentWith GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.trPaymentWiths
                    where e.PaymentWithID == id
                    select e).FirstOrDefault();
        }

        protected override trPaymentWith UpdateEntity(FitEntity entityContext, trPaymentWith entity)
        {
            return (from e in entityContext.trPaymentWiths
                    where e.PaymentWithID == entity.PaymentWithID
                    select e).FirstOrDefault();
        }
    }
}
