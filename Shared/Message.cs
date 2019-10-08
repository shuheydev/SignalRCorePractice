using System;

namespace Shared
{
    public class Message
    {
        public string SenderName { get; set; }
        public string Body { get; set; }
        public DateTimeOffset SendDateTime { get; set; }
    }
}
