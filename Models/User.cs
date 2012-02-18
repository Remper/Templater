using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Templater.Models
{
    public class User
    {
        private string _Email;
        private string _WorkGroup;
        private bool _AuthState;

        public string Email { get { return this._Email; } }
        public string WorkGroup { get { return this._WorkGroup; } }
        public bool AuthState { get { return this._AuthState; } }

        public bool Authorize(String email, String password)
        {
            this._Email = email;
            return true;
        }
    }
}