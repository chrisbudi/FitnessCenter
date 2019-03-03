using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Event
{
    public class PersonEventRepo : DataRepoBase<trPersonEvent>
    {
        protected override trPersonEvent AddEntity(FitEntity entityContext, trPersonEvent entity)
        {
            return entityContext.trPersonEvents.Add(entity);
        }

        protected override IQueryable<trPersonEvent> GetEntities(FitEntity entityContext)
        {
            return entityContext.trPersonEvents;
        }

        protected override trPersonEvent GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.trPersonEvents
                    where e.EvRoleId == id
                    select e).FirstOrDefault();
        }

        protected override trPersonEvent UpdateEntity(FitEntity entityContext, trPersonEvent entity)
        {
            return (from e in entityContext.trPersonEvents
                    where e.EvRoleId == entity.EvRoleId
                    select e).FirstOrDefault();
        }
    }
}
