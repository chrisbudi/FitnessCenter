using System.Linq;
using DataObjects.Entities;

namespace DataAccessService.PT
{
    public interface IServiceActionParam
    {
        void Insert(StrActionKlaimParam member);
        StrActionKlaimParam Get(int id);
        IQueryable<StrActionKlaimParam> Get();
    }
}