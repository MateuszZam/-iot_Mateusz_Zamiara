using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using PaperForeverApi.Data;
using PaperForeverApi.Models;

namespace PaperForeverApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private AppDbContext _dbContext;

        public ReceiptsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //api/receipts
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromForm] Receipt newReceipt)
        {
            newReceipt.ReceiptDateTime = DateTime.Now;

            if (newReceipt.Image != null)
            {
                long length = newReceipt.Image.Length;
                using var readStream = newReceipt.Image.OpenReadStream();
                byte[] bytes = new byte[length];
                readStream.Read(bytes, 0, (int)length);
                newReceipt.ImageBytes = bytes;
            }

            _dbContext.Receipts.Add(newReceipt);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/receipts/UserReceipts/1
        [Authorize]
        [HttpGet("[action]/{id}")]
        public IActionResult UserReceipts(int id)
        {
            var receipts = from receipt in _dbContext.Receipts
                           join user in _dbContext.Users on receipt.UserId equals user.Id
                           join shop in _dbContext.Shops on receipt.ShopId equals shop.Id
                           where user.Id == id
                           orderby receipt.ReceiptDateTime descending
                           select new
                           {
                               Id = receipt.Id,
                               ReceiptDateTime = receipt.ReceiptDateTime,
                               Price = receipt.Price,
                               ShopName = shop.Name
                           };

            return Ok(receipts);
        }

        //api/receipts/ReceiptDetails?uid=&rid=
        [Authorize]
        [HttpGet("[action]")]
        public IActionResult ReceiptDetails(int uid, int rid)
        {
            var receiptDetails = (from receipt in _dbContext.Receipts
                                  join user in _dbContext.Users on receipt.UserId equals user.Id
                                  join shop in _dbContext.Shops on receipt.ShopId equals shop.Id
                                  where receipt.UserId == uid && receipt.Id == rid
                                  select new
                                  {
                                      Id = receipt.Id,
                                      ReceiptDateTime = receipt.ReceiptDateTime,
                                      Price = receipt.Price,
                                      ImageBytes = receipt.ImageBytes,
                                      ShopName = shop.Name
                                  }).FirstOrDefault();

            return Ok(receiptDetails);
        }

        //api/receipts/1
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var receipt = _dbContext.Receipts.Find(id);

            if (receipt == null)
            {
                return NotFound("Receipt with this id not found or already deleted");
            }
            else
            {
                _dbContext.Receipts.Remove(receipt);
                _dbContext.SaveChanges();
                return Ok("Receipt successfully removed from database");
            }
        }
    }
}
