using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class SalesActionRepo : DataRepoBase<tSalesAction>
    {
        protected override tSalesAction AddEntity(FitEntity entityContext, tSalesAction entity)
        {
            return entityContext.tSalesActions.Add(entity);
        }

        protected override IQueryable<tSalesAction> GetEntities(FitEntity entityContext)
        {
            return entityContext.tSalesActions;
        }

        protected override tSalesAction GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tSalesActions
                    where e.SalesActionID == id
                    select e).FirstOrDefault();
        }

        protected override tSalesAction UpdateEntity(FitEntity entityContext, tSalesAction entity)
        {
            return (from e in entityContext.tSalesActions
                    where e.SalesActionID == entity.SalesActionID
                    select e).FirstOrDefault();
        }
    }
}
