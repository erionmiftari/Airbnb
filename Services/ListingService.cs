using System;
using System.Collections.Generic;
using System.Linq;
using Airbnb.Data;
using Airbnb.Models;

namespace Airbnb.Services
{
    public enum ListingSortOption
    {
        IdAsc = 1,
        TitleAsc = 2,
        PriceAsc = 3,
        PriceDesc = 4
    }

    public class ListingStatistics
    {
        public int Count { get; set; }
        public double TotalPrice { get; set; }
        public double AveragePrice { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
    }

    public class ListingService
    {
        private readonly IRepository<Listing> _repo;

        public ListingService(IRepository<Listing> repo)
        {
            _repo = repo;
        }

        public List<Listing> List(
            string? titleContains = null,
            double? minPrice = null,
            double? maxPrice = null,
            ListingSortOption sort = ListingSortOption.IdAsc)
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

            items = sort switch
            {
                ListingSortOption.TitleAsc => items.OrderBy(x => x.Title),
                ListingSortOption.PriceAsc => items.OrderBy(x => x.Price),
                ListingSortOption.PriceDesc => items.OrderByDescending(x => x.Price),
                _ => items.OrderBy(x => x.Id)
            };

            return items.ToList();
        }

        public ListingStatistics GetStatistics(string? titleContains = null, double? minPrice = null, double? maxPrice = null)
        {
            var items = List(titleContains, minPrice, maxPrice);
            if (items.Count == 0)
            {
                return new ListingStatistics
                {
                    Count = 0,
                    TotalPrice = 0,
                    AveragePrice = 0,
                    MinPrice = 0,
                    MaxPrice = 0
                };
            }

            return new ListingStatistics
            {
                Count = items.Count,
                TotalPrice = items.Sum(x => x.Price),
                AveragePrice = items.Average(x => x.Price),
                MinPrice = items.Min(x => x.Price),
                MaxPrice = items.Max(x => x.Price)
            };
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

        public bool TryAdd(string title, string priceRaw, string ownerIdRaw, out Listing? added, out string message)
        {
            added = null;
            message = "";
            try
            {
                if (!double.TryParse(priceRaw, out var price))
                {
                    message = "Ju lutem shkruani numër valid për çmimin.";
                    return false;
                }

                if (!int.TryParse(ownerIdRaw, out var ownerId))
                {
                    message = "Ju lutem shkruani numër valid për OwnerId.";
                    return false;
                }

                added = Add(title, price, ownerId);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
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

        public bool TryUpdate(int id, string title, string priceRaw, string ownerIdRaw, out string message)
        {
            message = "";
            try
            {
                if (!double.TryParse(priceRaw, out var price))
                {
                    message = "Ju lutem shkruani numër valid për çmimin.";
                    return false;
                }

                if (!int.TryParse(ownerIdRaw, out var ownerId))
                {
                    message = "Ju lutem shkruani numër valid për OwnerId.";
                    return false;
                }

                bool ok = Update(id, title, price, ownerId);
                message = ok ? "U përditësua me sukses." : "Itemi nuk u gjet.";
                return ok;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}

