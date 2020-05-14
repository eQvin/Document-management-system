using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Discipline_v2.Models.Main
{
    public class company
    {
        public int id { get; set; }
        public string company_name { get; set; }
        public string address { get; set; }
        public string company_country { get; set; }
        public string ceo_name { get; set; }
        public string bank_detail { get; set; }
        public string post_index { get; set; }
        public string site { get; set; }
        public string tell { get; set; }
    }
}