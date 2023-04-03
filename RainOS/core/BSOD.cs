using Cosmos.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core
{
    internal class BSOD
    {
        internal static void Trigger(Exception ex)
        {
            if (Globals.canvas != null)
                Globals.canvas.Disable();

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            string title = "RainOS has encountered an unrecoverable exception!";
            Console.WriteLine(new String(' ', 80));
            Console.SetCursorPosition(0, 1);
            Console.Write(new String(' ', 15));
            Console.Write(title);
            Console.WriteLine(new String(' ', 15));
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(new String(' ', 80));

            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("We will quickly write an error log...");

            Console.WriteLine();

            string errorLog = "--CPU--";
            string name = "--CPU--";

            try
            {
                Console.WriteLine("Gathering CPU info...");

                errorLog = "--CPU--";

                errorLog += $"\n\tVendor: {CPU.GetCPUVendorName()}";
                errorLog += $"\n\tBrand String: {CPU.GetCPUBrandString()}";
                //errorLog += $"\n\tCycle Speed: {CPU.GetCPUCycleSpeed()}";
                errorLog += $"\n\tUptime: {CPU.GetCPUUptime()}";

                Console.WriteLine("Gathering Memory info...");

                errorLog += "\n\n--MEMORY--";
                errorLog += $"\n\tAmount (MB): {CPU.GetAmountOfRAM()}";
                //errorLog += $"\n\tMemory Map: {CPU.GetMemoryMap()}";

                Console.WriteLine("Gathering Error info...");

                errorLog += "\n\n--Error--";

                errorLog += $"\n\tError: {(ex != null ? ex.ToString() : "N/A")}";
                // errorLog += $"\n\tSource: {(ex.Source != null ? ex.Source : "N/A")}";
                // errorLog += $"\n\tStack Trace: {(ex.StackTrace != null ? ex.StackTrace : "N/A")}";
                //errorLog += $"\n\tType: {(ex.GetType() != null ? ex.GetType() : "N/A")}";

                Console.WriteLine("Finalizing...");

                errorLog += $"\n\n{DateTime.Now.ToString("yyyy.MM.dd - HH:mm:ss")}";

                Console.WriteLine("Saving to file...");

                name = $@"0:\errorlog-{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.txt";

                try
                {
                    StreamWriter s = File.CreateText(name);
                    s.WriteLine(errorLog);
                    s.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR WHILE SAVING LOG TO FILE! " + e.ToString());
                }
            }
            catch (Exception e)
            {
                // rewrite the header lolol

                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Clear();

                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(new String(' ', 80));
                Console.SetCursorPosition(0, 1);
                Console.Write(new String(' ', 15));
                Console.Write(title);
                Console.WriteLine(new String(' ', 15));
                Console.SetCursorPosition(0, 2);
                Console.WriteLine(new String(' ', 80));

                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine(errorLog);

                Console.WriteLine($"\nThere was an error while gathering the remaining data:\n\t" + e.ToString());

                Console.WriteLine("\nPress any key to power off the system...");

                Console.ReadKey();

                Cosmos.Core.CPU.DisableInterrupts();

                Cosmos.Core.CPU.Halt();
            }

            // rewrite the header lolol

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(new String(' ', 80));
            Console.SetCursorPosition(0, 1);
            Console.Write(new String(' ', 15));
            Console.Write(title);
            Console.WriteLine(new String(' ', 15));
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(new String(' ', 80));

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine(errorLog);

            Console.WriteLine($"\nLog has been saved to '{name}'");

            Console.WriteLine("\nPress any key to power off the system...");

            Console.ReadKey();

            Cosmos.Core.CPU.DisableInterrupts();

            Cosmos.Core.CPU.Halt();
        }
    }
}
