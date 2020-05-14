using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Discipline_v2.Models.Autorize
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }


        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public Int64 UserId { get; set; }
        public string UserEmail { get; set; }
        public int position { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomPrincipalSerializeModel
    {
        public Int64 UserId { get; set; }
        public string UserEmail { get; set; }
        public int position { get; set; }

    }
}