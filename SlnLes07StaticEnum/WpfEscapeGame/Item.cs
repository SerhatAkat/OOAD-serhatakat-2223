using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsLocked { get; set; } = false;
        public bool IsPortable { get; set; } = true;
        public Item Key { get; set; }
        public Item HiddenItem { get; set; }

        public Item(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

        public Item(string name, string desc, bool isPortable)
        {
            Name = name;
            Description = desc;
            IsPortable = isPortable;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
