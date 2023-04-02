using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using Console = System.Console;
using Sys = Cosmos.System;

namespace RainOS
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            Console.WriteLine("Initializing Filesystem...");

            core.Globals.fs = new CosmosVFS();
            VFSManager.RegisterVFS(core.Globals.fs);

            Console.WriteLine("Initalizing UMS...");

            // initialize user management service

            Console.WriteLine("Checking system status...");

            // check for important files
        }

        protected override void Run()
        {
            try
            {
                
            }
            catch (Exception e)
            {
                bsod(e);
            }
        }

        public static void bsod(Exception e)
        {
            //screen.Disable();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            string title = "RainOS had a fucking stroke and died!";
            Console.WriteLine(new String(' ', 80));
            Console.SetCursorPosition(0, 1);
            Console.Write(new String(' ', 23));
            Console.Write(title);
            Console.WriteLine(new String(' ', 20));
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(new String(' ', 80));

            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("ERROR: " + e.ToString());

            Console.WriteLine();

            Console.WriteLine("Press any key to power off the system...");

            Console.ReadKey();

            Cosmos.Core.CPU.DisableInterrupts();

            Cosmos.Core.CPU.Halt();
        }
    }
}
