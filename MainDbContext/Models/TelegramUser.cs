using System;
using System.Collections.Generic;
using System.Text;

namespace DbCore.Models
{
    public class TelegramUser
    {
        public Guid Id { get; set; }
        public long TelegramUserId { get; set; }
        public string Role { get; set; }
    }
}
