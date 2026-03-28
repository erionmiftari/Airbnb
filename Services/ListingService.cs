using System;
using System.Collections.Generic;
using System.Linq;
using Airbnb.Data;
using Airbnb.Models;

namespace Airbnb.Services
{
    public class ListingService
    {
        private readonly IRepository<Listing> _repo;

        public ListingService(IRepository<Listing> repo)
        {
            _repo = repo;
        }

        public List<Listing> List(string? titleContains = null, double? minPrice = null, double? maxPrice = null)
        {
            var items = _repo.GetAll().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(titleContains))
            {
                items = items.Where(x => (x.Title ?? "").Contains(titleContains, StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue)
                items = items.Where(x => x.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                items = items.Where(x => x.Price <= maxPrice.Value);

            return items.OrderBy(x => x.Id).ToList();
        }

        public Listing? GetById(int id) => _repo.GetById(id);

        public Listing Add(string title, double price, int ownerId)
        {
            title = title?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Emri/Title nuk mund të jetë bosh.");
            if (title.Contains(","))
                throw new ArgumentException("Title nuk lejohet të ketë presje (,).");
            if (price <= 0)
                throw new ArgumentException("Çmimi duhet të jetë > 0.");

            var all = _repo.GetAll();
            int nextId = all.Count == 0 ? 1 : all.Max(x => x.Id) + 1;

            var item = new Listing
            {
                Id = nextId,
                Title = title,
                Price = price,
                OwnerId = ownerId
            };

            _repo.Add(item);
            _repo.Save();
            return item;
        }

        public bool Update(int id, string title, double price, int ownerId)
        {
            title = title?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Emri/Title nuk mund të jetë bosh.");
            if (title.Contains(","))
                throw new ArgumentException("Title nuk lejohet të ketë presje (,).");
            if (price <= 0)
                throw new ArgumentException("Çmimi duhet të jetë > 0.");

            var ok = _repo.Update(new Listing
            {
                Id = id,
                Title = title,
                Price = price,
                OwnerId = ownerId
            });

            if (ok) _repo.Save();
            return ok;
        }

        public bool Delete(int id)
        {
            var ok = _repo.Delete(id);
            if (ok) _repo.Save();
            return ok;
        }
    }
}

