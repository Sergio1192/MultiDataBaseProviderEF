using Microsoft.Extensions.DependencyInjection;
using MultiDataBaseProvider.Domain;

namespace MultiDataBaseProvider.IntegrationTests.Infrastructure;

public class Given(TestServer server)
{
    public Task<T> AddAsync<T>(T value) where T : class
        => ActionInContext<T, T>(value, dbSet => async data => await dbSet.AddAsync(data));

    public Task<T[]> AddRangeAsync<T>(IEnumerable<T> values) where T : class
        => ActionInContext<T, T[]>([.. values], dbSet => dbSet.AddRangeAsync);

    public Task<TResult> Data<TEntity, TResult>(Func<MyDbContext, DbSet<TEntity>> getDbSet, Func<DbSet<TEntity>, Task<TResult>> func) where TEntity : class
        => DbContextExecuteAsync(getDbSet, (_, dbSet) => func(dbSet));

    private Task<T> ActionInContext<TEntity, T>(T data, Func<DbSet<TEntity>, Func<T, Task>> action) where TEntity : class
        => DbContextExecuteAsync(
            context => context.Set<TEntity>(),
            async (db, dbSet) =>
            {
                await action(dbSet)(data);
                await db.SaveChangesAsync();

                return data;
            }
        );

    private async Task<TResult> DbContextExecuteAsync<TEntity, TResult>(Func<MyDbContext, DbSet<TEntity>> getDbSet, Func<MyDbContext, DbSet<TEntity>, Task<TResult>> func) where TEntity : class
    {
        using var scope = server.Host.Services.CreateScope();
        var db = scope.ServiceProvider
            .GetRequiredService<MyDbContext>();

        var dbSet = getDbSet(db);
        return await func(db, dbSet);
    }
}
