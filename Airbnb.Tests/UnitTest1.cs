using Airbnb.Data;
using Airbnb.Models;
using Airbnb.Services;

namespace Airbnb.Tests;

public class ListingServiceTests
{
    [Fact]
    public void Add_ValidListing_ReturnsSuccess()
    {
        var repo = new InMemoryListingRepository();
        var service = new ListingService(repo);

        var added = service.Add("Laptop", 1200, 1);

        Assert.NotNull(added);
        Assert.Equal("Laptop", added.Title);
        Assert.Single(repo.GetAll());
    }

    [Fact]
    public void Add_EmptyTitle_ThrowsArgumentException()
    {
        var repo = new InMemoryListingRepository();
        var service = new ListingService(repo);

        Assert.Throws<ArgumentException>(() => service.Add("", 100, 1));
    }

    [Fact]
    public void List_WithTitleFilter_ExistingAndNonExisting_BehavesCorrectly()
    {
        var repo = new InMemoryListingRepository();
        repo.Add(new Listing { Id = 1, Title = "Laptop", Price = 500, OwnerId = 1 });
        repo.Add(new Listing { Id = 2, Title = "Phone", Price = 300, OwnerId = 1 });

        var service = new ListingService(repo);

        var existing = service.List(titleContains: "Laptop");
        var nonExisting = service.List(titleContains: "Nuk-Ekziston");

        Assert.Single(existing);
        Assert.Equal("Laptop", existing[0].Title);
        Assert.Empty(nonExisting);
    }

    [Fact]
    public void List_MinPriceGreaterThanMax_ThrowsArgumentException()
    {
        var repo = new InMemoryListingRepository();
        var service = new ListingService(repo);

        Assert.Throws<ArgumentException>(() => service.List(minPrice: 100, maxPrice: 50));
    }

    [Fact]
    public void Add_OwnerIdZero_ThrowsArgumentException()
    {
        var repo = new InMemoryListingRepository();
        var service = new ListingService(repo);

        Assert.Throws<ArgumentException>(() => service.Add("Valid", 99, 0));
    }

    [Fact]
    public void GetStatistics_ReturnsCorrectTotals()
    {
        var repo = new InMemoryListingRepository();
        repo.Add(new Listing { Id = 1, Title = "A", Price = 10, OwnerId = 1 });
        repo.Add(new Listing { Id = 2, Title = "B", Price = 30, OwnerId = 2 });

        var service = new ListingService(repo);
        var stats = service.GetStatistics();

        Assert.Equal(2, stats.Count);
        Assert.Equal(40, stats.TotalPrice);
        Assert.Equal(20, stats.AveragePrice);
        Assert.Equal(10, stats.MinPrice);
        Assert.Equal(30, stats.MaxPrice);
    }
}

internal class InMemoryListingRepository : IRepository<Listing>
{
    private readonly List<Listing> _items = new();

    public List<Listing> GetAll() => _items.Select(x => new Listing
    {
        Id = x.Id,
        Title = x.Title,
        Price = x.Price,
        OwnerId = x.OwnerId
    }).ToList();

    public Listing? GetById(int id) => _items.FirstOrDefault(x => x.Id == id);

    public void Add(Listing item) => _items.Add(item);

    public bool Update(Listing item)
    {
        var idx = _items.FindIndex(x => x.Id == item.Id);
        if (idx < 0) return false;
        _items[idx] = item;
        return true;
    }

    public bool Delete(int id) => _items.RemoveAll(x => x.Id == id) > 0;

    public void Save()
    {
    }
}
