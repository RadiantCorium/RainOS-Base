using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.objects
{
    internal class User
    {
        /// <summary>
        /// Creates a new user object
        /// </summary>
        /// <param name="name">username</param>
        /// <param name="password">hashed password</param>
        internal User(string name, string password, bool allowLoggingIn)
        {
            this.Name = name;
            this.Password = password;
            AllowLoggingIn = allowLoggingIn;
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public bool AllowLoggingIn { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
    }
}
