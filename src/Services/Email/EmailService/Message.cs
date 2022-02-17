using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class Message
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(string to, string subjcet, string content)
        {
            this.To = to;
            this.Subject = subjcet;
            this.Content = content;
        }
    }
}
