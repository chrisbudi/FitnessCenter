using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.PT
{
    public class PlanPTRepo : DataRepoBase<trPlanAktifitasPT>
    {
        protected override trPlanAktifitasPT AddEntity(FitEntity entityContext, trPlanAktifitasPT entity)
        {
            return entityContext.trPlanAktifitasPTs.Add(entity);
        }

        protected override IQueryable<trPlanAktifitasPT> GetEntities(FitEntity entityContext)
        {
            return entityContext.trPlanAktifitasPTs;
        }

        protected override trPlanAktifitasPT GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.trPlanAktifitasPTs
                    where e.PlanAktifitasPTID == id
                    select e).FirstOrDefault();
        }

        protected override trPlanAktifitasPT UpdateEntity(FitEntity entityContext, trPlanAktifitasPT entity)
        {
            return (from e in entityContext.trPlanAktifitasPTs
                    where e.PlanAktifitasPTID == entity.PlanAktifitasPTID
                    select e).FirstOrDefault();
        }
    }
}
