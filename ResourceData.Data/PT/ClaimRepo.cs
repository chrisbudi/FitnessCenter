using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.PT
{
    public class ClaimRepo : DataRepoBase<strKlaimPT>
    {
        protected override strKlaimPT AddEntity(FitEntity entityContext, strKlaimPT entity)
        {
            return entityContext.strKlaimPTs.Add(entity);
        }

        protected override IQueryable<strKlaimPT> GetEntities(FitEntity entityContext)
        {
            return entityContext.strKlaimPTs;
        }

        protected override strKlaimPT GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.strKlaimPTs
                    where e.ClaimPTID == id
                    select e).FirstOrDefault();
        }

        protected override strKlaimPT UpdateEntity(FitEntity entityContext, strKlaimPT entity)
        {
            return (from e in entityContext.strKlaimPTs
                    where e.ClaimPTID == entity.ClaimPTID
                    select e).FirstOrDefault();
        }
    }
}
