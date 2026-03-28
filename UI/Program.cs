using System;
using System.Globalization;
using System.IO;
using Airbnb.Data;
using Airbnb.Services;

class Program
{
    static void Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "listings.csv");
        var repo = new FileRepository(dataPath);
        var service = new ListingService(repo);

        while (true)
        {
            Console.WriteLine("\n--- LISTINGS MENU ---");
            Console.WriteLine("1. Listo (opsionale: filter title/min/max)");
            Console.WriteLine("2. Gjej sipas ID");
            Console.WriteLine("3. Shto Listing");
            Console.WriteLine("4. Update Listing");
            Console.WriteLine("5. Delete Listing");
            Console.WriteLine("0. Exit");
            Console.Write("Zgjedhja: ");

            string? input = Console.ReadLine();
            if (input == null) break;

            try
            {
                if (input == "1") UiList(service);
                else if (input == "2") UiGetById(service);
                else if (input == "3") UiAdd(service);
                else if (input == "4") UiUpdate(service);
                else if (input == "5") UiDelete(service);
                else if (input == "0") break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gabim: {ex.Message}");
            }
        }
    }

    static void UiList(ListingService service)
    {
        Console.Write("Filter title (Enter për skip): ");
        string? title = Console.ReadLine();

        Console.Write("Min price (Enter për skip): ");
        string? minRaw = Console.ReadLine();

        Console.Write("Max price (Enter për skip): ");
        string? maxRaw = Console.ReadLine();

        double? min = double.TryParse(minRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out var mn) ? mn : null;
        double? max = double.TryParse(maxRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out var mx) ? mx : null;

        var items = service.List(titleContains: string.IsNullOrWhiteSpace(title) ? null : title, minPrice: min, maxPrice: max);
        if (items.Count == 0)
        {
            Console.WriteLine("Nuk ka rezultate.");
            return;
        }

        foreach (var x in items)
            Console.WriteLine($"ID: {x.Id} | Title: {x.Title} | Price: {x.Price} | OwnerId: {x.OwnerId}");
    }

    static void UiGetById(ListingService service)
    {
        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID jo valide.");
            return;
        }

        var x = service.GetById(id);
        if (x == null) Console.WriteLine("Nuk u gjet.");
        else Console.WriteLine($"ID: {x.Id} | Title: {x.Title} | Price: {x.Price} | OwnerId: {x.OwnerId}");
    }

    static void UiAdd(ListingService service)
    {
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Price: ");
        string priceRaw = Console.ReadLine() ?? "";
        if (!double.TryParse(priceRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out double price))
        {
            Console.WriteLine("Price jo valide.");
            return;
        }

        Console.Write("OwnerId: ");
        if (!int.TryParse(Console.ReadLine(), out int ownerId))
        {
            Console.WriteLine("OwnerId jo valide.");
            return;
        }

        var added = service.Add(title, price, ownerId);
        Console.WriteLine($"U shtua me sukses. ID e re: {added.Id}");
    }

    static void UiUpdate(ListingService service)
    {
        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID jo valide.");
            return;
        }

        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Price: ");
        string priceRaw = Console.ReadLine() ?? "";
        if (!double.TryParse(priceRaw, NumberStyles.Float, CultureInfo.InvariantCulture, out double price))
        {
            Console.WriteLine("Price jo valide.");
            return;
        }

        Console.Write("OwnerId: ");
        if (!int.TryParse(Console.ReadLine(), out int ownerId))
        {
            Console.WriteLine("OwnerId jo valide.");
            return;
        }

        bool ok = service.Update(id, title, price, ownerId);
        Console.WriteLine(ok ? "U përditësua me sukses." : "Nuk u gjet ID për update.");
    }

    static void UiDelete(ListingService service)
    {
        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID jo valide.");
            return;
        }

        bool ok = service.Delete(id);
        Console.WriteLine(ok ? "U fshi me sukses." : "Nuk u gjet ID për delete.");
    }
}