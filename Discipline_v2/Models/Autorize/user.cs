using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Discipline_v2.Models.Autorize
{
    public class user
    {
        public int id { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public int position { get; set; }
        public Int16 check { get; set; }
        public Int16 email_check { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime birth_day { get; set; }
        public DateTime register_day { get; set; }
        public string iin { get; set; }
        public Int16 sex { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string sur_name { get; set; }
        public string tel_number { get; set; }
        public Int16 mailing_schedule { get; set; }
        public int city_id { get; set; }
        public int company_id { get; set; }
        public string security_stamp { get; set; }
        public int department_id { get; set; }
        public int department_position { get; set; }
        public user()
        {
            id = new int();
            login = "";
            email = "";
            password = "";
            position = 1;
            check = new short();
            email_check = new short();
            birth_day = new DateTime();
            register_day = DateTime.Now;
            iin = "";
            sex = new short();
            first_name = "";
            last_name = "";
            sur_name = "";
            tel_number = "";
            mailing_schedule = new short();
            city_id = new int();
            company_id = new int();
            security_stamp = "";
            department_id = new int();
            department_position = new int();
        }
    }

    public class user_paswords
    {
        public string pasword { get; set; }
        public string pasword_confirm { get; set; }
    }
}
