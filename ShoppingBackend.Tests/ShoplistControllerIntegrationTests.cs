using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ShoppingBackend;              // your app’s root namespace
using ShoppingBackend.Models;       // for ShoplistDbContext
using System.Text.Json;
using System.Collections.Generic;
using System.Text;




namespace ShoppingBackend.Tests
{
    public class ShoplistControllerIntegrationTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ShoplistControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Override DI so we use a fresh InMemory database (no seed)
            var customizedFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 1) Remove existing DbContext registration
                    var descriptor = services
                        .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ShoplistDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    // 2) Add InMemory DbContext for testing
                    services.AddDbContext<ShoplistDbContext>(opts =>
                    {
                        opts.UseInMemoryDatabase("TestDb");
                    });
                });
            });

            _client = customizedFactory.CreateClient();
        }

        [Fact]
        public async Task DeleteItem_NonExistingId_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync("/api/shoplist/999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var text = await response.Content.ReadAsStringAsync();
            Assert.Contains("Product with id 999 not found", text);
        }

        [Fact]
        public async Task AddNewItem_ThenGetShoplist_IncludesNewItem()
        {
            // Arrange
            var newItem = new Shoplist { Item = "IntegrationTestItem", Amount = 3 };
            var payload = JsonSerializer.Serialize(newItem);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            // Act: POST the new item
            var postResponse = await _client.PostAsync("/api/shoplist", content);
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            var postText = await postResponse.Content.ReadAsStringAsync();
            Assert.Contains("Added new item", postText);

            // Act: GET the full list
            var getResponse = await _client.GetAsync("/api/shoplist");
            getResponse.EnsureSuccessStatusCode();
            var json = await getResponse.Content.ReadAsStringAsync();

            // Deserialize and Assert
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var items = JsonSerializer.Deserialize<List<Shoplist>>(json, options);
            Assert.NotNull(items);
            Assert.Contains(items, i =>
                i.Item.Trim() == "IntegrationTestItem" &&
                i.Amount == 3
            );
        }
    }
}
