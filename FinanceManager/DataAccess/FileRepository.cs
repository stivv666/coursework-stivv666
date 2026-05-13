using System;
using System.Text.Json;
using FinanceManager.Domain;

namespace FinanceManager.DataAccess
{
    public class FileRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly string _filePath;
        private List<T> _items;
        public FileRepository(string filePath)
        {
            _filePath = filePath;

            var directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _items = LoadFromFile();
        }
        public IEnumerable<T> GetAll() => _items;

        public void Add(T entity)
        {
            _items.Add(entity);
            SaveToFile();
        }

        public void Update(T entity)
        {
            var index = _items.FindIndex(e => e.Id == entity.Id);
            if (index != -1)
            {
                _items[index] = entity;
                SaveToFile();
            }
        }
        public void Delete(Guid id)
        {
            var item = _items.FirstOrDefault(e => e.Id == id);
            if (item != null)
            {
                _items.Remove(item);
                SaveToFile();
            }
        }
        private List<T> LoadFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        private void SaveToFile()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_items, options);
            File.WriteAllText(_filePath, json);
        }
    }
}