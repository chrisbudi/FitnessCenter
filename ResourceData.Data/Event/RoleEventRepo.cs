using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Event
{
    public class RoleEventRepo : DataRepoBase<tRoleEvent>
    {
        protected override tRoleEvent AddEntity(FitEntity entityContext, tRoleEvent entity)
        {
            return entityContext.tRoleEvents.Add(entity);
        }

        protected override IQueryable<tRoleEvent> GetEntities(FitEntity entityContext)
        {
            return entityContext.tRoleEvents;
        }

        protected override tRoleEvent GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tRoleEvents
                    where e.EvRoleId == id
                    select e).FirstOrDefault();
        }

        protected override tRoleEvent UpdateEntity(FitEntity entityContext, tRoleEvent entity)
        {
            return (from e in entityContext.tRoleEvents
                    where e.EvRoleId == entity.EvRoleId
                    select e).FirstOrDefault();
        }
    }
}
