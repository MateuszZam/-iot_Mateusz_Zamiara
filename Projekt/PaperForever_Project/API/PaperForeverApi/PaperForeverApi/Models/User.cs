using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaperForeverApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        
        public string Role { get; set; }

        public ICollection<Receipt> Receipts { get; set; }
    }
}
