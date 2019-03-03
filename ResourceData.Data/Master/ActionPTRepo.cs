using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class ActionPTRepo : DataRepoBase<tActionPT>
    {
        protected override tActionPT AddEntity(FitEntity entityContext, tActionPT entity)
        {
            return entityContext.tActionPTs.Add(entity);
        }

        protected override IQueryable<tActionPT> GetEntities(FitEntity entityContext)
        {
            return entityContext.tActionPTs;
        }

        protected override tActionPT GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tActionPTs
                    where e.ActionPTID == id
                    select e).FirstOrDefault();
        }

        protected override tActionPT UpdateEntity(FitEntity entityContext, tActionPT entity)
        {
            return (from e in entityContext.tActionPTs
                    where e.ActionPTID == entity.ActionPTID
                    select e).FirstOrDefault();
        }
    }
}
