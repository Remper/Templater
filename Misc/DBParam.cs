using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Templater.Misc
{
    public class DBParam
    {
        public String Name;
        public MySqlDbType Type;
        public Object Value;

        public DBParam(String Name, MySqlDbType Type, Object Value)
        {
            this.Name = Name;
            this.Type = Type;
            this.Value = Value;
        }
    }
}