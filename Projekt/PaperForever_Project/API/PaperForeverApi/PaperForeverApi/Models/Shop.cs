using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PaperForeverApi.Models
{
    public class Shop
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int InCityCount { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public byte[] ImageBytes { get; set; }
    }
}
