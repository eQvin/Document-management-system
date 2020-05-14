using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discipline_v2.Models.ChatModels
{
    public class ChatModel
    {
        public string lname { get; set; }
        public string fname { get; set; }
        public string msg { get; set; }
        public string date { get; set; }
        public string otherUser { get; set; }
        public int sender { get; set; }
        public string msg_type { get; set; }
    }
}