namespace StockDemo.API.Services
{
    /// <summary>
    /// Builds the localized title/body for low-stock push notifications. Extracted so the
    /// language-selection logic can be unit-tested independently of FCM and the database.
    /// </summary>
    internal static class LowStockMessageBuilder
    {
        /// <summary>Collapses any locale to a supported one; anything but "en" falls back to "vi".</summary>
        public static string NormalizeLocale(string? locale)
        {
            return string.Equals(locale, "en", StringComparison.OrdinalIgnoreCase) ? "en" : "vi";
        }

        public static (string Title, string Body) Build(
            string locale, IReadOnlyList<(int ProductId, string Name, int Current, int Min)> items)
        {
            if (NormalizeLocale(locale) == "en")
            {
                var title = "Low stock alert";
                var body = items.Count == 1
                    ? $"{items[0].Name}: {items[0].Current} left (min {items[0].Min})"
                    : $"{items.Count} products below minimum: " + string.Join(", ", items.Take(3).Select(x => x.Name));
                return (title, body);
            }

            var viTitle = "Cảnh báo tồn thấp";
            var viBody = items.Count == 1
                ? $"{items[0].Name} còn {items[0].Current} (định mức {items[0].Min})"
                : $"{items.Count} sản phẩm dưới định mức: " + string.Join(", ", items.Take(3).Select(x => x.Name));
            return (viTitle, viBody);
        }
    }
}
