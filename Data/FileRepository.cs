using System.Collections.Generic;
using System.IO;

namespace Airbnb.Data
{
    public class FileRepository<T> : IRepository<T>
    {
        private List<T> items = new List<T>();

        public List<T> GetAll()
        {
            return items;
        }

        public T GetById(int id)
        {
            return items[id];
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Save()
        {
            File.WriteAllText("data.txt", "data saved");
        }
    }
}