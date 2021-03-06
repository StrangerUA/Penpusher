using System.Collections.Generic;

namespace Penpusher
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T Add(T entity);

        T Edit(T entity);

        T Delete(int id);

        T GetById(int id);
    }
}