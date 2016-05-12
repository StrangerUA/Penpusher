using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Penpusher.DAL
{
    class Repository<T> : IRepository<T> where T : class
    {
        private PenpusherDatabaseEntities EntitiesContext = new PenpusherDatabaseEntities();
        internal DbSet<T> dbSet;

        public Repository()
        {
            this.dbSet = EntitiesContext.Set<T>();
        }

        public IEnumerable<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public T Add(T entity)
        {
            dbSet.Add(entity);
            EntitiesContext.SaveChanges();
            return entity;
        }

        public T Edit<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}