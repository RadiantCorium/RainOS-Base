using Cosmos.Core;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using Cosmos.System.ScanMaps;
using IL2CPU.API.Attribs;
using RainOS.core;
using RainOS.core.services;
using System;
using System.IO;
using Console = System.Console;
using Sys = Cosmos.System;

namespace RainOS
{
    public class Kernel : Sys.Kernel
    {
        protected override void BeforeRun()
        {
            try
            {
                Console.WriteLine("Initializing Filesystem...");

                core.Globals.fs = new CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(core.Globals.fs);

                Console.WriteLine("Initializing UMS...");

                core.services.UMS.Init();

                Console.WriteLine("Checking System Status...");

                if (!File.Exists(@"0:\sysconfig.psc"))
                {
                    FTS.Open();
                }

                Console.WriteLine("Initializing PM...");
                PM.Init();

                Console.WriteLine("Initializing PSCR...");
                Globals.sysconfig = new PSCR(@"0:\sysconfig.psc");

                Console.WriteLine("Checking for missing config...");
                var amntUpdated = 0;
                foreach (var kv in Globals.defConf)
                {
                    if (!Globals.sysconfig.data.ContainsKey(kv.Key))
                    {
                        Globals.sysconfig.data[kv.Key] = kv.Value;
                        amntUpdated++;
                    }
                }

                Console.WriteLine($"Found {amntUpdated} missing config string(s)!");

                if (amntUpdated > 0)
                {
                    Console.WriteLine($"Saving updated config...");
                    Globals.sysconfig.Save();
                    Console.WriteLine($"Rebooting...");
                    CPU.Reboot();
                }

                Console.WriteLine("Applying Configuration...");
                switch (Globals.sysconfig.data["kl"])
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

                switch (Globals.sysconfig.data["dbm"])
                {
                    case "0": // boot menu
                        // TODO: Add boot menu
                        BSOD.Trigger(new Exception("Unsupported DBM setting"));
                        break;

                    case "1": // console mode
                        PM.AddProcess(new Processes.ConsoleModeLogonUI(new Random().Next(100000, 999999)));
                        break;

                    case "2": // graphical mode
                        BSOD.Trigger(new Exception("Unsupported DBM setting"));
                        break;
                        
                    default:
                        BSOD.Trigger(new Exception("Unsupported DBM setting"));
                        break;
                }
            }
            catch (Exception e)
            {
                BSOD.Trigger(e);
            }
        }

        protected override void Run()
        {
            try
            {
                PM.UpdateAll();
            }
            catch (Exception e)
            {
                BSOD.Trigger(e);
            }
        }
    }
}
