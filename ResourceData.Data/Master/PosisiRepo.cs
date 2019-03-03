using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class PosisiRepo : DataRepoBase<tPosisi>
    {
        protected override tPosisi AddEntity(FitEntity entityContext, tPosisi entity)
        {
            return entityContext.tPosisis.Add(entity);
        }

        protected override IQueryable<tPosisi> GetEntities(FitEntity entityContext)
        {
            return entityContext.tPosisis;
        }

        protected override tPosisi GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tPosisis
                    where e.PosisiID == id
                    select e).FirstOrDefault();
        }

        protected override tPosisi UpdateEntity(FitEntity entityContext, tPosisi entity)
        {
            return (from e in entityContext.tPosisis
                    where e.PosisiID == entity.PosisiID
                    select e).FirstOrDefault();
        }
    }
}
