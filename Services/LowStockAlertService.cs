using Microsoft.EntityFrameworkCore;
using StockDemo.API.Data;
using StockDemo.API.Repositories.DeviceTokenRepository;

namespace StockDemo.API.Services
{
    /// <summary>
    /// Detects products that have crossed below their reorder level and sends a single
    /// push notification per product (tracked in-memory) until they recover. Shared by
    /// the event-driven hooks (after stock mutations) and the scheduled sweep.
    /// </summary>
    public class LowStockAlertService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IFcmService fcm;
        private readonly ILogger<LowStockAlertService> logger;

        private readonly HashSet<int> notifiedProductIds = new();
        private readonly SemaphoreSlim gate = new(1, 1);

        public LowStockAlertService(
            IServiceScopeFactory scopeFactory,
            IFcmService fcm,
            ILogger<LowStockAlertService> logger)
        {
            this.scopeFactory = scopeFactory;
            this.fcm = fcm;
            this.logger = logger;
        }

        public async Task CheckAndNotifyAsync()
        {
            await gate.WaitAsync();
            try
            {
                using var scope = scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<StockDemoDbContext>();

                var stockByProduct = await db.Stocks
                    .GroupBy(s => s.ProductId)
                    .Select(g => new { ProductId = g.Key, Total = g.Sum(s => s.Quantity) })
                    .ToDictionaryAsync(x => x.ProductId, x => x.Total);

                var products = await db.Products
                    .Where(p => p.IsActive && p.MinQuantity > 0)
                    .ToListAsync();

                var lowNow = new List<(int ProductId, string Name, int Current, int Min)>();
                foreach (var p in products)
                {
                    var current = stockByProduct.TryGetValue(p.ProductId, out var q) ? q : 0;
                    if (current < p.MinQuantity)
                    {
                        lowNow.Add((p.ProductId, p.ProductName, current, p.MinQuantity));
                    }
                }

                var lowIds = lowNow.Select(x => x.ProductId).ToHashSet();

                // Products that recovered can be alerted again in the future.
                notifiedProductIds.RemoveWhere(id => !lowIds.Contains(id));

                var newlyLow = lowNow.Where(x => !notifiedProductIds.Contains(x.ProductId)).ToList();
                if (newlyLow.Count == 0)
                {
                    return;
                }

                if (!fcm.IsEnabled)
                {
                    // Still mark as notified so we don't spam once FCM is enabled with a backlog.
                    logger.LogInformation("Low-stock detected for {Count} product(s) but FCM is disabled.", newlyLow.Count);
                    foreach (var item in newlyLow) notifiedProductIds.Add(item.ProductId);
                    return;
                }

                var deviceRepo = scope.ServiceProvider.GetRequiredService<IDeviceTokenRepository>();
                var tokens = await deviceRepo.GetAllTokensAsync();
                if (tokens.Count == 0)
                {
                    foreach (var item in newlyLow) notifiedProductIds.Add(item.ProductId);
                    return;
                }

                var title = "Cảnh báo tồn thấp";
                var body = newlyLow.Count == 1
                    ? $"{newlyLow[0].Name} còn {newlyLow[0].Current} (định mức {newlyLow[0].Min})"
                    : $"{newlyLow.Count} sản phẩm dưới định mức: " + string.Join(", ", newlyLow.Take(3).Select(x => x.Name));

                var invalid = await fcm.SendAsync(tokens, title, body,
                    new Dictionary<string, string> { ["type"] = "low_stock", ["count"] = newlyLow.Count.ToString() });

                if (invalid.Count > 0)
                {
                    await deviceRepo.RemoveByTokensAsync(invalid);
                }

                foreach (var item in newlyLow) notifiedProductIds.Add(item.ProductId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "LowStockAlertService.CheckAndNotifyAsync failed");
            }
            finally
            {
                gate.Release();
            }
        }

        /// <summary>Fire-and-forget helper for controllers (does not block the request).</summary>
        public void QueueCheck()
        {
            _ = Task.Run(CheckAndNotifyAsync);
        }
    }
}
