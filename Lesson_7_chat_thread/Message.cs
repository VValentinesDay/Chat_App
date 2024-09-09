


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Chat_Web_App
{
    public class Message
    {
        public Message() { }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
            // превращает строку в Json
        }
        public static Message? FromJson(string message)
        {
            // из Json формирует string 
            return JsonSerializer.Deserialize<Message>(message);
        }

        public override string ToString()
        {
            return $"{Name} {Text} {DateTime.ToShortTimeString()}";
        }


    }
}
