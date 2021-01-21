using System;
using System.Collections.Generic;
using System.Text;
using ChatAppDTO;

namespace MobileChatApp.Models
{
    class ChatMessageListItem : ChatMessage
    {
        protected ChatMessageListItem() {}

        public ChatMessageListItem(ChatMessage msg)
        {
            TimeStamp = msg.TimeStamp;
            Message = msg.Message;
            UserName = msg.UserName;
        }

        public string Summary =>
            $"[{FormattedTimeStamp}] {UserName} | {Message}";
    }
}
