using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.BackOffice
{
    public class ActivityBORepo : DataRepoBase<trCinCout>
    {
        protected override trCinCout AddEntity(FitEntity entityContext, trCinCout entity)
        {
            return entityContext.trCinCouts.Add(entity);
        }

        protected override IQueryable<trCinCout> GetEntities(FitEntity entityContext)
        {
            return entityContext.trCinCouts;
        }

        protected override trCinCout GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.trCinCouts
                    where e.CinCoutID == id
                    select e).FirstOrDefault();
        }

        protected override trCinCout UpdateEntity(FitEntity entityContext, trCinCout entity)
        {
            return (from e in entityContext.trCinCouts
                    where e.CinCoutID == entity.CinCoutID
                    select e).FirstOrDefault();
        }
    }
}
