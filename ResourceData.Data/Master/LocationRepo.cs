using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class LocationRepo : DataRepoBase<tLocFitnessCenter>
    {
        protected override tLocFitnessCenter AddEntity(FitEntity entityContext, tLocFitnessCenter entity)
        {
            return entityContext.tLocFitnessCenters.Add(entity);
        }

        protected override IQueryable<tLocFitnessCenter> GetEntities(FitEntity entityContext)
        {
            return entityContext.tLocFitnessCenters;
        }

        protected override tLocFitnessCenter GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tLocFitnessCenters
                    where e.LocationID == id
                    select e).FirstOrDefault();
        }

        protected override tLocFitnessCenter UpdateEntity(FitEntity entityContext, tLocFitnessCenter entity)
        {
            return (from e in entityContext.tLocFitnessCenters
                    where e.LocationID == entity.LocationID
                    select e).FirstOrDefault();
        }
    }
}
