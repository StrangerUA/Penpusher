using System.Collections.Generic;

namespace Penpusher
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Add(T entity);
        void Edit(T entity);
        void Delete(int id);
        T GetById(int id);
    }
}