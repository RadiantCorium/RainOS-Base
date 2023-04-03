using RainOS.core.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.services
{
    /// <summary>
    /// Permission Enforcement Layer
    /// </summary>
    internal class PEL
    {
        public static bool CheckUserPermLevel(string username, PermissionLevel level)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            var info = UMS.GetUserInfo(username);

            if (info == null)
            {
                throw new ObjectNotFoundException("user '" +  username + "'");
            }

            int.TryParse(info.Split(",")[2], out int res);

            if (res >= (int)level)
            {
                return true;
            }

            return false;
        }

        public static void AssertUserPermLevel(string username, PermissionLevel level)
        {
            if (!CheckUserPermLevel(username, level))
            {
                throw new AccessViolationException($"User '{username}' failed to meet permission level {level}");
            }
        }

        /// <summary>
        /// Sets a process' permission level to elevated
        /// </summary>
        /// <param name="process">the process to elevate</param>
        /// <param name="force">Whether or not to ignore the permission level of the user running the process</param>
        /// <returns>true if sucessfull, false if permission denied.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool ElevateProcess(Process process, bool force)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }

            if (!CheckUserPermLevel(process.user.Name, PermissionLevel.Elevated) && !force)
            {
                return false;
            }

            process.level = PermissionLevel.Elevated;
            return true;
        }
    }
}
