using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.Membership
{
    public class ActivitySalesRepo : DataRepoBase<strAktivitasSale>
    {
        protected override strAktivitasSale AddEntity(FitEntity entityContext, strAktivitasSale entity)
        {
            return entityContext.strAktivitasSales.Add(entity);
        }

        protected override IQueryable<strAktivitasSale> GetEntities(FitEntity entityContext)
        {
            return entityContext.strAktivitasSales;
        }

        protected override strAktivitasSale GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.strAktivitasSales
                    where e.AktivitasSalesID == id
                    select e).FirstOrDefault();
        }

        protected override strAktivitasSale UpdateEntity(FitEntity entityContext, strAktivitasSale entity)
        {
            return (from e in entityContext.strAktivitasSales
                    where e.AktivitasSalesID == entity.AktivitasSalesID
                    select e).FirstOrDefault();
        }
    }
}
