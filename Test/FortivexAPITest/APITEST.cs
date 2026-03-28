using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FortivexAPI.Data;
using FortivexAPI.Dtos;
using Xunit;

namespace FortivexAPI.Tests
{
    public class AccountsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AccountsApiTests(WebApplicationFactory<Program> factory)
        {
            // Itt konfiguráljuk át a teszt szervert, hogy InMemory DB-t használjon
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Eltávolítjuk az eredeti MariaDB konfigurációt
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<FortivexContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    // Hozzáadjuk az InMemory adatbázist
                    services.AddDbContext<FortivexContext>(options =>
                    {
                        options.UseInMemoryDatabase("ApiIntegrationTestDb");
                    });
                    // --- IDE ILLESZD BE A KIEGÉSZÍTÉST: ---
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<FortivexContext>();

                        // Ez kényszeríti ki az OnModelCreating lefutását és a Seed adatok betöltését
                        db.Database.EnsureCreated();
                    }
                    // --- KIEGÉSZÍTÉS VÉGE ---
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetAccounts_Endpoint_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/accounts");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var accounts = await response.Content.ReadFromJsonAsync<List<AccountDto>>();
            Assert.NotNull(accounts);
        }

        [Fact]
        public async Task Login_WithSeedData_ReturnsSuccess()
        {
            // Arrange - A FortivexContext-ben lévő seed adatot használjuk
            var loginDto = new LoginRequestDto
            {
                UserName = "testplayer",
                PasswordHash = "test123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/accounts/login", loginDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}