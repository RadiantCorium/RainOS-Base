using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainOS.core.objects
{
    public class Event
    {
        public string Name { get; }
        public dynamic Data { get; }

        public Event(string name, dynamic? data)
        {
            Name = name;
            Data = data;
        }
    }
}
