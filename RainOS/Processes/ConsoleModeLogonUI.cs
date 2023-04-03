using RainOS.core;
using RainOS.core.objects;
using RainOS.core.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.Processes
{
    internal class ConsoleModeLogonUI : Process
    {
        internal bool succesful = false;
        internal int fails = 0;
        internal int failed = 1;

        internal User loggedonUser;

        public ConsoleModeLogonUI(int id) : base("Console Mode Login UI", id, "The UI responsible for logging the user in in console mode", PermissionLevel.System, true, false, UMS.GetUser("KERNEL"))
        {
        }

        internal override void destroy()
        {
            // start ConsoleInterpreter
            PM.AddProcess(new ConsoleInterpreter(new Random().Next(100000, 999999), loggedonUser));
        }

        internal override void init()
        {
            while (!succesful)
            {
                Console.Clear();

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                string title = "Log into Console Mode";
                Console.WriteLine(new String(' ', 80));
                Console.SetCursorPosition(0, 1);
                Console.Write(new String(' ', 30));
                Console.Write(title);
                Console.WriteLine(new String(' ', 29));
                Console.SetCursorPosition(0, 2);
                Console.WriteLine(new String(' ', 80));

                Console.WriteLine();

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("Username: ");
                string u = Console.ReadLine();

                Console.Write("Password: ");
                string p = Console.ReadLine();

                Console.WriteLine("Checking credentials...");
                int result = UMS.LoginUser(u, p);

                switch (result)
                {
                    case 0:
                        Console.WriteLine("OK");
                        loggedonUser = UMS.GetUser(u);
                        succesful = true;
                        break;
                    case 1:
                        Console.WriteLine("Username not found");
                        fails++;
                        break;
                    case 2:
                        Console.WriteLine("Password incorrect");
                        fails++;
                        break;
                }

                if (fails >= 3)
                {
                    Console.WriteLine($"Credentials incorrect 3 or more times. Please wait for {60 * failed} seconds before trying again.");
                    fails = 0;
                    sleep(60000 * failed);
                    failed++;
                }
            }

            // if sucessful, close the application.
            PM.RemoveProcess(this, false);
        }

        internal override void update()
        {
            
        }

        /// <summary>
        /// !!Blocks other processes!!
        /// </summary>
        /// <param name="milliseconds"></param>
        internal void sleep(int milliseconds)
        {
            var end = DateTime.Now.AddMilliseconds(milliseconds);
            while (DateTime.Now < end) {}
            return;
        }
    }
}
