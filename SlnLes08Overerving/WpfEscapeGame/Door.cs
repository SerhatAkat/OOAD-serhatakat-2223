using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace WpfEscapeGame
{
    internal class Door : LockableItem
    {
        public Room LeadsTo { get; set; }

        public Door(string name, bool isLocked, Item key, Room leadsTo) : base(name, "A door", isLocked, key)
        {
            LeadsTo = leadsTo;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
