using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingBackend.Controllers;
using ShoppingBackend.Models;
using Xunit;

namespace ShoppingBackend.Tests
{
    public class ShoplistControllerUnitTests
    {
        private ShoplistDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoplistDbContext>()
                .UseInMemoryDatabase("TestDb_" + Guid.NewGuid())
                .Options;
            return new ShoplistDbContext(options);
        }

        [Fact]
        public void GetShoplist_WhenNoItems_ReturnsEmptyList()
        {
            // Arrange: create in-memory context and controller
            using var db = GetInMemoryDbContext();
            var controller = new ShoplistController(db);
            // Inject the in-memory context into the public 'db' field
            typeof(ShoplistController)
                .GetField("db", BindingFlags.Instance | BindingFlags.Public)
                ?.SetValue(controller, db);

            // Act
            var actionResult = controller.GetShoplist() as OkObjectResult;
            var items = Assert.IsType<List<Shoplist>>(actionResult.Value);

            // Assert
            Assert.Equal(200, actionResult.StatusCode);
            Assert.Empty(items);
        }

        [Fact]
        public void AddNewItem_WithValidItem_AddsItemToDatabase()
        {
            // Arrange
            using var db = GetInMemoryDbContext();
            var controller = new ShoplistController(db);
            typeof(ShoplistController)
                .GetField("db", BindingFlags.Instance | BindingFlags.Public)
                ?.SetValue(controller, db);

            var newItem = new Shoplist { Item = "Testi", Amount = 1 };

            // Act
            var actionResult = controller.AddNewItem(newItem) as OkObjectResult;

            // Assert HTTP response
            Assert.NotNull(actionResult);
            Assert.Equal(200, actionResult.StatusCode);
            Assert.Equal("Added new item", actionResult.Value);

            // Assert database state
            var stored = db.Shoplist.ToList();
            Assert.Single(stored);
            Assert.Equal("Testi", stored[0].Item);
            Assert.Equal(1, stored[0].Amount);
        }
    }
}
