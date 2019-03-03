using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class RuangKelasRepo : DataRepoBase<tRuangKela>
    {
        protected override tRuangKela AddEntity(FitEntity entityContext, tRuangKela entity)
        {
            return entityContext.tRuangKelas.Add(entity);
        }

        protected override IQueryable<tRuangKela> GetEntities(FitEntity entityContext)
        {
            return entityContext.tRuangKelas;
        }

        protected override tRuangKela GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tRuangKelas
                    where e.RuangKelasID == id
                    select e).FirstOrDefault();
        }

        protected override tRuangKela UpdateEntity(FitEntity entityContext, tRuangKela entity)
        {
            return (from e in entityContext.tRuangKelas
                    where e.RuangKelasID == entity.RuangKelasID
                    select e).FirstOrDefault();
        }
    }
}
