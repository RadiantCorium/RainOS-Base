using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.objects
{
    internal class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string obj) : base(obj)
        {
            
        }
    }
}
