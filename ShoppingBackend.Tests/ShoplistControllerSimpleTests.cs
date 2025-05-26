using Microsoft.AspNetCore.Mvc;
using ShoppingBackend.Controllers;
using ShoppingBackend.Models;

public class ShoplistControllerSimpleTests
{
    [Fact]
    public void GetShoplist_ReturnsOkResultWithItems()
    {
        // Arrange: instantiate context and controller
        var dbContext = new ShoplistDbContext();
        var controller = new ShoplistController(dbContext);

        // Act
        var result = controller.GetShoplist() as OkObjectResult;
        var items = Assert.IsType<List<Shoplist>>(result.Value);

        // Assert
        Assert.Equal(200, result.StatusCode);
        Assert.NotEmpty(items); // DB is seeded
    }
}
