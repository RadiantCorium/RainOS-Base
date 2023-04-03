using RainOS.core.objects;
using RainOS.core.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.Processes
{
    internal class ConsoleInterpreter : Process
    {
        public ConsoleInterpreter(int id, User user) : base("Console Mode", id, "The ConsoleMode command interpreter and renderer", user.PermissionLevel, true, false, user)
        {
        }

        internal override void destroy()
        {
            throw new NotImplementedException();
        }

        internal override void init()
        {
            Console.Clear();

        }

        internal override void update()
        {
            throw new NotImplementedException();
        }
    }
}
