using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBackend.Models;

namespace ShoppingBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoplistController : ControllerBase
    {

        public readonly ShoplistDbContext db = new();
        private ShoplistDbContext db1;

        public ShoplistController(ShoplistDbContext db1)
        {
            this.db1 = db1;
        }


        // Hakee ostoslistan
        [HttpGet]
        public ActionResult GetShoplist()
        {
            var items = db.Shoplist.ToList();

            return Ok(items);
        }


        // Tavaroiden lisääminen ostoslistalle
        [HttpPost]
        public ActionResult AddNewItem([FromBody] Shoplist item)
        {
            db.Shoplist.Add(item);
            db.SaveChanges();

            return Ok("Added new item");
        }


        // Tavaroiden poistaminen listalta
        [HttpDelete("{id}")]
        public ActionResult AddNewItem(int id)
        {
            Shoplist? item = db.Shoplist.Find(id);
            if (item != null)
            {
                db.Shoplist.Remove(item);
                db.SaveChanges();
                return Ok("Item removed successfully.");
            }
            else
            {
                return NotFound("Product with id " + id + " not found.");
            }
        }

    }
}