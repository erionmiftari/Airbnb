using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Airbnb.Models;

namespace Airbnb.Data
{
    public class FileRepository : IRepository<Listing>
    {
        private readonly string _filePath;
        private readonly List<Listing> _items = new List<Listing>();

        public FileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<Listing> GetAll()
        {
            LoadFromCsv();
            return _items.ToList();
        }

        public Listing? GetById(int id)
        {
            LoadFromCsv();
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Listing item)
        {
            LoadFromCsv();
            _items.Add(item);
        }

        public bool Update(Listing item)
        {
            LoadFromCsv();
            int idx = _items.FindIndex(x => x.Id == item.Id);
            if (idx < 0) return false;
            _items[idx] = item;
            return true;
        }

        public bool Delete(int id)
        {
            LoadFromCsv();
            int removed = _items.RemoveAll(x => x.Id == id);
            return removed > 0;
        }

        public void Save()
        {
            try
            {
                EnsureDirectory();
                using var sw = new StreamWriter(_filePath, false);
                sw.WriteLine("Id,Title,Price,OwnerId");
                foreach (var x in _items.OrderBy(i => i.Id))
                {
                    sw.WriteLine($"{x.Id},{Escape(x.Title)},{x.Price.ToString(CultureInfo.InvariantCulture)},{x.OwnerId}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Gabim gjatë ruajtjes së file-it: {ex.Message}");
            }
        }

        private void LoadFromCsv()
        {
            _items.Clear();
            try
            {
                if (!File.Exists(_filePath))
                {
                    EnsureDirectory();
                    File.WriteAllText(_filePath, "Id,Title,Price,OwnerId" + Environment.NewLine);
                    Console.WriteLine("File nuk u gjet, po krijoj file të ri...");
                    return;
                }

                var lines = File.ReadAllLines(_filePath);
                foreach (var raw in lines.Skip(1)) // skip header
                {
                    if (string.IsNullOrWhiteSpace(raw)) continue;

                    var parts = SplitCsvLine(raw);
                    if (parts.Count < 4) continue;

                    if (!int.TryParse(parts[0], out int id)) continue;
                    string title = Unescape(parts[1]);
                    if (!double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double price)) continue;
                    if (!int.TryParse(parts[3], out int ownerId)) continue;

                    _items.Add(new Listing { Id = id, Title = title, Price = price, OwnerId = ownerId });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Gabim gjatë leximit të file-it: {ex.Message}");
            }
        }

        private void EnsureDirectory()
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);
        }

        private static string Escape(string value)
        {
            value ??= "";
            if (value.Contains('"') || value.Contains(',') || value.Contains('\n') || value.Contains('\r'))
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            return value;
        }

        private static string Unescape(string value)
        {
            if (value == null) return "";
            value = value.Trim();
            if (value.Length >= 2 && value.StartsWith("\"") && value.EndsWith("\""))
            {
                value = value.Substring(1, value.Length - 2);
                value = value.Replace("\"\"", "\"");
            }
            return value;
        }

        private static List<string> SplitCsvLine(string line)
        {
            var result = new List<string>();
            if (line == null) return result;

            var current = new System.Text.StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            result.Add(current.ToString());
            return result;
        }
    }
}