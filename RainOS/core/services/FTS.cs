using Cosmos.Core;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.ScanMaps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = System.Console;

namespace RainOS.core.services
{
    /// <summary>
    /// First time setup
    /// </summary>
    internal class FTS
    {
        private static int state = 0;
        private static int sel = 0;

        private static string[] layoutOptions;

        private static string configString = "";

        internal static void Open()
        {
            #region Formatting
            clearScreenWHeader();

            Console.WriteLine("WARNING: Setting up RainOS will clear all data on disk parition 0:\\.\n\n\nENTER: Proceed - BACKSPACE/ESC: Shut down system");

            Console.CursorVisible = false;

            ConsoleKey inKey = Console.ReadKey().Key;
            if (inKey == ConsoleKey.Backspace || inKey == ConsoleKey.Escape)
            {
                CPU.DisableInterrupts();
                CPU.Halt();
            }
            else if (inKey == ConsoleKey.Enter)
            {
                Console.WriteLine("Deleting directories...");
                foreach (var subDir in new DirectoryInfo(@"0:\").GetDirectories())
                {
                    subDir.Delete(true);
                }

                Console.WriteLine("Deleting files...");
                foreach (var file in new DirectoryInfo(@"0:\").GetFiles())
                {
                    file.Delete();
                }
            }
            #endregion

            #region keyboardLayout
            layoutOptions = new string[] { "DE", "FR", "TR", "US" };

            while (state == 0)
            {
                clearScreenWHeader();

                Console.WriteLine("Please select a keyboard layout.\n\n");

                for (int i = 0; i < layoutOptions.Length; i++)
                {
                    if (sel == i)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.SetCursorPosition(0, Console.GetCursorPosition().Top - 1);

                    Console.WriteLine(layoutOptions[i] + new String(' ', 78));

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.CursorVisible = false;

                ConsoleKey k = Console.ReadKey().Key;

                if (k == ConsoleKey.DownArrow && sel < layoutOptions.Length - 1)
                {
                    sel++;
                }
                else if (k == ConsoleKey.UpArrow && sel > 0)
                {
                    sel--;
                }
                else if (k == ConsoleKey.Enter)
                {
                    state = 1;
                    configString = "kl:" + layoutOptions[sel]; // Keyboard Layout
                    switch (layoutOptions[sel])
                    {
                        case "DE":
                            KeyboardManager.SetKeyLayout(new DE_Standard());
                            break;
                        case "FR":
                            KeyboardManager.SetKeyLayout(new FR_Standard());
                            break;
                        case "TR":
                            KeyboardManager.SetKeyLayout(new TR_StandardQ());
                            break;
                        case "US":
                            KeyboardManager.SetKeyLayout(new US_Standard());
                            break;
                    }
                }
            }
            #endregion

            #region UserAccount
            clearScreenWHeader();

            Console.WriteLine("Please create a user account.\nNOTE: This account will have Administrator priviliges by default.\n\n");
            Console.Write("Username: ");

            Console.CursorVisible = true;

            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Console.CursorVisible = false;

            Console.WriteLine("\nCreating User account...");

            UMS.CreateUser(username, password, core.objects.PermissionLevel.Elevated, false);

            Console.WriteLine("\nSuccess!");

            state = 2;
            #endregion

            #region AutoSetup
            clearScreenWHeader();

            Console.WriteLine("Generating Config...");

            configString += "\nrx:1920"; // Resolution X
            configString += "\nry:1080"; // Resolution Y
            configString += "\nl:EN-US"; // TODO: Add language support
            configString += "\ndbm:1"; // Default Boot Mode - 0 = Menu, 1 = Console, 2 = Graphical. Set to 1 for now, as graphical is unfinished, so no point for a menu.
            configString += "\nv:v23.13.1"; // version
            configString += "\npa:1"; // PEL Activation

            Console.WriteLine("Saving Config to file...");

            var confFile = File.CreateText(@"0:\sysconfig.psc"); // PSC = Protected System Config, nothing special about it besides RainOS doesn't let you create, modify, or delete these files.
            confFile.WriteLine(configString);
            confFile.Close();
            #endregion

            clearScreenWHeader();

            Console.WriteLine("\n\nPlease press any key to reboot.");
            Console.ReadKey();

            Console.WriteLine("Rebooting...");

            CPU.Reboot();
        }

        private static void clearScreenWHeader()
        {
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            string title = "RainOS Setup";
            Console.WriteLine(new String(' ', 80));
            Console.SetCursorPosition(0, 1);
            Console.Write(new String(' ', 34));
            Console.Write(title);
            Console.WriteLine(new String(' ', 34));
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(new String(' ', 80));

            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
