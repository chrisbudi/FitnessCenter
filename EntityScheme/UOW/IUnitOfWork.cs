using Scheme.Interface;

namespace Scheme.UOW
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Save method.
        /// </summary>
        //        void Save();
        T GetDataRepository<T>() where T : IDataRepository, new();
    }
}