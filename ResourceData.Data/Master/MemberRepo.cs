using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class MemberRepo : DataRepoBase<tMember>
    {
        protected override tMember AddEntity(FitEntity entityContext, tMember entity)
        {
            return entityContext.tMembers.Add(entity);
        }

        protected override IQueryable<tMember> GetEntities(FitEntity entityContext)
        {
            return entityContext.tMembers;
        }

        protected override tMember GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tMembers
                    where e.MemberID == id
                    select e).FirstOrDefault();
        }

        protected override tMember UpdateEntity(FitEntity entityContext, tMember entity)
        {
            return (from e in entityContext.tMembers
                    where e.MemberID == entity.MemberID
                    select e).FirstOrDefault();
        }
    }
}
