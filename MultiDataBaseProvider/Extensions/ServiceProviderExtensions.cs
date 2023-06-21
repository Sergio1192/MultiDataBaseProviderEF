using Microsoft.EntityFrameworkCore;
using MultiDataBaseProvider.Domain;

namespace MultiDataBaseProvider.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task MigrateDbContextAsync(this IServiceProvider serviceProvider)
    {
        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider
            .GetRequiredService<MyDbContext>();

        await db.Database.MigrateAsync();
    }
}
