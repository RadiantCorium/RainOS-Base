using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.objects
{
    internal interface User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
    }
}
