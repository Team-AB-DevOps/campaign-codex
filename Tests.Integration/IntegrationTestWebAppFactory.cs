using Hovedopgave.Core.Data;
using Hovedopgave.Core.Services;
using Hovedopgave.Features.Account.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Tests.Integration;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("hovedopgave_test_db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        await context.Database.MigrateAsync();
        await DbInitializer.SeedData(context, userManager);
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Provide a dummy connection string to prevent Program.cs from throwing
        builder.UseSetting("CONNECTION_STRING", "Host=dummy;Database=dummy;Username=dummy;Password=dummy");

        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
                {
                    options
                        .UseNpgsql(_dbContainer.GetConnectionString());
                }
            );

            // Remove real UserAccessor
            var userAccessorDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUserAccessor));
            if (userAccessorDescriptor is not null)
            {
                services.Remove(userAccessorDescriptor);
            }

            // Add test UserAccessor
            services.AddScoped<IUserAccessor, TestUserAccessor>();
        });
    }
}

public class TestUserAccessor : IUserAccessor
{
    public string UserId { get; set; } = Guid.NewGuid().ToString();

    public Task<User> GetUserAsync()
    {
        return Task.FromResult(new User { Id = UserId, UserName = "TestUser" });
    }

    public string GetUserId()
    {
        return UserId;
    }
}