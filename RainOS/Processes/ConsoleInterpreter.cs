using Cosmos.Core;
using Cosmos.Core.IOGroup;
using Cosmos.HAL;
using Cosmos.System;
using RainOS.core;
using RainOS.core.objects;
using RainOS.core.services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Console = System.Console;

namespace RainOS.Processes
{
    internal class ConsoleInterpreter : Process
    {
        string workingDir = @"0:\";

        public ConsoleInterpreter(int id, User user) : base("Console Mode", id, "The ConsoleMode command interpreter and renderer", user.PermissionLevel, true, false, user)
        {
        }

        internal override void destroy()
        {
            
        }

        internal override void init()
        {
            Console.Clear();
            Console.WriteLine(@" _______             _             ___     ______   
|_   __ \           (_)          .'   `. .' ____ \  
  | |__) |   ,--.   __   _ .--. /  .-.  \| (___ \_| 
  |  __ /   `'_\ : [  | [ `.-. || |   | | _.____`.  
 _| |  \ \_ // | |, | |  | | | |\  `-'  /| \____) | 
|____| |___|\'-;__/[___][___||__]`.___.'  \______.' 
                                                    ");
            Console.WriteLine("(C) thepercentageguy, 2023");
        }

        internal override void update()
        {
            Console.Write($"{user.Username}@{workingDir}> ");
            var input = Console.ReadLine();

            var splitInput = input.Split(' ');

            switch (splitInput[0])
            {
                case "version":
                    Console.WriteLine("RainOS Version " + Globals.sysconfig.data["v"]);
                    break;

                case "echo":
                    var final = "";
                    for (int i = 1; i < splitInput.Length; i++)
                    {
                        final += splitInput[i] + " ";
                    }
                    Console.WriteLine(final);
                    break;

                case "ls":
                case "dir":
                    if (splitInput.Length == 1)
                    {
                        foreach (string path in Directory.GetDirectories(workingDir))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(path + "\t");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        foreach (string path in Directory.GetFiles(workingDir))
                        {
                            Console.Write(path + "\t");
                        }
                    }
                    else
                    {
                        if (splitInput[1].StartsWith('.'))
                        {
                            splitInput[1] = Path.Combine(workingDir, splitInput[1].Substring(2));
                        }

                        if (!Directory.Exists(splitInput[1]))
                        {
                            Console.WriteLine("Directory '" + splitInput[1] + "' not found.");
                            break;
                        }

                        foreach (string path in Directory.GetDirectories(splitInput[1]))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(path + "\t");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        foreach (string path in Directory.GetFiles(splitInput[1]))
                        {
                            Console.Write(path + "\t");
                        }
                    }
                    Console.WriteLine();
                    break;

                case "sysconfig":
                    if (!PEL.CheckUserPermLevel(user.Username, PermissionLevel.Elevated))
                    {
                        Console.WriteLine("Access denied.");
                        break;
                    }
                    switch (splitInput[1])
                    {
                        case "save":
                            try
                            {
                                Globals.sysconfig.Save();
                                Console.WriteLine($"sysconfig saved sucessfully! Please reboot to apply changes fully.");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"There was an error while saving sysconfig: '{e.ToString()}'");
                            }
                            break;

                        case "load":
                            Globals.sysconfig.Load();
                            Console.WriteLine($"sysconfig loaded sucessfully!");
                            break;

                        case "set":
                            Globals.sysconfig.data[splitInput[2]] = splitInput[3];
                            break;

                        case "get":
                            if (!Globals.sysconfig.data.ContainsKey(splitInput[2]))
                            {
                                Console.WriteLine($"Key '{splitInput[2]}' is not defined!");
                                break;
                            }
                            Console.WriteLine(Globals.sysconfig.data[splitInput[2]]);
                            break;

                        case "list":
                            foreach (var kv in Globals.sysconfig.data)
                            {
                                Console.WriteLine(kv.Key + " : " + kv.Value);
                            }
                            break;

                        case "setdefault":
                            if (!Globals.defConf.ContainsKey(splitInput[2]))
                            {
                                Console.WriteLine($"Key '{splitInput[2]}' has no default value!");
                                break;
                            }
                            Globals.sysconfig.data[splitInput[2]] = Globals.defConf[splitInput[2]];
                            break;

                        case "remove":
                            if (!Globals.sysconfig.data.ContainsKey(splitInput[2]))
                            {
                                Console.WriteLine($"Key '{splitInput[2]}' is not defined!");
                                break;
                            }
                            Globals.sysconfig.data.Remove(splitInput[2]);
                            break;
                    }
                    break;

                case "reboot":
                    CPU.Reboot();
                    break;

                case "shutdown":
                    CPU.DisableInterrupts();
                    CPU.Halt();
                    break;

                case "pm":
                    switch (splitInput[1])
                    {
                        case "list":
                            foreach (Process p in PM.GetProcesses())
                            {
                                if (!p.isHidden)
                                {
                                    Console.WriteLine("N: " + p.name + ", ID: " + p.id + ", USRN: " + p.user.Username + ", CRIT: " + p.isCritical + ", LEV: " + (int)p.level);
                                }
                            }
                            break;

                        case "kill":
                            string processName = splitInput[2].Replace('_', ' ');
                            bool isID = int.TryParse(processName, out int id);
                            foreach (Process p in PM.GetProcesses())
                            {
                                if (isID)
                                {
                                    if (p.id == id)
                                    {
                                        PM.RemoveProcess(p, splitInput[3] == "force");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Process with ID '" + id + "' not found.");
                                    }
                                }
                                else
                                {
                                    if (p.name == name)
                                    {
                                        PM.RemoveProcess(p, splitInput[3] == "force");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Process with Name '" + id + "' not found.");
                                    }
                                }
                            }
                            break;
                    }

                    break;

                default:
                    Console.WriteLine("Unknown command '" + splitInput[0] + "'");
                    break;
            }
        }
    }
}
