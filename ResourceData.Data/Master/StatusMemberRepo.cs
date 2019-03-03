using System.Linq;
using DataObjects.Entities;
using DataObjects.Context;

namespace DataAccessResource.Master
{
    public class StatusMemberRepo : DataRepoBase<tStatusMember>
    {
        protected override tStatusMember AddEntity(FitEntity entityContext, tStatusMember entity)
        {
            return entityContext.tStatusMembers.Add(entity);
        }

        protected override IQueryable<tStatusMember> GetEntities(FitEntity entityContext)
        {
            return entityContext.tStatusMembers;
        }

        protected override tStatusMember GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.tStatusMembers
                    where e.StatusMID == id
                    select e).FirstOrDefault();
        }

        protected override tStatusMember UpdateEntity(FitEntity entityContext, tStatusMember entity)
        {
            return (from e in entityContext.tStatusMembers
                    where e.StatusMID == entity.StatusMID
                    select e).FirstOrDefault();
        }
    }
}
