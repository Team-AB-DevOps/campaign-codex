using Hovedopgave.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Infrastructure;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    protected readonly AppDbContext DbContext;
    protected readonly IntegrationTestWebAppFactory Factory;
    protected readonly IServiceProvider ServiceProvider;

    public BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        Factory = factory;
        _scope = factory.Services.CreateScope();
        ServiceProvider = _scope.ServiceProvider;
        DbContext = ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual async Task DisposeAsync()
    {
        // Clean up test data after each test
        var characters = DbContext.Characters.ToList();
        await DbContext.Characters.ExecuteDeleteAsync();

        _scope.Dispose();
    }
}