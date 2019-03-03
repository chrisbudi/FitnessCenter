using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class StatusInOutRepo : DataRepoBase<tTypeStatusCinCout>
    {
        protected override tTypeStatusCinCout AddEntity(FitEntity entityContext, tTypeStatusCinCout entity)
        {
            return entityContext.tTypeStatusCinCouts.Add(entity);
        }

        protected override IQueryable<tTypeStatusCinCout> GetEntities(FitEntity entityContext)
        {
            return entityContext.tTypeStatusCinCouts;
        }

        protected override tTypeStatusCinCout GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tTypeStatusCinCouts
                    where e.TypeStatusInOut == id
                    select e).FirstOrDefault();
        }

        protected override tTypeStatusCinCout UpdateEntity(FitEntity entityContext, tTypeStatusCinCout entity)
        {
            return (from e in entityContext.tTypeStatusCinCouts
                    where e.TypeStatusInOut == entity.TypeStatusInOut
                    select e).FirstOrDefault();
        }
    }
}
