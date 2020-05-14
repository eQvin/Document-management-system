using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discipline_v2.Models.ChatModels
{
    public class message
    {
        public int id { get; set; }
        public int conversation_id { get; set; }
        public DateTime date { get; set; }
        public string msg { get; set; }
        public string msg_type { get; set; }
        public string sender { get; set; }
    }
}