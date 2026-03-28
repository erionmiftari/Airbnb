using System.Collections.Generic;

namespace Airbnb.Data
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T? GetById(int id);
        void Add(T item);
        bool Update(T item);
        bool Delete(int id);
        void Save();
    }
}