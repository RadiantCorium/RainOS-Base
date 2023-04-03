using Cosmos.Core;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
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
