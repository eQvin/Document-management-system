using Discipline_v2.Models.Autorize;
using Discipline_v2.Models.ChatModels;
using Discipline_v2.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Discipline_v2.Models
{
    public class BaseModel
    {
        public user User { get; set; }
        public company Company { get; set; }

        public List<company>  Companies { get; set; }
        public List<department> Departments { get; set; }
        public List<document> Documents { get; set; }
        public List<document> DocumentsHistory { get; set; }
        public List<document> DocumentsInbox { get; set; }

        public List<user> Users { get; set; }
        public ChatModel ChatModel { get; set; }
        public conversation conversation { get; set; }
        public message message { get; set; }
        public BaseModel()  //Конструктор – для экземпляра класса
        {
            // Users region
            User = new user();
            Users = new List<user>();
            Company = new company();
            Companies = new List<company>();
            Departments = new List<department>();

            // Document region
            Documents = new List<document>();
            DocumentsHistory = new List<document>();
            DocumentsInbox = new List<document>();

            //ChatModel region
            conversation = new conversation();
            message = new message();
            ChatModel = new ChatModel();

        }
    }
}