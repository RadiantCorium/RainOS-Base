using RainOS.core.crypto;
using RainOS.core.objects;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// <param name="password">unhashed password of the user to log in</param>
        /// <returns>0 if sucessful, 1 if user not found, 2 if password incorrect</returns>
        public static int LoginUser(string username, string password)
        {
            // hash the password
            string hashedPass = HashPassword(password);

            // check if the user exists
            string userInfo = GetUserInfo(username);
            if (userInfo == null)
                return 1;

            // compare the credentials
            if (userInfo.Split(",")[1] != hashedPass)
                return 2;

            // if we're at this point, the credentials are correct. create the user object and add it
            User user = new User(username, hashedPass);
            if (int.TryParse(userInfo.Split(",")[2], out int level))
            {
                user.PermissionLevel = (PermissionLevel)level;
                Users.Add(user);
            }
            else
            {
                user.PermissionLevel = PermissionLevel.User;
            }
            return 0;
        }

        /// <summary>
        /// Closes all applications related to a user, and logs them out
        /// </summary>
        /// <param name="user">the user to log out</param>
        public static void LogoutUser(User user)
        {
            // TODO: add process closing

            Users.Remove(user);
        }

        /// <summary>
        /// Closes all applications related to a user, and logs them out
        /// </summary>
        /// <param name="name">the user to log out</param>
        public static void LogoutUser(string name)
        {
            // TODO: add process closing

            foreach (User user in Users)
            {
                if (user.Name == name)
                {
                    Users.Remove(user);
                }
            }
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="username">username of the user to create</param>
        /// <param name="password">unhashed password of the user</param>
        /// <param name="permissionLevel">permission level of the user</param>
        /// <param name="login">Whether to automatically log the user in</param>
        /// <returns>0 if successfull, 1 if user already exists</returns>
        public static int CreateUser(string username, string password, PermissionLevel permissionLevel, bool login)
        {
            // first check if the user already exists
            if (GetUserInfo(username) != null)
            {
                if (login)
                {
                    LoginUser(username, password);
                }
                return 1;
            }

            // hash the password
            string hashedPass = HashPassword(password);

            // if not, write the user's data to the file
            string userdata = username + "," + hashedPass + "," + (int)permissionLevel;

            StreamWriter s = File.AppendText(@"0:\users.txt");
            s.WriteLine(userdata);
            s.Close();

            if (login)
            {
                LoginUser(username, password);
            }

            return 0;
        }

        /// <summary>
        /// Checks if a user exists
        /// </summary>
        /// <param name="username">the username to check for</param>
        /// <returns>null if the user is not found, otherwise returns user info in the format NAME,HASHEDPASSWORD,PERMISSIONLEVEL</returns>
        public static string GetUserInfo(string username)
        {
            // Load the contents of the "users.txt" file
            // check if the users.txt file exists, create one if not
            if (!File.Exists(@"0:\users.txt"))
            {
                File.Create(@"0:\users.txt");
            }
            string[] usersFile = File.ReadAllText(@"0:\users.txt").Split("\n");

            foreach (string user in usersFile)
            {
                string name = user.Split(",")[0];
                if (name == username)
                {
                    return user;
                }
            }

            return null;
        }

        public static string HashPassword(string password)
        {
            Sha256 sha256 = new Sha256();
            byte[] passwordBytesUnhashed = Encoding.Unicode.GetBytes(password);
            sha256.AddData(passwordBytesUnhashed, 0, (uint)passwordBytesUnhashed.Length);
            return Convert.ToBase64String(sha256.GetHash());
        }
    }
}
