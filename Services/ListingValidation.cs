using System;

namespace Airbnb.Services
{
    /// <summary>
    /// Rregulla të përbashkëta të validimit për listings — një vend për logjikën e biznesit,
    /// pa duplikim midis Add dhe Update.
    /// </summary>
    internal static class ListingValidation
    {
        public static void ValidateTitle(string? title)
        {
            var t = title?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(t))
                throw new ArgumentException("Emri/Title nuk mund të jetë bosh.");
            if (t.Contains(","))
                throw new ArgumentException("Title nuk lejohet të ketë presje (,).");
        }

        public static void ValidatePrice(double price)
        {
            if (price <= 0)
                throw new ArgumentException("Çmimi duhet të jetë > 0.");
        }

        public static void ValidateOwnerId(int ownerId)
        {
            if (ownerId <= 0)
                throw new ArgumentException("OwnerId duhet të jetë numër pozitiv (> 0).");
        }

        public static void ValidatePriceFilterRange(double? minPrice, double? maxPrice)
        {
            if (minPrice.HasValue && maxPrice.HasValue && minPrice.Value > maxPrice.Value)
                throw new ArgumentException("Minimumi i çmimit nuk mund të jetë më i madh se maksimumi.");
        }
    }
}
