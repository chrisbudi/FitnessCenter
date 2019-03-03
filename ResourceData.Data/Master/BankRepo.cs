using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class BankRepo : DataRepoBase<tBank>
    {
        protected override tBank AddEntity(FitEntity entityContext, tBank entity)
        {
            return entityContext.tBanks.Add(entity);
        }

        protected override IQueryable<tBank> GetEntities(FitEntity entityContext)
        {
            return entityContext.tBanks;
        }

        protected override tBank GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tBanks
                    where e.BankID == id
                    select e).FirstOrDefault();
        }

        protected override tBank UpdateEntity(FitEntity entityContext, tBank entity)
        {
            return (from e in entityContext.tBanks
                    where e.BankID == entity.BankID
                    select e).FirstOrDefault();
        }
    }
}
