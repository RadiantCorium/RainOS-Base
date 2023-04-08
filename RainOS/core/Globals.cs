using Cosmos.System.FileSystem;
using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core
{
    public class Globals
    {
        private static Canvas _canvas;
        public static Canvas Canvas
        {
            get
            {
                return _canvas;
            }
            set
            {
                if (_canvas == null)
                {
                    _canvas = value;
                }
            }
        }

        private static CosmosVFS _vfs;
        public static CosmosVFS VFS
        {
            get
            {
                return _vfs;
            }
            set
            {
                if (_vfs == null)
                {
                    _vfs = value;
                }
            }
        }

        public static bool drawCursor = false;
    }
}
