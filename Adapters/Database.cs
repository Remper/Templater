using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Templater.Adapters
{
    public class Database
    {
        public bool CheckUserCredentials(String email, String password)
        {
            return true;
        }
    }
}