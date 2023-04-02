using RainOS.core.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.services
{
    /// <summary>
    /// UMS stands for User Management Service.
    /// It keeps track of all currently logged in users.
    /// </summary>
    internal class UMS
    {
        public static List<User> Users { get; set; }

        /// <summary>
        /// This method logs a user in, and is responsible for checking credentials
        /// </summary>
        /// <param name="username">username of the user to log in</param>
        /// <param name="password">hashed password of the user to log in</param>
        public static void LoginUser(string username, string password)
        {

        }

        /// <summary>
        /// Closes all applications related to a user, and logs them out
        /// </summary>
        /// <param name="user">the user to log out</param>
        public static void LogoutUser(User user)
        {

        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="username">username of the user to create</param>
        /// <param name="password">hashed password of the user</param>
        /// <param name="permissionLevel">permission level of the user</param>
        /// <param name="login">Whether or not to automatically log the user in</param>
        public static void CreateUser(string username, string password, PermissionLevel permissionLevel, bool login)
        {

        }
    }
}
