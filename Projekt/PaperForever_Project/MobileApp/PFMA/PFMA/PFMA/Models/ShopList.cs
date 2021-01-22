using System;
using System.Collections.Generic;
using System.Text;

namespace PFMA.Models
{
    public class ShopList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] ImageBytes { get; set; }
        public object Image { get; set; }
    }
}
