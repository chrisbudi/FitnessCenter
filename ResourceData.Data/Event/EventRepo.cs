using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.Event
{
    public class EventRepo : DataRepoBase<tEvent>
    {
        protected override tEvent AddEntity(FitEntity entityContext, tEvent entity)
        {
            return entityContext.tEvents.Add(entity);
        }

        protected override IQueryable<tEvent> GetEntities(FitEntity entityContext)
        {
            return entityContext.tEvents;
        }

        protected override tEvent GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tEvents
                    where e.EvId == id
                    select e).FirstOrDefault();
        }

        protected override tEvent UpdateEntity(FitEntity entityContext, tEvent entity)
        {
            return (from e in entityContext.tEvents
                    where e.EvId == entity.EvId
                    select e).FirstOrDefault();
        }
    }
}
