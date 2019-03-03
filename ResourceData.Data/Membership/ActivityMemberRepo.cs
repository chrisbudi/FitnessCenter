using System.Linq;
using DataObjects.Context;
using DataObjects.Entities;

namespace DataAccessResource.Membership
{
    public class ActivityMemberRepo : DataRepoBase<trAktifitasMember>
    {
        protected override trAktifitasMember AddEntity(FitEntity entityContext, trAktifitasMember entity)
        {
            return entityContext.trAktifitasMembers.Add(entity);
        }

        protected override IQueryable<trAktifitasMember> GetEntities(FitEntity entityContext)
        {
            return entityContext.trAktifitasMembers;
        }

        protected override trAktifitasMember GetEntity(FitEntity entityContext, int id)
        {
            return (from e in entityContext.trAktifitasMembers
                    where e.AktifitasMemberID == id
                    select e).FirstOrDefault();
        }

        protected override trAktifitasMember UpdateEntity(FitEntity entityContext, trAktifitasMember entity)
        {
            return (from e in entityContext.trAktifitasMembers
                    where e.AktifitasMemberID == entity.AktifitasMemberID
                    select e).FirstOrDefault();
        }
    }
}
