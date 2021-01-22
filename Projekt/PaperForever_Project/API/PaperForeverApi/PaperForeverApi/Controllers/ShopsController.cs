using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using PaperForeverApi.Data;
using PaperForeverApi.Models;

namespace PaperForeverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private AppDbContext _dbContext;

        public ShopsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //api/shops
        [Authorize]
        [HttpGet]
        public IActionResult AllShops()
        {
            var shops = from shop in _dbContext.Shops
                        orderby shop.Name
                        select new
                        {
                            Id = shop.Id,
                            Name = shop.Name,
                            Type = shop.Type,
                            ImageBytes = shop.ImageBytes
                        };

            return Ok(shops);
        }

        //api/shops/ShopsOfType?type=
        [Authorize]
        [HttpGet("[action]")]
        public IActionResult ShopsOfType(string type)
        {
            var shops = from shop in _dbContext.Shops
                        where shop.Type.StartsWith(type)
                        select new
                        {
                            Id = shop.Id,
                            Name = shop.Name,
                            Type = shop.Type,
                            ImageBytes = shop.ImageBytes
                        };

            return Ok(shops);
        }

        //api/shops/FindShops?name=
        [Authorize]
        [HttpGet("[action]")]
        public IActionResult FindShops(string name)
        {
            var shops = from shop in _dbContext.Shops
                        where shop.Name.StartsWith(name)
                        select new
                        {
                            Id = shop.Id,
                            Name = shop.Name,
                            Type = shop.Type,
                            ImageBytes = shop.ImageBytes
                        };

            return Ok(shops);
        }

        //api/shops/ShopDetails/1
        [Authorize]
        [HttpGet("[action]/{id}")]
        public IActionResult ShopDetails(int id)
        {
            var shop = _dbContext.Shops.Find(id);

            if (shop == null)
            {
                return NotFound("Shop with this id not found");
            }

            return Ok(shop);
        }

        //api/shops
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromForm] Shop newShop)
        {
            if (newShop.Image != null)
            {
                long length = newShop.Image.Length;
                using var readStream = newShop.Image.OpenReadStream();
                byte[] bytes = new byte[length];
                readStream.Read(bytes, 0, (int) length);
                newShop.ImageBytes = bytes;
            }

            _dbContext.Shops.Add(newShop);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/shops/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult EditShop(int id, [FromForm] Shop shop)
        {
            var existingShop = _dbContext.Shops.Find(id);

            if (existingShop == null)
            {
                return NotFound("Shop with id not found");
            }
            else
            {
                if (shop.Image != null)
                {
                    long length = shop.Image.Length;
                    using var readStream = shop.Image.OpenReadStream();
                    byte[] bytes = new byte[length];
                    readStream.Read(bytes, 0, (int)length);
                    existingShop.ImageBytes = bytes;
                }

                existingShop.Name = shop.Name;
                existingShop.Type = shop.Type;
                existingShop.InCityCount = shop.InCityCount;

                _dbContext.SaveChanges();
                return Ok("Shop updated successfully");
            }
        }

        //api/shops/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteShop(int id)
        {
            var shop = _dbContext.Shops.Find(id);

            if (shop == null)
            {
                return NotFound("Shop with this id not found or already deleted");
            }
            else
            {
                _dbContext.Shops.Remove(shop);
                _dbContext.SaveChanges();
                return Ok("Shop successfully removed from database");
            }
        }
    }
}
