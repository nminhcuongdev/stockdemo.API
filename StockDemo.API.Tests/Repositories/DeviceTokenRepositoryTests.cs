using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Models.Domain;
using StockDemo.API.Repositories.DeviceTokenRepository;
using Xunit;

namespace StockDemo.API.Tests.Repositories
{
    public class DeviceTokenRepositoryTests
    {
        private static StockDemoDbContext NewContext()
        {
            var options = new DbContextOptionsBuilder<StockDemoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new StockDemoDbContext(options);
        }

        [Fact]
        public async Task AddAsync_then_GetByTokenAsync_round_trips()
        {
            using var ctx = NewContext();
            var repo = new DeviceTokenRepository(ctx);

            await repo.AddAsync(new DeviceToken { Token = "abc", UserId = 1, Platform = "android", Locale = "en" });

            var found = await repo.GetByTokenAsync("abc");
            Assert.NotNull(found);
            Assert.Equal(1, found!.UserId);
            Assert.Equal("en", found.Locale);
        }

        [Fact]
        public async Task GetByTokenAsync_returns_null_when_missing()
        {
            using var ctx = NewContext();
            var repo = new DeviceTokenRepository(ctx);

            var found = await repo.GetByTokenAsync("does-not-exist");

            Assert.Null(found);
        }

        [Fact]
        public async Task RemoveByTokensAsync_deletes_only_the_matching_tokens()
        {
            using var ctx = NewContext();
            var repo = new DeviceTokenRepository(ctx);
            await repo.AddAsync(new DeviceToken { Token = "keep", UserId = 1 });
            await repo.AddAsync(new DeviceToken { Token = "drop-1", UserId = 2 });
            await repo.AddAsync(new DeviceToken { Token = "drop-2", UserId = 3 });

            await repo.RemoveByTokensAsync(new[] { "drop-1", "drop-2" });

            Assert.Null(await repo.GetByTokenAsync("drop-1"));
            Assert.Null(await repo.GetByTokenAsync("drop-2"));
            Assert.NotNull(await repo.GetByTokenAsync("keep"));
        }

        [Fact]
        public async Task RemoveByTokensAsync_is_a_noop_for_unknown_tokens()
        {
            using var ctx = NewContext();
            var repo = new DeviceTokenRepository(ctx);
            await repo.AddAsync(new DeviceToken { Token = "keep", UserId = 1 });

            await repo.RemoveByTokensAsync(new[] { "unknown" });

            Assert.NotNull(await repo.GetByTokenAsync("keep"));
        }

        [Fact]
        public async Task GetAllAsync_returns_every_registered_device()
        {
            using var ctx = NewContext();
            var repo = new DeviceTokenRepository(ctx);
            await repo.AddAsync(new DeviceToken { Token = "a", UserId = 1, Locale = "vi" });
            await repo.AddAsync(new DeviceToken { Token = "b", UserId = 2, Locale = "en" });

            var all = await repo.GetAllAsync();

            Assert.Equal(2, all.Count());
        }
    }
}
