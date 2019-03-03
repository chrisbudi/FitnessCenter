using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class MemberStateRepo : DataRepoBase<tMemberState>
    {
        protected override tMemberState AddEntity(FitEntity entityContext, tMemberState entity)
        {
            return entityContext.tMemberStates.Add(entity);
        }

        protected override IQueryable<tMemberState> GetEntities(FitEntity entityContext)
        {
            return entityContext.tMemberStates;
        }

        protected override tMemberState GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tMemberStates
                    where e.MemberStateID == id
                    select e).FirstOrDefault();
        }

        protected override tMemberState UpdateEntity(FitEntity entityContext, tMemberState entity)
        {
            return (from e in entityContext.tMemberStates
                    where e.MemberStateID == entity.MemberStateID
                    select e).FirstOrDefault();
        }
    }
}
