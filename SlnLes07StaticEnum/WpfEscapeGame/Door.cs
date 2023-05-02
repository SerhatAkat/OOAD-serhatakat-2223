using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace WpfEscapeGame
{
    internal class Door
    {
        public string Name { get; set; }
        public bool IsLocked { get; set; } = false;
        public Item Key { get; set; }
        public Room LeadsTo { get; set; }

        public Door(string name, bool isLocked, Item key, Room leadsTo)
        {
            Name = name;
            IsLocked = isLocked;
            Key = key;
            LeadsTo = leadsTo;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
