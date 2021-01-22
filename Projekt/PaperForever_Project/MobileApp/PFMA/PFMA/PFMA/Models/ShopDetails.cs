using System;
using System.Collections.Generic;
using System.Text;

namespace PFMA.Models
{
    public class ShopDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int InCityCount { get; set; }
        public object Image { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
