using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PaperForeverApi.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        
        public DateTime ReceiptDateTime { get; set; }

        [Required]
        public float Price { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public byte[] ImageBytes { get; set; }

        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int ShopId { get; set; }
    }
}
