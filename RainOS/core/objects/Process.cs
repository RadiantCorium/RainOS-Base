using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.objects
{
    internal abstract class Process
    {
        internal string name;
        internal int id;
        internal string description;
        internal PermissionLevel level;
        internal bool isCritical;
        internal bool isHidden;
        internal User user;

        internal Process(string name, int id, string desc, PermissionLevel level, bool isCritical, bool isHidden, User user)
        {
            this.name = name;
            this.id = id;
            this.description = desc;
            this.level = level;
            this.isCritical = isCritical;
            this.isHidden = isHidden;
            this.user = user;
        }

        internal abstract void init();

        internal abstract void update();

        internal abstract void destroy();
    }
}
