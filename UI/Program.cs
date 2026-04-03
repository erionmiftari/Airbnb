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
            Console.WriteLine("6. Statistika");
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
                else if (input == "6") UiStats(service);
                else if (input == "0") break;
                else Console.WriteLine("Opsion i pavlefshëm.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gabim: {ex.Message}");
            }
        }
    }

    static void UiList(ListingService service)
    {
        try
        {
            Console.Write("Filter title (Enter për skip): ");
            string? title = Console.ReadLine();

            Console.Write("Min price (Enter për skip): ");
            string? minRaw = Console.ReadLine();

            Console.Write("Max price (Enter për skip): ");
            string? maxRaw = Console.ReadLine();

            if (!TryParseNullableDouble(minRaw, out var min))
            {
                Console.WriteLine("Ju lutem shkruani numër valid për minimumin.");
                return;
            }

            if (!TryParseNullableDouble(maxRaw, out var max))
            {
                Console.WriteLine("Ju lutem shkruani numër valid për maksimumin.");
                return;
            }

            Console.WriteLine("Sortimi: 1=ID, 2=Title A-Z, 3=Price rritës, 4=Price zbritës");
            Console.Write("Zgjedhja e sortimit: ");
            string? sortRaw = Console.ReadLine();
            var sort = ParseSort(sortRaw);

            var items = service.List(
                titleContains: string.IsNullOrWhiteSpace(title) ? null : title,
                minPrice: min,
                maxPrice: max,
                sort: sort);

            if (items.Count == 0)
            {
                Console.WriteLine("Nuk ka rezultate.");
                return;
            }

            foreach (var x in items)
                Console.WriteLine($"ID: {x.Id} | Title: {x.Title} | Price: {x.Price} | OwnerId: {x.OwnerId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gabim në listim: {ex.Message}");
        }
    }

    static void UiGetById(ListingService service)
    {
        try
        {
            Console.Write("ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ju lutem shkruani numër valid.");
                return;
            }

            var x = service.GetById(id);
            if (x == null) Console.WriteLine("Itemi nuk u gjet.");
            else Console.WriteLine($"ID: {x.Id} | Title: {x.Title} | Price: {x.Price} | OwnerId: {x.OwnerId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gabim në kërkim: {ex.Message}");
        }
    }

    static void UiAdd(ListingService service)
    {
        try
        {
            Console.Write("Title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Price: ");
            string priceRaw = Console.ReadLine() ?? "";

            Console.Write("OwnerId: ");
            string ownerIdRaw = Console.ReadLine() ?? "";

            bool ok = service.TryAdd(title, priceRaw, ownerIdRaw, out var added, out var message);
            if (!ok)
            {
                Console.WriteLine(message);
                return;
            }

            Console.WriteLine($"U shtua me sukses. ID e re: {added!.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gabim gjatë shtimit: {ex.Message}");
        }
    }

    static void UiUpdate(ListingService service)
    {
        try
        {
            Console.Write("ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ju lutem shkruani numër valid.");
                return;
            }

            Console.Write("Title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Price: ");
            string priceRaw = Console.ReadLine() ?? "";

            Console.Write("OwnerId: ");
            string ownerIdRaw = Console.ReadLine() ?? "";

            service.TryUpdate(id, title, priceRaw, ownerIdRaw, out string message);
            Console.WriteLine(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gabim gjatë përditësimit: {ex.Message}");
        }
    }

    static void UiDelete(ListingService service)
    {
        try
        {
            Console.Write("ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Ju lutem shkruani numër valid.");
                return;
            }

            bool ok = service.Delete(id);
            Console.WriteLine(ok ? "U fshi me sukses." : "Itemi nuk u gjet.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gabim gjatë fshirjes: {ex.Message}");
        }
    }

    static void UiStats(ListingService service)
    {
        try
        {
            Console.Write("Filter title (Enter për skip): ");
            string? title = Console.ReadLine();

            Console.Write("Min price (Enter për skip): ");
            string? minRaw = Console.ReadLine();
            if (!TryParseNullableDouble(minRaw, out var min))
            {
                Console.WriteLine("Ju lutem shkruani numër valid për minimumin.");
                return;
            }

            Console.Write("Max price (Enter për skip): ");
            string? maxRaw = Console.ReadLine();
            if (!TryParseNullableDouble(maxRaw, out var max))
            {
                Console.WriteLine("Ju lutem shkruani numër valid për maksimumin.");
                return;
            }

            var stats = service.GetStatistics(
                string.IsNullOrWhiteSpace(title) ? null : title,
                min,
                max);

            Console.WriteLine("--- Statistika ---");
            Console.WriteLine($"Numri i itemave: {stats.Count}");
            Console.WriteLine($"Totali i çmimeve: {stats.TotalPrice}");
            Console.WriteLine($"Mesatarja: {stats.AveragePrice}");
            Console.WriteLine($"Minimumi: {stats.MinPrice}");
            Console.WriteLine($"Maksimumi: {stats.MaxPrice}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Gabim në statistika: {ex.Message}");
        }
    }

    static bool TryParseNullableDouble(string? input, out double? value)
    {
        value = null;
        if (string.IsNullOrWhiteSpace(input)) return true;
        if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed))
        {
            value = parsed;
            return true;
        }
        return false;
    }

    static ListingSortOption ParseSort(string? sortRaw)
    {
        if (!int.TryParse(sortRaw, out int sortValue))
            return ListingSortOption.IdAsc;
        if (!Enum.IsDefined(typeof(ListingSortOption), sortValue))
            return ListingSortOption.IdAsc;
        return (ListingSortOption)sortValue;
    }
}