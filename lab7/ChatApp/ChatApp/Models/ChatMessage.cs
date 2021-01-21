using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Models
{
    public class ChatMessage
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string FormattedTimeStamp => TimeStamp.ToString("yyyy-MM-dd, HH:mm:ss");
    }
}
