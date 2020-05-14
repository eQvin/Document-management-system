using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discipline_v2.Models.Main
{
    public class document
    {
        public int id { get; set; }
        public string description { get; set; }
        public string send_id { get; set; }
        public string owner_id { get; set; }
        public int status { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public string tittle { get; set; }
        public DateTime date { get; set; }
    }
}