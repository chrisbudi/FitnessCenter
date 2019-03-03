using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class PaymentTypeRepo : DataRepoBase<tPaymentType>
    {
        protected override tPaymentType AddEntity(FitEntity entityContext, tPaymentType entity)
        {
            return entityContext.tPaymentTypes.Add(entity);
        }

        protected override IQueryable<tPaymentType> GetEntities(FitEntity entityContext)
        {
            return entityContext.tPaymentTypes;
        }

        protected override tPaymentType GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tPaymentTypes
                    where e.PaymentTypeID == id
                    select e).FirstOrDefault();
        }

        protected override tPaymentType UpdateEntity(FitEntity entityContext, tPaymentType entity)
        {
            return (from e in entityContext.tPaymentTypes
                    where e.PaymentTypeID == entity.PaymentTypeID
                    select e).FirstOrDefault();
        }
    }
}
