using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class PaketPTRepo : DataRepoBase<tPaketPT>
    {
        protected override tPaketPT AddEntity(FitEntity entityContext, tPaketPT entity)
        {
            return entityContext.tPaketPTs.Add(entity);
        }

        protected override IQueryable<tPaketPT> GetEntities(FitEntity entityContext)
        {
            return entityContext.tPaketPTs;
        }

        protected override tPaketPT GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tPaketPTs
                    where e.tPaketPTID == id
                    select e).FirstOrDefault();
        }

        protected override tPaketPT UpdateEntity(FitEntity entityContext, tPaketPT entity)
        {
            return (from e in entityContext.tPaketPTs
                    where e.tPaketPTID == entity.tPaketPTID
                    select e).FirstOrDefault();
        }
    }
}
