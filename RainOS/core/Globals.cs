using Cosmos.System.FileSystem;
using Cosmos.System.Graphics;
using RainOS.core.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core
{
    internal class Globals
    {
        public static CosmosVFS fs;

        public static Canvas canvas;

        public static bool consoleMode = true;

        public static PSCR sysconfig;

        public static Dictionary<string, string> defConf = new Dictionary<string, string>()
        {
            { "kl", "US" }, // Keyboard Layout
            { "rx", "1920" }, // ResX
            { "ry", "1080" }, // ResY
            { "l", "EN-US" }, // Language
            { "dbm", "1" },   // Default Boot Mode - 0 = Menu, 1 = Console, 2 = Graphical. Set to 1 for now, as graphical is unfinished, so no point for a menu.
            { "v", "23.13.1" }, // version
            { "pa", "1" }, // PEL Activation
            { "pn", "RainOS Base" } // Product Name
        };
    }
}
