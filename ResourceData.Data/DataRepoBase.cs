using Scheme.Core;
using DataObjects.Context;

namespace DataAccessResource
{
    public abstract class DataRepoBase<T> : DataRepositoryBase<T, FitEntity>
        where T : class, new()
    {
    }
}
