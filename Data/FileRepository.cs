using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Airbnb.Data
{
    public class FileRepository<T> : IRepository<T>
    {
        private List<T> items = new List<T>();
        private readonly string _filePath;

        public FileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> GetAll() => items;

        public T GetById(int id) => items[id];

        public void Add(T item) => items.Add(item);

        public void Save()
        {
            string json = JsonSerializer.Serialize(items);
            File.WriteAllText(_filePath, json);
        }
    }
}