namespace StockDemo.API.Services
{
    /// <summary>Periodic safety-net sweep that checks for low stock and sends notifications.</summary>
    public class LowStockScheduledService : BackgroundService
    {
        private readonly LowStockAlertService alertService;
        private readonly ILogger<LowStockScheduledService> logger;
        private readonly TimeSpan interval;

        public LowStockScheduledService(
            LowStockAlertService alertService,
            IConfiguration configuration,
            ILogger<LowStockScheduledService> logger)
        {
            this.alertService = alertService;
            this.logger = logger;
            var minutes = configuration.GetValue<int?>("Fcm:ScheduledIntervalMinutes") ?? 360;
            interval = TimeSpan.FromMinutes(Math.Max(1, minutes));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Low-stock scheduled sweep every {Interval}", interval);
            // Small startup delay so the DB migration has completed.
            try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); } catch (OperationCanceledException) { return; }

            while (!stoppingToken.IsCancellationRequested)
            {
                await alertService.CheckAndNotifyAsync();
                try { await Task.Delay(interval, stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }
}
