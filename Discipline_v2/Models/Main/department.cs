using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discipline_v2.Models.Main
{
    public class department
    {
        public int id { get; set; }
        public string dpt_name { get; set; }
        public int company_id { get; set; }
        public string dpt_description { get; set; }

    }
}