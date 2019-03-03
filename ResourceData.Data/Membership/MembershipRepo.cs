using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.Membership
{
    public class MembershipRepo : DataRepoBase<trMembership>
    {
        protected override trMembership AddEntity(FitEntity entityContext, trMembership entity)
        {
            return entityContext.trMemberships.Add(entity);
        }

        protected override IQueryable<trMembership> GetEntities(FitEntity entityContext)
        {
            return entityContext.trMemberships;
        }

        protected override trMembership GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.trMemberships
                    where e.trMembershipID == id
                    select e).FirstOrDefault();
        }

        protected override trMembership UpdateEntity(FitEntity entityContext, trMembership entity)
        {
            return (from e in entityContext.trMemberships
                    where e.trMembershipID == entity.trMembershipID
                    select e).FirstOrDefault();
        }
    }
}
