using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.Kelas
{
    public class PlanKelasRepo : DataRepoBase<trPlanKela>
    {
        protected override trPlanKela AddEntity(FitEntity entityContext, trPlanKela entity)
        {
            return entityContext.trPlanKelas.Add(entity);
        }

        protected override IQueryable<trPlanKela> GetEntities(FitEntity entityContext)
        {
            return entityContext.trPlanKelas;
        }

        protected override trPlanKela GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.trPlanKelas
                    where e.PlanKelasID == id
                    select e).FirstOrDefault();
        }

        protected override trPlanKela UpdateEntity(FitEntity entityContext, trPlanKela entity)
        {
            return (from e in entityContext.trPlanKelas
                    where e.PlanKelasID == entity.PlanKelasID
                    select e).FirstOrDefault();
        }
    }
}
