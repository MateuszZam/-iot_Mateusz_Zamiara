using System;
using System.Collections.Generic;
using System.Text;

namespace PFMA.Models
{
    public class ReceiptDetails
    {
        public int Id { get; set; }
        public DateTime ReceiptDateTime { get; set; }
        public float Price { get; set; }
        public byte[] ImageBytes { get; set; }
        public string ShopName { get; set; }
    }
}
