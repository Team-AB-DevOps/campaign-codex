using Hovedopgave.Core.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Infrastructure;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
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
}