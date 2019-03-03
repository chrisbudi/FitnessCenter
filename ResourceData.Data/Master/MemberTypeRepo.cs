using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class MemberTypeRepo : DataRepoBase<tMemberType>
    {
        protected override tMemberType AddEntity(FitEntity entityContext, tMemberType entity)
        {
            return entityContext.tMemberTypes.Add(entity);
        }

        protected override IQueryable<tMemberType> GetEntities(FitEntity entityContext)
        {
            return entityContext.tMemberTypes;
        }

        protected override tMemberType GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tMemberTypes
                    where e.MemberTypeID == id
                    select e).FirstOrDefault();
        }

        protected override tMemberType UpdateEntity(FitEntity entityContext, tMemberType entity)
        {
            return (from e in entityContext.tMemberTypes
                    where e.MemberTypeID == entity.MemberTypeID
                    select e).FirstOrDefault();
        }
    }
}
