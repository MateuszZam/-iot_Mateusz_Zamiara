using System;
using System.Collections.Generic;
using System.Text;

namespace PFMA.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public object Image { get; set; }
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public byte[] ImageArray { get; set; }
    }
}
