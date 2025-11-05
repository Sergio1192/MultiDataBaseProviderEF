using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MultiDataBaseProvider.Domain;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace MultiDataBaseProvider.IntegrationTests.Infrastructure;

[Collection(nameof(ApiCollection))]
public abstract class TestsBase(ApiFixture fixture) : IAsyncLifetime
{
    private readonly static JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected TestServer Server { get => fixture.Server; }
    protected IServiceProvider Services { get => fixture.Services; }

    public Task InitializeAsync() => ResetDatabase();

    public Task DisposeAsync() => Task.CompletedTask;

    protected Task<TResponse> GetResultAsync<TController, TResponse>(Expression<Func<TController, Task<ActionResult<TResponse>>>> actionSelector)
        where TController : class
        => GetResultAsync<TController, ActionResult<TResponse>, TResponse>(actionSelector);

    protected Task<TResponse> GetResultAsync<TController, TResponse>(Expression<Func<TController, Task<TResponse>>> actionSelector)
        where TController : class
        => GetResultAsync<TController, TResponse, TResponse>(actionSelector);

    private async Task<TResponse> GetResultAsync<TController, TResult, TResponse>(Expression<Func<TController, Task<TResult>>> actionSelector)
        where TController : class
    {
        var response = await SendResponseAsync(actionSelector);

        var result = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(result))
            return default!;

        if (typeof(TResponse) == typeof(string))
            result = $"\"{result}\"";

        return JsonSerializer.Deserialize<TResponse>(result, jsonSerializerOptions)!;
    }

    protected Task<HttpResponseMessage> SendResponseAsync<TController>(Expression<Func<TController, object>> actionSelector)
       where TController : class
        => SendResponseAsync<TController, object>(actionSelector);

    private async Task<HttpResponseMessage> SendResponseAsync<TController, TResponse>(Expression<Func<TController, TResponse>> actionSelector, bool checkResponse = true)
        where TController : class
    {
        var request = Server.CreateHttpApiRequest(actionSelector);

        HttpResponseMessage response;

        var methodCall = (MethodCallExpression)actionSelector.Body;
        var attributes = methodCall.Method.GetCustomAttributes().Select(attr => attr.GetType());
        if (attributes.Contains(typeof(HttpPostAttribute)))
        {
            response = await request.PostAsync();
        }
        else if (attributes.Contains(typeof(HttpPutAttribute)))
        {
            response = await request.SendAsync(HttpMethod.Put.ToString());
        }
        else if (attributes.Contains(typeof(HttpPatchAttribute)))
        {
            response = await request.SendAsync(HttpMethod.Patch.ToString());
        }
        else if (attributes.Contains(typeof(HttpDeleteAttribute)))
        {
            response = await request.SendAsync(HttpMethod.Delete.ToString());
        }
        else
        {
            response = await request.GetAsync();
        }

        if (checkResponse)
        {
            await response.IsSuccessStatusCodeOrThrow();
        }

        return response;
    }

    private async Task ResetDatabase()
    {
        using var scope = Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

        await context.Database.OpenConnectionAsync();
        using var connection = context.Database.GetDbConnection();

        await fixture.Respawner.ResetAsync(connection);

        await context.Database.CloseConnectionAsync();
    }
}
