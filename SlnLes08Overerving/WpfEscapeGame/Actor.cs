using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class Actor
    {
        public string Name { get; }
        public string Description { get; }

        public Actor(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
