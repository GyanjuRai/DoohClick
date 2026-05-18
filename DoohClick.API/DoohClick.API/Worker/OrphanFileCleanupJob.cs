using DoohClick.Interface.Shared.Worker;

namespace DoohClick.API.worker
{
    public class OrphanFileCleanupJob : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;
        public OrphanFileCleanupJob(
                IServiceScopeFactory serviceScopeFactory,
                IWebHostEnvironment webHostEnvironment,
                ILogger<OrphanFileCleanupJob> logger
            )
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoCleanUp();
                    _logger.LogInformation("DoCleanUp completed at {Time}", DateTime.UtcNow);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "DoCleanUp failed at {Time}", DateTime.UtcNow);
                }
                await Task.Delay(GetNextRunDelay(), stoppingToken);

            }
        }

        private static TimeSpan GetNextRunDelay()
        {
            DateTime now = DateTime.UtcNow;
            DateTime next = now.Date.AddDays(1).AddHours(2);
            return next - now;
        }

        private async Task DoCleanUp()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var service = scope.ServiceProvider
                .GetRequiredService<IOrphanFileCleanupService>();
            await service.ExecuteAsync();
        }

    }
}
