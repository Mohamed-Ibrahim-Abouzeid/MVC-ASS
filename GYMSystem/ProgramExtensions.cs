using GymManagementSystemG01.DAL.Data.DataSeed;
using GYMSystem.DAL.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystemG01.PL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GYMDBContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();



            var pending = await dbContext.Database.GetPendingMigrationsAsync();
            if (pending.Any())
            {
                logger.LogInformation("Applying {Count} pending migrations...", pending.Count());
                await dbContext.Database.MigrateAsync();
            }

            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, seedPath, logger);
        }
    }
}
