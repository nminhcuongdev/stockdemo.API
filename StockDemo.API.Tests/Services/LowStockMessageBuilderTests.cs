using StockDemo.API.Services;
using Xunit;

namespace StockDemo.API.Tests.Services
{
    public class LowStockMessageBuilderTests
    {
        [Theory]
        [InlineData("en", "en")]
        [InlineData("EN", "en")]
        [InlineData("En", "en")]
        [InlineData("vi", "vi")]
        [InlineData("fr", "vi")]     // unsupported -> default
        [InlineData("", "vi")]
        [InlineData(null, "vi")]
        public void NormalizeLocale_collapses_to_supported_locale(string? input, string expected)
        {
            Assert.Equal(expected, LowStockMessageBuilder.NormalizeLocale(input));
        }

        private static List<(int, string, int, int)> OneItem() =>
            new() { (1, "Laptop", 3, 10) };

        private static List<(int, string, int, int)> ManyItems() =>
            new()
            {
                (1, "Laptop", 3, 10),
                (2, "Mouse", 1, 5),
                (3, "Keyboard", 0, 20),
                (4, "Monitor", 2, 8),
            };

        [Fact]
        public void Build_english_single_item()
        {
            var (title, body) = LowStockMessageBuilder.Build("en", OneItem());

            Assert.Equal("Low stock alert", title);
            Assert.Equal("Laptop: 3 left (min 10)", body);
        }

        [Fact]
        public void Build_vietnamese_single_item()
        {
            var (title, body) = LowStockMessageBuilder.Build("vi", OneItem());

            Assert.Equal("Cảnh báo tồn thấp", title);
            Assert.Equal("Laptop còn 3 (định mức 10)", body);
        }

        [Fact]
        public void Build_english_multiple_items_lists_first_three()
        {
            var (title, body) = LowStockMessageBuilder.Build("en", ManyItems());

            Assert.Equal("Low stock alert", title);
            Assert.Equal("4 products below minimum: Laptop, Mouse, Keyboard", body);
            Assert.DoesNotContain("Monitor", body); // capped at three names
        }

        [Fact]
        public void Build_vietnamese_multiple_items_lists_first_three()
        {
            var (title, body) = LowStockMessageBuilder.Build("vi", ManyItems());

            Assert.Equal("Cảnh báo tồn thấp", title);
            Assert.Equal("4 sản phẩm dưới định mức: Laptop, Mouse, Keyboard", body);
        }

        [Fact]
        public void Build_treats_unknown_locale_as_vietnamese()
        {
            var (title, _) = LowStockMessageBuilder.Build("de", OneItem());

            Assert.Equal("Cảnh báo tồn thấp", title);
        }
    }
}
