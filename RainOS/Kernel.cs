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
using Globals = RainOS.core.Globals;
using Cosmos.Core.Memory;
using System.Drawing;

namespace RainOS
{
    public class Kernel : Sys.Kernel
    {
        private bool displayLoadingAnimation = false;

        private int TCycles = 0;

        protected override void BeforeRun()
        {
            try
            {
                Heap.Init();

                Console.WriteLine("Initializing event service...");
                EventService.Init();

                Console.WriteLine("Subscribing to 'kernel' event channel...");
                EventService.RegisterHandler("kernel", HandleEvents);

                Console.WriteLine("Initializing filesystem...");
                Globals.VFS = new CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(Globals.VFS);

                Console.WriteLine("Initializing UI...");
                Globals.Canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(1280, 720, ColorDepth.ColorDepth32));

                DrawLoading("Loading...");

                DrawLoading("Initializing Process Manager...");

            }
            catch (Exception e)
            {
                if (core.Globals.Canvas == null)
                    core.BSOD.Fallback.Trigger(e);
                // TODO: new BSoD
            }
        }

        protected override void Run()
        {
            try
            {
                // process manager handle here

                if (core.Globals.Canvas != null)
                {
                    Globals.Canvas.Display();
                }

                EventService.ProcessEvents();

                // garbage collection

                if (TCycles++ == 100)
                {
                    TCycles = 0;
                    Heap.Collect();
                }
            }
            catch (Exception e)
            {
                if (core.Globals.Canvas == null)
                    core.BSOD.Fallback.Trigger(e);
                // TODO: new BSoD
            }
        }

        private void HandleEvents(core.objects.Event ev)
        {
            Console.WriteLine("Recieved event: " + ev.Name);
        }

        private void DrawLoading(string loadingstring)
        {
            Globals.Canvas.Clear(Color.FromArgb(255, 100, 100, 100));
            Globals.Canvas.DrawString(
                loadingstring,
                PCScreenFont.Default,
                new Pen(Color.White),
                new Sys.Graphics.Point(
                    Globals.Canvas.Mode.Columns / 2 - (loadingstring.Length * PCScreenFont.Default.Width) / 2,
                    Globals.Canvas.Mode.Rows / 2 - (PCScreenFont.Default.Height) / 2
                )
            );
            Globals.Canvas.Display();
        }
    }
}
