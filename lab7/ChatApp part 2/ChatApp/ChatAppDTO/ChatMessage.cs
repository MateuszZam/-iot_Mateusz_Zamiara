using System;

namespace ChatAppDTO
{
    public class ChatMessage
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string FormattedTimeStamp => TimeStamp.ToString("yyyy-MM-dd, HH:mm:ss");
    }
}
