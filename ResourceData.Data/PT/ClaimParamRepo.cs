using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.PT
{
    public class ActionClaimParamRepo : DataRepoBase<StrActionKlaimParam>
    {
        protected override StrActionKlaimParam AddEntity(FitEntity entityContext, StrActionKlaimParam entity)
        {
            return entityContext.StrActionKlaimParams.Add(entity);
        }

        protected override IQueryable<StrActionKlaimParam> GetEntities(FitEntity entityContext)
        {
            return entityContext.StrActionKlaimParams;
        }

        protected override StrActionKlaimParam GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.StrActionKlaimParams
                    where e.IdPar == id
                    select e).FirstOrDefault();
        }

        protected override StrActionKlaimParam UpdateEntity(FitEntity entityContext, StrActionKlaimParam entity)
        {
            return (from e in entityContext.StrActionKlaimParams
                    where e.IdPar == entity.IdPar
                    select e).FirstOrDefault();
        }
    }
}
