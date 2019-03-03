using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class BackOfficeRepo : DataRepoBase<tUserBackOffice>
    {
        protected override tUserBackOffice AddEntity(FitEntity entityContext, tUserBackOffice entity)
        {
            return entityContext.tUserBackOffices.Add(entity);
        }

        protected override IQueryable<tUserBackOffice> GetEntities(FitEntity entityContext)
        {
            return entityContext.tUserBackOffices;
        }

        protected override tUserBackOffice GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tUserBackOffices
                    where e.PersonBOID == id
                    select e).FirstOrDefault();
        }

        protected override tUserBackOffice UpdateEntity(FitEntity entityContext, tUserBackOffice entity)
        {
            return (from e in entityContext.tUserBackOffices
                    where e.PersonBOID == entity.PersonBOID
                    select e).FirstOrDefault();
        }
    }
}
