using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class CardStatusRepo : DataRepoBase<tCardStatu>
    {
        protected override tCardStatu AddEntity(FitEntity entityContext, tCardStatu entity)
        {
            return entityContext.tCardStatus.Add(entity);
        }

        protected override IQueryable<tCardStatu> GetEntities(FitEntity entityContext)
        {
            return entityContext.tCardStatus;
        }

        protected override tCardStatu GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tCardStatus
                    where e.CardStatusID == id
                    select e).FirstOrDefault();
        }

        protected override tCardStatu UpdateEntity(FitEntity entityContext, tCardStatu entity)
        {
            return (from e in entityContext.tCardStatus
                    where e.CardStatusID == entity.CardStatusID
                    select e).FirstOrDefault();
        }
    }
}
