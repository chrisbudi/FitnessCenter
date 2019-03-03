using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.PT
{
    public class PersonalTrainerRepo : DataRepoBase<trPersonalTrainer>
    {
        protected override trPersonalTrainer AddEntity(FitEntity entityContext, trPersonalTrainer entity)
        {
            return entityContext.trPersonalTrainers.Add(entity);
        }

        protected override IQueryable<trPersonalTrainer> GetEntities(FitEntity entityContext)
        {
            return entityContext.trPersonalTrainers;
        }

        protected override trPersonalTrainer GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.trPersonalTrainers
                    where e.trPersonalTrainerID == id
                    select e).FirstOrDefault();
        }

        protected override trPersonalTrainer UpdateEntity(FitEntity entityContext, trPersonalTrainer entity)
        {
            return (from e in entityContext.trPersonalTrainers
                    where e.trPersonalTrainerID == entity.trPersonalTrainerID
                    select e).FirstOrDefault();
        }
    }
}
