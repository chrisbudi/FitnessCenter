#region Using Namespaces...

using Scheme.Interface;

#endregion

namespace Scheme.UOW
{
    public class UnitOfWork : IUnitOfWork
    {

        #region Private member variables...

        //        private readonly EntityContext _context = null;
        //       private DataRepositoryBase<T> _userRepository;
        #endregion

        //        public UnitOfWork()
        //        {
        //            _context = new EntityContext();
        //        }

        #region Public Repository Creation properties...


        //        public TRepository GetDataRepository<TRepository>()
        //          where TRepository : class, new()
        //        {
        //            return new TRepository();
        //        }


        /// <summary>
        /// Get/Set Property for product repository.
        /// </summary>
        /// 
        //        T GetDataRepository<T>() where T : IDataRepository
        //        {
        //            return new T();
        //        }

        public TRepository GetDataRepository<TRepository>()
        where TRepository : IDataRepository, new()
        {
            return new TRepository();
        }

        #endregion

        #region Public member methods...

        //        public void Save()
        //        {
        //            try
        //            {
        //                _context.SaveChanges();
        //            }
        //            catch (DbEntityValidationException e)
        //            {
        //
        //                var outputLines = new List<string>();
        //                foreach (var eve in e.EntityValidationErrors)
        //                {
        //                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
        //                    foreach (var ve in eve.ValidationErrors)
        //                    {
        //                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
        //                    }
        //                }
        //                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);
        //
        //                throw e;
        //            }
        //
        //        }





        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        #endregion

        //        public void Dispose()
        //        {
        //            Dispose(true);
        //            GC.SuppressFinalize(this);
        //        }
        #endregion
    }
}

//class ItemFactory<T> where T : new()
//{
//    public T GetNewItem()
//    {
//        return new T();
//    }
//}\