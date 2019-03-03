using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class KelasRepo : DataRepoBase<tKela>
    {
        protected override tKela AddEntity(FitEntity entityContext, tKela entity)
        {
            return entityContext.tKelas.Add(entity);
        }

        protected override IQueryable<tKela> GetEntities(FitEntity entityContext)
        {
            return entityContext.tKelas;
        }

        protected override tKela GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tKelas
                    where e.KelasID == id
                    select e).FirstOrDefault();
        }

        protected override tKela UpdateEntity(FitEntity entityContext, tKela entity)
        {
            return (from e in entityContext.tKelas
                    where e.KelasID == entity.KelasID
                    select e).FirstOrDefault();
        }
    }
}
