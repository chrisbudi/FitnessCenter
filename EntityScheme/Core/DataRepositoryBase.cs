using System;
using System.Data.Entity;
using System.Linq;
using Scheme.Interface;

namespace Scheme.Core
{
    public abstract class DataRepositoryBase<T, U> : IDataRepository<T>
       where T : class, new()
       where U : DbContext, new()
    {
        protected abstract T AddEntity(U entityContext, T entity);

        protected abstract T UpdateEntity(U entityContext, T entity);

        protected abstract IQueryable<T> GetEntities(U entityContext);

        protected abstract T GetEntity(U entityContext, int id);

        public T Add(T entity)
        {
            using (U entityContext = new U())
            {
                T addedEntity = AddEntity(entityContext, entity);
                entityContext.SaveChanges();
                return addedEntity;
            }
        }

        public void Remove(T entity)
        {
            using (U entityContext = new U())
            {
                entityContext.Entry<T>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            using (U entityContext = new U())
            {
                T entity = GetEntity(entityContext, id);
                entityContext.Entry<T>(entity).State = EntityState.Deleted;
                entityContext.SaveChanges();
            }
        }

        public T Update(T entity)
        {
            using (U entityContext = new U())
            {
                T existingEntity = UpdateEntity(entityContext, entity);
                //entityContext.Entry<T>(entity).State = EntityState.Modified;
                SimpleMapper.PropertyMap(entity, existingEntity);

                entityContext.SaveChanges();
                return existingEntity;
            }
        }

        public IQueryable<T> Get()
        {
            U entityContext = new U();
            return (GetEntities(entityContext));
        }

        public T Get(int id)
        {
            U entityContext = new U();
            return GetEntity(entityContext, id);
        }
    }
}